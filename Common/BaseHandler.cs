using FluentValidation;
using Harmonix.Domain.Common;

namespace Harmonix.Common;

public interface IHandler
{
}

public abstract class BaseHandler<TResponse> : IHandler
{
    public abstract Task<Result<TResponse>> ExecuteAsync(CancellationToken ct);
}

public abstract class BaseHandler<TRequest, TResponse> : IHandler
{
    private readonly IValidator<TRequest>? _validator;

    protected BaseHandler(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }
    public async Task<Result<TResponse>> ExecuteAsync(TRequest request, CancellationToken ct = default)
    {
        if (_validator is not null)
        {
            var validation = await _validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return Result<TResponse>.Fail(validation.ToValidationError());
        }

        return await HandleAsync(request, ct);
    }

    protected abstract Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken ct = default);
}
