using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Domaine;
using CVTech.GestionIdentite.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Permissions = CVTech.BuildingBlocks.Permissions.Permissions;

namespace CVTech.GestionIdentite.Tests.Domaine;

public sealed class MatricePermissionsTests
{
    [Theory]
    // --- Candidat ---
    [InlineData(RoleUtilisateur.Candidat, Permissions.ConstituerCv, true)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.PostulerAnnonce, true)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.SoumettrePropositionFreelance, true)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.SAbonnerDomaineMetier, true)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.PublierAnnonce, false)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.PublierAppelOffre, false)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.ConsulterCandidaturesRecues, false)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.ConsulterPropositionsRecues, false)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.PublierArticleActualite, false)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.ModererAnnonceOuAppelOffre, false)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.BloquerReactiverCompte, false)]
    [InlineData(RoleUtilisateur.Candidat, Permissions.GererReferentielDomaines, false)]
    // --- Entreprise ---
    [InlineData(RoleUtilisateur.Entreprise, Permissions.ConstituerCv, false)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.PostulerAnnonce, false)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.SoumettrePropositionFreelance, false)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.PublierAnnonce, true)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.PublierAppelOffre, true)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.ConsulterCandidaturesRecues, true)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.ConsulterPropositionsRecues, true)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.SAbonnerDomaineMetier, true)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.PublierArticleActualite, false)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.ModererAnnonceOuAppelOffre, false)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.BloquerReactiverCompte, false)]
    [InlineData(RoleUtilisateur.Entreprise, Permissions.GererReferentielDomaines, false)]
    // --- Administrateur ---
    [InlineData(RoleUtilisateur.Administrateur, Permissions.ConstituerCv, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.PostulerAnnonce, false)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.SoumettrePropositionFreelance, false)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.PublierAnnonce, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.PublierAppelOffre, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.ConsulterCandidaturesRecues, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.ConsulterPropositionsRecues, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.SAbonnerDomaineMetier, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.PublierArticleActualite, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.ModererAnnonceOuAppelOffre, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.BloquerReactiverCompte, true)]
    [InlineData(RoleUtilisateur.Administrateur, Permissions.GererReferentielDomaines, true)]
    public void LaMatricePermissionsRespecteLeReferentielDuReadme(RoleUtilisateur role, string action, bool estAutorise)
    {
        MatricePermissions.ARoleLaPermission(role, action).Should().Be(estAutorise);
    }

    [Fact]
    public void UnCandidatNePeutPasModererUneAnnonce()
    {
        var options = new DbContextOptionsBuilder<IdentiteDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var dbContext = new IdentiteDbContext(options);
        var verificateur = new VerificateurPermission(dbContext);

        var action = () => verificateur.VerifierPermission(RoleUtilisateur.Candidat, Permissions.ModererAnnonceOuAppelOffre);

        action.Should().Throw<PermissionRefuseeException>();
    }
}
