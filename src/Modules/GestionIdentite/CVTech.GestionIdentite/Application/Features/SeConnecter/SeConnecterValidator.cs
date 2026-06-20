using FluentValidation;

namespace CVTech.GestionIdentite.Application.Features.SeConnecter;

public sealed class SeConnecterValidator : AbstractValidator<SeConnecterCommand>
{
    public SeConnecterValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.MotDePasse).NotEmpty();
    }
}
