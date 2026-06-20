using CVTech.BuildingBlocks.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CVTech.Api.Middlewares;

/// <summary>
/// Traduit les exceptions métier (<see cref="ExceptionMetier"/>) et de validation
/// (<see cref="ValidationException"/>) en réponses HTTP <see cref="ProblemDetails"/> cohérentes,
/// pour tous les modules.
/// </summary>
public sealed class GestionnaireExceptionsGlobal : IExceptionHandler
{
    private readonly ILogger<GestionnaireExceptionsGlobal> _logger;

    public GestionnaireExceptionsGlobal(ILogger<GestionnaireExceptionsGlobal> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception non gérée");

        var (statut, titre) = exception switch
        {
            PermissionRefuseeException or CompteBloqueException => (StatusCodes.Status403Forbidden, "Accès refusé"),
            EntiteIntrouvableException => (StatusCodes.Status404NotFound, "Ressource introuvable"),
            ValidationException => (StatusCodes.Status400BadRequest, "Données invalides"),
            ExceptionMetier => (StatusCodes.Status400BadRequest, "Règle métier violée"),
            _ => (StatusCodes.Status500InternalServerError, "Erreur interne"),
        };

        var probleme = new ProblemDetails
        {
            Status = statut,
            Title = titre,
            Detail = exception is ExceptionMetier or ValidationException
                ? exception.Message
                : "Une erreur inattendue est survenue.",
        };

        if (exception is ValidationException validationException)
        {
            probleme.Extensions["erreurs"] = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        httpContext.Response.StatusCode = statut;
        await httpContext.Response.WriteAsJsonAsync(probleme, cancellationToken);

        return true;
    }
}
