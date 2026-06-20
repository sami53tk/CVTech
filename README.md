# CVTech — Plateforme de Recrutement Tech

Monolithe modulaire .NET 10, DDD, MediatR, EF Core SQLite, frontend React + Vite + TypeScript.

---

## Architecture

```mermaid
graph TB
    subgraph "Frontend (React + Vite)"
        UI_Candidat["Espace Candidat<br/>CV · Candidatures · Abonnements · Notifications"]
        UI_Entreprise["Espace Entreprise<br/>Annonces · Appels d'offre · Lauréat"]
        UI_Admin["Espace Admin<br/>Modération · Articles · Référentiel"]
        UI_Public["Pages publiques<br/>Annonces · Appels d'offre · Flux RSS"]
    end

    subgraph "API Host (ASP.NET Core 10)"
        JWT["JWT Auth Middleware"]
        ExHandler["Exception Handler"]
        subgraph "Module A — GestionIdentite"
            A_Domain["Domaine : Utilisateur · Profil<br/>MatricePermissions"]
            A_App["Application : Inscription · Connexion<br/>BloquerCompte · ConsulterProfil"]
            A_Infra["Infrastructure : IdentiteDbContext<br/>GenerateurJwt · VerificateurPermission"]
        end
        subgraph "Module B — CatalogueEmploi"
            B_Domain["Domaine : AnnonceEmploi<br/>CurriculumVitae · Candidature"]
            B_App["Application : PublierAnnonce · PostulerAnnonce<br/>ConstituerCV · ConsulterCandidatures"]
            B_Infra["Infrastructure : EmploiDbContext"]
        end
        subgraph "Module C — AppelOffreFreelance"
            C_Domain["Domaine : AppelOffre<br/>PropositionFreelance"]
            C_App["Application : PublierAO · SoumettreProposition<br/>SelectionnerLaureat"]
            C_Infra["Infrastructure : AppelOffreFreelanceDbContext"]
        end
        subgraph "Module D — ActualiteEtAbonnement"
            D_Domain["Domaine : ArticleActualite<br/>Abonnement · Notification"]
            D_App["Application : PublierArticle · FeedRss<br/>GererAbonnement · ConsulterNotifications"]
            D_Infra["Infrastructure : ActualiteDbContext<br/>GestionnaireAnnoncePubliee"]
        end
    end

    subgraph "BuildingBlocks (partagés)"
        BB["IVerificateurPermission · IBusEvenements<br/>IGestionnaireEvenement · Permissions<br/>AnnoncePublieeEvent · AppelOffrePublieEvent<br/>EntiteBase · AgregatRacine"]
    end

    subgraph "Données (SQLite)"
        DB_A[("identite.db")]
        DB_B[("emploi.db")]
        DB_C[("appel_offre_freelance.db")]
        DB_D[("actualite.db")]
    end

    UI_Candidat -->|HTTPS + JWT| JWT
    UI_Entreprise -->|HTTPS + JWT| JWT
    UI_Admin -->|HTTPS + JWT| JWT
    UI_Public -->|HTTP public| JWT

    JWT --> A_App
    JWT --> B_App
    JWT --> C_App
    JWT --> D_App

    A_App --> A_Domain
    A_App --> A_Infra
    B_App --> B_Domain
    B_App --> B_Infra
    C_App --> C_Domain
    C_App --> C_Infra
    D_App --> D_Domain
    D_App --> D_Infra

    A_Infra --> DB_A
    B_Infra --> DB_B
    C_Infra --> DB_C
    D_Infra --> DB_D

    B_App -->|AnnoncePublieeEvent| BB
    C_App -->|AppelOffrePublieEvent| BB
    BB -->|dispatch| D_Infra

    A_Infra -.->|implémente| BB
    B_App -.->|utilise| BB
    C_App -.->|utilise| BB
    D_App -.->|utilise| BB
```

---

## Prérequis

- .NET SDK 10.0.x
- Node.js 18+ / npm 9+
- (Optionnel) `dotnet-ef` : `dotnet tool install --global dotnet-ef`

---

## Lancement pas à pas

### 1. Restaurer et compiler le backend

```bash
cd CVTech
dotnet restore
dotnet build
```

### 2. Lancer l'API

```bash
dotnet run --project src/Api/CVTech.Api
```

L'API démarre sur `http://localhost:5298` (ou le port affiché).  
Les migrations EF Core et les données de seed s'appliquent automatiquement au premier démarrage.

### 3. Lancer le frontend

Dans un second terminal :

```bash
cd frontend
npm install
npm run dev
```

Le frontend démarre sur `http://localhost:5173`.

---

## Comptes de seed

| Rôle | Email | Mot de passe |
|------|-------|--------------|
| Administrateur | `admin@cvtech.fr` | `Admin1234!` |
| Entreprise | `techcorp@cvtech.fr` | `Entreprise1!` |
| Candidat | `candidat@cvtech.fr` | `Candidat1!` |

---

## Tests

```bash
dotnet test
```

**80 tests — 0 échec** :
- GestionIdentite : 44 tests (matrice de permissions, JWT, compte bloqué)
- CatalogueEmploi : 17 tests (permissions, émission d'événement, accès public)
- AppelOffreFreelance : 12 tests (propriété, sélection lauréat, modération)
- ActualiteEtAbonnement : 7 tests (RSS, abonnement → notification)

---

## Flux RSS

```bash
curl http://localhost:5298/feed/rss
```

Renvoie du XML RSS 2.0 contenant uniquement les articles éditoriaux.  
Filtrable par domaine : `curl "http://localhost:5298/feed/rss?domaine=dev-web"`

---

## Démo du flux de notification

1. Se connecter en tant que **Candidat** (`candidat@cvtech.fr`)
2. Espace Candidat → onglet **Abonnements** → s'abonner au domaine `dev-web`
3. Se connecter en tant qu'**Entreprise** (`techcorp@cvtech.fr`)
4. Espace Entreprise → **Publier une annonce** avec le domaine `dev-web`
5. Se reconnecter en tant que **Candidat**
6. Espace Candidat → onglet **Notifications** : la notification apparaît

Le log de l'API affiche également une ligne `[EMAIL] Notification envoyée à candidat@cvtech.fr`.

---

## Structure du projet

```
CVTech/
├── src/
│   ├── BuildingBlocks/CVTech.BuildingBlocks/   # Contrats partagés
│   ├── Modules/
│   │   ├── GestionIdentite/CVTech.GestionIdentite/
│   │   ├── CatalogueEmploi/CVTech.CatalogueEmploi/
│   │   ├── AppelOffreFreelance/CVTech.AppelOffreFreelance/
│   │   └── ActualiteEtAbonnement/CVTech.ActualiteEtAbonnement/
│   └── Api/CVTech.Api/                          # Composition root
├── tests/
│   ├── CVTech.GestionIdentite.Tests/
│   ├── CVTech.CatalogueEmploi.Tests/
│   ├── CVTech.AppelOffreFreelance.Tests/
│   └── CVTech.ActualiteEtAbonnement.Tests/
├── frontend/                                    # React + Vite + TypeScript
└── .agent/skills/                               # Fichiers de compétences IA
    ├── architecture-monolithe.md
    ├── regles-permissions.md
    └── regles-tdd.md
```

---

## Matrice de permissions (résumé)

| Action | Candidat | Entreprise | Admin |
|--------|----------|------------|-------|
| Publier annonce | ✗ | ✓ | ✓ |
| Postuler à une annonce | ✓ | ✗ | ✗ |
| Constituer son CV | ✓ | ✗ | ✗ |
| Consulter les candidatures reçues | ✗ | ✓ (propres) | ✓ |
| Modérer une annonce | ✗ | ✗ | ✓ |
| Publier appel d'offre | ✗ | ✓ | ✓ |
| Soumettre une proposition | ✓ | ✗ | ✗ |
| Sélectionner un lauréat | ✗ | ✓ (propre AO) | ✓ |
| Gérer abonnements | ✓ | ✗ | ✗ |
| Publier article éditorial | ✗ | ✗ | ✓ |
| Gérer référentiel domaines | ✗ | ✗ | ✓ |
| Bloquer un compte | ✗ | ✗ | ✓ |
