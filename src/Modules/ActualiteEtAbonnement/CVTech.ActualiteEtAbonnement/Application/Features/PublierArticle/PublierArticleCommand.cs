using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.ActualiteEtAbonnement.Application.Features.PublierArticle;

public sealed record PublierArticleCommand(
    RoleUtilisateur RoleActeur,
    Guid AuteurId,
    string Titre,
    string Contenu,
    string? DomaineMetierCode,
    string LienExterne
) : IRequest<PublierArticleResultat>;

public sealed record PublierArticleResultat(Guid ArticleId);
