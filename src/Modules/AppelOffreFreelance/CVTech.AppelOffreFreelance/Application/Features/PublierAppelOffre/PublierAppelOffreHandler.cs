using CVTech.AppelOffreFreelance.Domaine;
using CVTech.AppelOffreFreelance.Infrastructure;
using CVTech.BuildingBlocks.Evenements;
using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.AppelOffreFreelance.Application.Features.PublierAppelOffre;

public sealed class PublierAppelOffreHandler : IRequestHandler<PublierAppelOffreCommand, PublierAppelOffreResultat>
{
    private readonly AppelOffreFreelanceDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;
    private readonly IBusEvenements _busEvenements;

    public PublierAppelOffreHandler(AppelOffreFreelanceDbContext dbContext,
        IVerificateurPermission verificateurPermission, IBusEvenements busEvenements)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
        _busEvenements = busEvenements;
    }

    public async Task<PublierAppelOffreResultat> Handle(PublierAppelOffreCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.PublierAppelOffre);

        var appelOffre = AppelOffre.Publier(
            request.Titre, request.Description, request.DomaineMetierCode,
            request.Localisation, request.BudgetMax, request.EntrepriseId);

        _dbContext.AppelsOffres.Add(appelOffre);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _busEvenements.PublierAsync(new AppelOffrePublieEvent(
            appelOffre.Id, appelOffre.Titre, appelOffre.DomaineMetierCode,
            appelOffre.EntrepriseId, appelOffre.DateCreation));

        return new PublierAppelOffreResultat(appelOffre.Id);
    }
}
