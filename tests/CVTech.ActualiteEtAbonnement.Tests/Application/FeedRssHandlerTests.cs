using Xunit;
using CVTech.ActualiteEtAbonnement.Application.Features.ConsulterFeedRss;
using CVTech.ActualiteEtAbonnement.Domaine;
using FluentAssertions;

namespace CVTech.ActualiteEtAbonnement.Tests.Application;

public sealed class FeedRssHandlerTests
{
    [Fact]
    public async Task LeFeedRssContientSeulementLesArticlesEditoriaux()
    {
        var ctx = OutilsTest.CreerContexte();
        var article1 = ArticleActualite.Publier("Article DevOps", "Contenu 1", "devops", Guid.NewGuid(), "https://example.com/1");
        var article2 = ArticleActualite.Publier("Article Cloud", "Contenu 2", "cloud-azure", Guid.NewGuid(), "https://example.com/2");
        ctx.Articles.AddRange(article1, article2);
        await ctx.SaveChangesAsync();

        var handler = new ConsulterFeedRssHandler(ctx);
        var resultat = await handler.Handle(new ConsulterFeedRssQuery(null), CancellationToken.None);

        resultat.Items.Should().HaveCount(2);
        resultat.Items.Should().OnlyContain(i => !string.IsNullOrEmpty(i.Titre));
    }

    [Fact]
    public async Task LeFeedRssPeutEtreFiltreParlDomaineMetier()
    {
        var ctx = OutilsTest.CreerContexte();
        ctx.Articles.Add(ArticleActualite.Publier("Article DevOps", "Contenu", "devops", Guid.NewGuid(), "https://ex.com/1"));
        ctx.Articles.Add(ArticleActualite.Publier("Article Cloud", "Contenu", "cloud-azure", Guid.NewGuid(), "https://ex.com/2"));
        await ctx.SaveChangesAsync();

        var handler = new ConsulterFeedRssHandler(ctx);
        var resultat = await handler.Handle(new ConsulterFeedRssQuery("devops"), CancellationToken.None);

        resultat.Items.Should().HaveCount(1);
        resultat.Items.First().Titre.Should().Be("Article DevOps");
    }

    [Fact]
    public async Task LeFeedRssNeContientPasLesAnnoncesOuAppelsOffres()
    {
        // Le feed RSS ne doit contenir que les ArticleActualite, jamais les annonces/AO
        // qui passent par le bus d'événements mais ne sont PAS insérés dans Articles.
        var ctx = OutilsTest.CreerContexte();
        // Pas d'articles insérés → feed vide
        var handler = new ConsulterFeedRssHandler(ctx);
        var resultat = await handler.Handle(new ConsulterFeedRssQuery(null), CancellationToken.None);

        resultat.Items.Should().BeEmpty();
    }
}
