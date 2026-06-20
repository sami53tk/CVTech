# Skill — Règles de Permissions CVTech

## Principe fondamental

**Tout handler qui modifie un état ou expose des données sensibles doit appeler `IVerificateurPermission` en première instruction.**

```csharp
public async Task<T> Handle(MaCommandeCommand request, CancellationToken ct)
{
    _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.MonAction); // ← 1re ligne
    // suite de la logique métier...
}
```

Si la permission est refusée, `VerifierPermission` lève immédiatement `PermissionRefuseeException` (HTTP 403).

## Matrice de permissions

| Action (`Permissions.*`) | Candidat | Entreprise | Administrateur |
|--------------------------|----------|------------|----------------|
| `ConstituerCv` | ✅ | ❌ | ✅ |
| `PostulerAnnonce` | ✅ | ❌ | ❌ |
| `SoumettrePropositionFreelance` | ✅ | ❌ | ❌ |
| `PublierAnnonce` | ❌ | ✅ | ✅ |
| `PublierAppelOffre` | ❌ | ✅ | ✅ |
| `ConsulterCandidaturesRecues` | ❌ | ✅ (propres annonces) | ✅ (toutes) |
| `ConsulterPropositionsRecues` | ❌ | ✅ (propres AO) | ✅ (tous) |
| `SelectionnerLaureat` | ❌ | ✅ (propres AO) | ✅ |
| `SAbonnerDomaineMetier` | ✅ | ✅ | ✅ |
| `PublierArticleActualite` | ❌ | ❌ | ✅ |
| `ModererAnnonceOuAppelOffre` | ❌ | ❌ | ✅ |
| `BloquerReactiverCompte` | ❌ | ❌ | ✅ |
| `GererReferentielDomaines` | ❌ | ❌ | ✅ |

## Exceptions métier de sécurité

| Exception | HTTP | Déclencheur |
|-----------|------|-------------|
| `PermissionRefuseeException` | 403 | Rôle non autorisé pour l'action |
| `CompteBloqueException` | 403 | Compte suspendu par un admin |
| `EntiteIntrouvableException` | 404 | Entité inexistante ou appartenant à un autre utilisateur |

## Règle de propriété (ownership)

Pour les ressources appartenant à une entreprise (annonces, AO) :

```csharp
if (request.RoleActeur != RoleUtilisateur.Administrateur
    && ressource.EntrepriseId != request.EntrepriseId)
{
    throw new PermissionRefuseeException(Permissions.ConsulterXxx);
}
```

L'Administrateur contourne toujours la vérification de propriété.

## Extraction du rôle depuis le JWT

```csharp
var role = ctx.User.ObtenirRole();             // RoleUtilisateur enum
var userId = ctx.User.ObtenirUtilisateurId();  // Guid
```

Ces méthodes d'extension sont dans `CVTech.BuildingBlocks.Securite.ClaimsPrincipalExtensions`.
