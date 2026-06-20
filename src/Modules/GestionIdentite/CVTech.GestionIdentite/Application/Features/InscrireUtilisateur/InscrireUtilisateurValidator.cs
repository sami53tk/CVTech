using CVTech.BuildingBlocks.Permissions;
using FluentValidation;

namespace CVTech.GestionIdentite.Application.Features.InscrireUtilisateur;

public sealed class InscrireUtilisateurValidator : AbstractValidator<InscrireUtilisateurCommand>
{
    public InscrireUtilisateurValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.MotDePasse).NotEmpty().MinimumLength(8)
            .WithMessage("Le mot de passe doit contenir au moins 8 caractères.");

        RuleFor(x => x.Role)
            .NotEqual(RoleUtilisateur.Administrateur)
            .WithMessage("L'inscription en tant qu'administrateur n'est pas autorisée.");

        When(x => x.Role == RoleUtilisateur.Candidat, () =>
        {
            RuleFor(x => x.Nom).NotEmpty().WithMessage("Le nom est obligatoire pour un candidat.");
            RuleFor(x => x.Prenom).NotEmpty().WithMessage("Le prénom est obligatoire pour un candidat.");
        });

        When(x => x.Role == RoleUtilisateur.Entreprise, () =>
        {
            RuleFor(x => x.RaisonSociale).NotEmpty().WithMessage("La raison sociale est obligatoire pour une entreprise.");
        });
    }
}
