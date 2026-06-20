using CVTech.AppelOffreFreelance.Infrastructure;
using CVTech.BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Application.Features.ConsulterAppelOffre;

public sealed class ConsulterAppelOffreHandler : IRequestHandler<ConsulterAppelOffreQuery, AppelOffreDetail>
{
    private readonly AppelOffreFreelanceDbContext _dbContext;

    public ConsulterAppelOffreHandler(AppelOffreFreelanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppelOffreDetail> Handle(ConsulterAppelOffreQuery request, CancellationToken cancellationToken)
    {
        var a = await _dbContext.AppelsOffres
            .FirstOrDefaultAsync(a => a.Id == request.AppelOffreId && !a.EstSupprime, cancellationToken)
            ?? throw new EntiteIntrouvableException("AppelOffre", request.AppelOffreId);

        return new AppelOffreDetail(a.Id, a.Titre, a.Description, a.DomaineMetierCode,
            a.Localisation, a.BudgetMax, a.EntrepriseId, a.DateCreation, a.LaureatId);
    }
}
