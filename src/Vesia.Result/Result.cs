namespace Vesia.Result;

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    private Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);

    public TOut Match<TOut>(
        Func<TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        return IsSuccess
            ? onSuccess()
            : onFailure(Error!);
    }
}

public class Result<T>
{
    public T Value { get; }
    public bool IsSuccess { get; }
    public Error? Error { get; }

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
        Error = null;
    }

    private Result(Error error)
    {
        Value = default!;
        IsSuccess = false;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);

    public TOut Match<TOut>(
        Func<T, TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        return IsSuccess
            ? onSuccess(Value)
            : onFailure(Error!);
    }

    public Result<TOut> Map<TOut>(Func<T, TOut> transform)
    {
        if (!IsSuccess)
            return Result<TOut>.Failure(Error!);
    
        return Result<TOut>.Success(transform(Value));
    }

    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> transform)
    {
        if (!IsSuccess)
            return Result<TOut>.Failure(Error!);

        return transform(Value);
    }
}