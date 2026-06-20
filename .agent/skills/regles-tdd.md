# Skill — Règles TDD CVTech

## Convention de nommage (français obligatoire)

Les noms de tests suivent le pattern : **`UnX[Ne]PeutPasY`** ou **`UnXPeutY`**

```csharp
// ✅ Correct
public async Task UnCandidatNePeutPasPublierUneAnnonce() { ... }
public async Task UneEntrepriseProprietairePeutConsulterLesCandidatures() { ... }
public async Task LeFeedRssNeContientPasLesAnnoncesDEmploi() { ... }

// ❌ Interdit
public async Task TestPublierAnnonce() { ... }
public async Task PublierAnnonceReturnsError_WhenRoleIsCandidat() { ... }
```

## Structure d'un test (AAA)

```csharp
[Fact]
public async Task UnCandidatNePeutPasPublierUneAnnonce()
{
    // Arrange
    var handler = new PublierAnnonceHandler(OutilsTest.CreerContexte(), OutilsTest.CreerVerificateurPermission(), OutilsTest.CreerBusEvenements());
    var cmd = new PublierAnnonceCommand(RoleUtilisateur.Candidat, Guid.NewGuid(), "Titre", "Desc", TypeContrat.Cdi, "dev-web", "Paris");

    // Act & Assert
    await Assert.ThrowsAsync<PermissionRefuseeException>(() =>
        handler.Handle(cmd, CancellationToken.None));
}
```

## Outils de test (`OutilsTest.cs` par projet)

Chaque projet de tests possède un `OutilsTest` statique qui expose :

| Méthode | Retourne | Description |
|---------|----------|-------------|
| `CreerContexte()` | `XxxDbContext` | DbContext EF InMemory avec GUID unique (isolation) |
| `CreerVerificateurPermission()` | `IVerificateurPermission` | Implémentation complète de la matrice |
| `CreerReferentielDomaineMetier()` | `IReferentielDomaineMetier` | 3 domaines fixes : dev-web, cloud-azure, data-science |
| `CreerBusEvenements()` | `IBusEvenements` | Bus test qui accumule les événements dans une liste |

## Disciplines Red → Green → Refactor

1. **Red** — Écrire le test qui échoue avant d'écrire le code de production.
2. **Green** — Écrire le minimum de code pour faire passer le test.
3. **Refactor** — Nettoyer sans casser les tests verts.

## Organisation des projets de tests

```
tests/
├── CVTech.GestionIdentite.Tests/
│   ├── Application/       ← tests des handlers (unit)
│   └── OutilsTest.cs
├── CVTech.CatalogueEmploi.Tests/
│   ├── Application/
│   └── OutilsTest.cs
├── CVTech.AppelOffreFreelance.Tests/
│   ├── Application/
│   └── OutilsTest.cs
└── CVTech.ActualiteEtAbonnement.Tests/
    ├── Application/
    └── OutilsTest.cs
```

## Ce qu'on teste

- **Handlers** : logique métier, permissions, règles domaine (exceptions levées).
- **Validators** : règles de validation asynchrones (ex: domaine métier inexistant).
- **Gestionnaires d'événements** : création de notifications pour les abonnés.
- **Feed RSS** : contient uniquement les articles éditoriaux (pas les annonces/AO).

## Ce qu'on ne teste PAS dans les projets unitaires

- Les endpoints HTTP (testé via smoke-test live après `dotnet run`).
- Les migrations EF Core (vérifiées au démarrage de l'API).
- L'infrastructure réelle SQLite (DbContext InMemory suffit pour les handlers).

## Lancer les tests

```bash
dotnet test                                    # tous les modules
dotnet test tests/CVTech.CatalogueEmploi.Tests # module spécifique
```

Résultat attendu : **0 échec**.
