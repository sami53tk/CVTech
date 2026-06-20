using CVTech.BuildingBlocks.Exceptions;

namespace CVTech.CatalogueEmploi.Domaine.Exceptions;

/// <summary>
/// Levée lorsqu'un candidat tente de postuler à une annonce sans avoir d'abord constitué son CV.
/// </summary>
public sealed class CvObligatoirePourPostulerException : ExceptionMetier
{
    public CvObligatoirePourPostulerException()
        : base("Vous devez constituer votre CV avant de pouvoir postuler à une annonce.")
    {
    }
}

/// <summary>
/// Levée lorsqu'un candidat tente de postuler une seconde fois à la même annonce.
/// </summary>
public sealed class CandidatureDejaSoumiseException : ExceptionMetier
{
    public CandidatureDejaSoumiseException()
        : base("Vous avez déjà postulé à cette annonce.")
    {
    }
}
