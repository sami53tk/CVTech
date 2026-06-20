using CVTech.ActualiteEtAbonnement.Domaine;
using CVTech.ActualiteEtAbonnement.Infrastructure;
using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.ActualiteEtAbonnement.Application.Features.PublierArticle;

public sealed class PublierArticleHandler : IRequestHandler<PublierArticleCommand, PublierArticleResultat>
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public PublierArticleHandler(ActualiteEtAbonnementDbContext dbContext,
        IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task<PublierArticleResultat> Handle(PublierArticleCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.PublierArticleActualite);

        var article = ArticleActualite.Publier(
            request.Titre, request.Contenu, request.DomaineMetierCode,
            request.AuteurId, request.LienExterne);

        _dbContext.Articles.Add(article);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new PublierArticleResultat(article.Id);
    }
}
