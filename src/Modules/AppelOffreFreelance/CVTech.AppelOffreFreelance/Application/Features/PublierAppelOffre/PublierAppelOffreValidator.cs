using CVTech.BuildingBlocks.Domaine;
using FluentValidation;

namespace CVTech.AppelOffreFreelance.Application.Features.PublierAppelOffre;

public sealed class PublierAppelOffreValidator : AbstractValidator<PublierAppelOffreCommand>
{
    public PublierAppelOffreValidator(IReferentielDomaineMetier referentielDomaineMetier)
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Localisation).NotEmpty().MaximumLength(128);
        RuleFor(x => x.BudgetMax).GreaterThan(0);
        RuleFor(x => x.DomaineMetierCode)
            .NotEmpty()
            .MustAsync((code, ct) => referentielDomaineMetier.ExisteAsync(code, ct))
            .WithMessage("Le domaine métier '{PropertyValue}' n'existe pas.");
    }
}
