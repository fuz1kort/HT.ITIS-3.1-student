namespace Dotnet.Homeworks.Shared.Dto;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    public Result(bool isSuccessful, string? error = default)
    {
        IsSuccess = isSuccessful;
        if (error is not null)
            Error = error;
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? val, bool isSuccessful, string? error = default)
        : base(isSuccessful, error)
    {
        _value = val;
    }

    public TValue? Value => IsSuccess
        ? _value
        : throw new Exception(Error);
}

public class ResultFactory
{
    public static TResponse CreateResult<TResponse>(bool isSuccess, object? value = default, string? error = default)
    {
        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericArgs = typeof(TResponse).GetGenericArguments();
            var resultType = typeof(Result<>).MakeGenericType(genericArgs);

            var constructor = resultType.GetConstructor(new[] { genericArgs[0], typeof(bool), typeof(string) });
            if (constructor == null)
            {
                throw new InvalidOperationException($"Constructor not found for {resultType}");
            }

            var result = constructor.Invoke(new[] { value, isSuccess, error });
            return (TResponse)result;
        }

        return (TResponse)(object)new Result(isSuccess, error);
    }
}