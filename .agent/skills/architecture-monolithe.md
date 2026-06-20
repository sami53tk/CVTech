# Skill — Architecture Monolithe Modulaire CVTech

## Responsabilités des couches (par module)

| Couche | Rôle | Règle |
|--------|------|-------|
| `Client/` | Endpoints Minimal API, DTOs HTTP, mapping requête→commande | Ne contient aucune logique métier |
| `Application/Features/<Feature>/` | Command/Query (IRequest), Handler (IRequestHandler), Validator (AbstractValidator) | Un seul handler par feature, MediatR vertical-slice |
| `Domaine/` | Entités riches, Value Objects, exceptions métier | Langage ubiquitaire français ; zéro dépendance externe |
| `Infrastructure/` | DbContext EF Core (SQLite), configurations Fluent API, migrations, implémentations de contrats | Seule couche qui dépend d'EF Core |
| `ModuleLoader.cs` | Point d'entrée unique pour DI et routage | Méthodes d'extension : `AjouterModule*`, `MapperEndpoints*`, `Migrer*Async` |

## Règles d'isolation strictes

- **Interdiction absolue** de référencer le `DbContext` d'un autre module — chaque module possède exactement **1 DbContext** → **1 fichier `.db`**.
- La communication inter-modules se fait **uniquement** via :
  1. Les contrats publics de `CVTech.BuildingBlocks` (`IVerificateurPermission`, `IReferentielDomaineMetier`).
  2. Le bus d'événements in-memory (`IBusEvenements` + `IGestionnaireEvenement<T>`).
- **Jamais** de `ProjectReference` d'un module vers un autre module.

## Convention de nommage

- **Domaine / Application** → français (noms de classes, méthodes, propriétés, exceptions).
- **Infrastructure / technique** → anglais (`DbContext`, `ConfigureServices`, migrations EF).

## Pattern ModuleLoader

```csharp
// Enregistrement (Program.cs)
builder.Services.AjouterModuleXxx(builder.Configuration);

// Routage (Program.cs)
app.MapperEndpointsXxx();

// Migration au démarrage (Program.cs)
await app.Services.MigrerXxxAsync();
```

## Pattern vertical-slice MediatR

Chaque feature = 1 sous-dossier contenant exactement :
- `<Feature>Command.cs` ou `<Feature>Query.cs` — record `IRequest<T>`
- `<Feature>Handler.cs` — `IRequestHandler<TRequest, TResponse>` ou `IRequestHandler<TRequest>` (sans réponse → `Task`, jamais `Task<Unit>`)
- `<Feature>Validator.cs` *(optionnel)* — `AbstractValidator<TCommand>`, enregistré via `AddValidatorsFromAssembly`

Le behavior `ComportementValidation<,>` est enregistré en open-generic sur la pipeline MediatR : toute commande avec un validator associé est validée automatiquement avant l'exécution du handler.
