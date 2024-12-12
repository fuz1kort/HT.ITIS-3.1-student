using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class ValidationDecorator<TRequest, TResponse> :
    PermissionCheckDecorator<TRequest, TResponse>,
    IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    protected ValidationDecorator(
        IEnumerable<IValidator<TRequest>> validators,
        IPermissionCheck permissionCheck
    ) : base(permissionCheck)
    {
        _validators = validators;
    }

    public new async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var res = await base.Handle(request, cancellationToken);
        if (res.IsFailure)
        {
            return res;
        }

        if (!_validators.Any())
        {
            return ResultFactory.CreateResult<TResponse>(true);
        }

        var validationResultTasks = _validators
            .Select(x => x.ValidateAsync(request, cancellationToken));
        var validationResult = await Task.WhenAll(validationResultTasks);

        if (!validationResult.Any(x => x.IsValid))
        {
            var failures = validationResult.SelectMany(x => x.Errors);
            return ResultFactory.CreateResult<TResponse>(false,
                error: string.Join(";", failures.Select(x => x.ErrorMessage)));
        }

        return ResultFactory.CreateResult<TResponse>(true);
    }
}