using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.ConsulterMonCv;

public sealed record ConsulterMonCvQuery(Guid CandidatId) : IRequest<CvResultat?>;

public sealed record CvResultat(Guid Id, string Titre, string Resume, IReadOnlyCollection<string> Competences, DateTimeOffset DateDerniereModification);
