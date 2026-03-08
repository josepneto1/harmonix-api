using Harmonix.Application.Common.Errors;
using Harmonix.Domain.Common;

namespace Harmonix.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Fail(Error error) => new(false, error);
    public static Result Fail(DomainError domainError) => new(false, ErrorMapper.FromDomain(domainError));
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, Error.None);
    public static new Result<T> Fail(Error error) => new(false, default, error);
    public static new Result<T> Fail(DomainError domainError) => new(false, default, ErrorMapper.FromDomain(domainError));
}
