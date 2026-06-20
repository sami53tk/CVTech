using FluentValidation;

namespace CVTech.CatalogueEmploi.Application.Features.ConstituerCv;

public sealed class ConstituerCvValidator : AbstractValidator<ConstituerCvCommand>
{
    public ConstituerCvValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Resume).NotEmpty();
    }
}
