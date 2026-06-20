using CVTech.BuildingBlocks.Exceptions;

namespace CVTech.AppelOffreFreelance.Domaine.Exceptions;

public sealed class PropositionDejaSubmiseException : ExceptionMetier
{
    public PropositionDejaSubmiseException()
        : base("Vous avez déjà soumis une proposition pour cet appel d'offre.") { }
}

public sealed class LaureatDejaSelectionneException : ExceptionMetier
{
    public LaureatDejaSelectionneException()
        : base("Un lauréat a déjà été sélectionné pour cet appel d'offre.") { }
}

public sealed class PropositionAppartientPasAuFreelanceException : ExceptionMetier
{
    public PropositionAppartientPasAuFreelanceException()
        : base("Cette proposition n'appartient pas à ce freelance.") { }
}
