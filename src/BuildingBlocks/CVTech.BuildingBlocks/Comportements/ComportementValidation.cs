using FluentValidation;
using MediatR;

namespace CVTech.BuildingBlocks.Comportements;

/// <summary>
/// Comportement de pipeline MediatR exécuté avant chaque Handler : valide la requête avec
/// tous les <see cref="IValidator{T}"/> enregistrés pour son type et lève
/// <see cref="ValidationException"/> (FluentValidation) si elle est invalide.
/// À enregistrer dans chaque module via <c>cfg.AddOpenBehavior(typeof(ComportementValidation&lt;,&gt;))</c>.
/// </summary>
public sealed class ComportementValidation<TRequete, TReponse> : IPipelineBehavior<TRequete, TReponse>
    where TRequete : IRequest<TReponse>
{
    private readonly IEnumerable<IValidator<TRequete>> _validateurs;

    public ComportementValidation(IEnumerable<IValidator<TRequete>> validateurs)
    {
        _validateurs = validateurs;
    }

    public async Task<TReponse> Handle(
        TRequete request,
        RequestHandlerDelegate<TReponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validateurs.Any())
        {
            return await next();
        }

        var contexte = new ValidationContext<TRequete>(request);

        var echecs = (await Task.WhenAll(_validateurs.Select(v => v.ValidateAsync(contexte, cancellationToken))))
            .SelectMany(resultat => resultat.Errors)
            .Where(echec => echec is not null)
            .ToList();

        if (echecs.Count != 0)
        {
            throw new ValidationException(echecs);
        }

        return await next();
    }
}
