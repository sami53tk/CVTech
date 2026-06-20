namespace CVTech.BuildingBlocks.Permissions;

/// <summary>
/// Constantes des actions métier soumises à permission, telles que listées dans la
/// « Matrice de Permissions » du README. Chaque handler de cas d'usage doit appeler
/// <see cref="IVerificateurPermission"/> avec une de ces constantes en première instruction.
///
/// Les actions de consultation publique (lecture des annonces, des appels d'offres et du flux
/// RSS) ne figurent volontairement pas ici : elles sont accessibles aux visiteurs anonymes et
/// ne nécessitent donc aucune vérification de permission.
/// </summary>
public static class Permissions
{
    /// <summary>Constituer / modifier son CV.</summary>
    public const string ConstituerCv = nameof(ConstituerCv);

    /// <summary>Postuler à une annonce d'emploi.</summary>
    public const string PostulerAnnonce = nameof(PostulerAnnonce);

    /// <summary>Soumettre une proposition sur un appel d'offre.</summary>
    public const string SoumettrePropositionFreelance = nameof(SoumettrePropositionFreelance);

    /// <summary>Publier une annonce d'emploi (entreprise : ses propres annonces).</summary>
    public const string PublierAnnonce = nameof(PublierAnnonce);

    /// <summary>Publier un appel d'offre (entreprise : ses propres appels d'offres).</summary>
    public const string PublierAppelOffre = nameof(PublierAppelOffre);

    /// <summary>Consulter les candidatures reçues sur ses propres annonces.</summary>
    public const string ConsulterCandidaturesRecues = nameof(ConsulterCandidaturesRecues);

    /// <summary>Consulter les propositions reçues sur ses propres appels d'offres.</summary>
    public const string ConsulterPropositionsRecues = nameof(ConsulterPropositionsRecues);

    /// <summary>Sélectionner le lauréat d'un appel d'offre.</summary>
    public const string SelectionnerLaureat = nameof(SelectionnerLaureat);

    /// <summary>S'abonner à un domaine métier pour recevoir des notifications.</summary>
    public const string SAbonnerDomaineMetier = nameof(SAbonnerDomaineMetier);

    /// <summary>Publier un article dans le fil d'actualité éditorial.</summary>
    public const string PublierArticleActualite = nameof(PublierArticleActualite);

    /// <summary>Modérer / supprimer une annonce d'emploi ou un appel d'offre.</summary>
    public const string ModererAnnonceOuAppelOffre = nameof(ModererAnnonceOuAppelOffre);

    /// <summary>Bloquer / réactiver un compte utilisateur.</summary>
    public const string BloquerReactiverCompte = nameof(BloquerReactiverCompte);

    /// <summary>Gérer le référentiel des domaines métier (créer/modifier/supprimer).</summary>
    public const string GererReferentielDomaines = nameof(GererReferentielDomaines);
}
