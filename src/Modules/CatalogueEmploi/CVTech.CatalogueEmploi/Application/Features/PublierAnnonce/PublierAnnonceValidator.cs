using CVTech.BuildingBlocks.Domaine;
using FluentValidation;

namespace CVTech.CatalogueEmploi.Application.Features.PublierAnnonce;

public sealed class PublierAnnonceValidator : AbstractValidator<PublierAnnonceCommand>
{
    public PublierAnnonceValidator(IReferentielDomaineMetier referentielDomaineMetier)
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Localisation).NotEmpty().MaximumLength(128);

        RuleFor(x => x.DomaineMetierCode)
            .NotEmpty()
            .MustAsync((code, ct) => referentielDomaineMetier.ExisteAsync(code, ct))
            .WithMessage("Le domaine métier indiqué n'existe pas dans le référentiel.");
    }
}
