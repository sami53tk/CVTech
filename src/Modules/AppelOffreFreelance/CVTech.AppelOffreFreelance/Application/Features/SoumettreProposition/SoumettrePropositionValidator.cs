using FluentValidation;

namespace CVTech.AppelOffreFreelance.Application.Features.SoumettreProposition;

public sealed class SoumettrePropositionValidator : AbstractValidator<SoumettrePropositionCommand>
{
    public SoumettrePropositionValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.TauxJournalier).GreaterThan(0);
        RuleFor(x => x.DureeEstimeeJours).GreaterThan(0);
    }
}
