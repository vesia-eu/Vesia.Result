namespace Vesia.Result;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    private Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
    
    public static Result Failure(ErrorType type, string message) 
        => Failure(new Error(type, message));

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
    public bool IsFailure => !IsSuccess;
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
    
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);
    
    public static Result<T> Failure(ErrorType type, string message) 
        => Failure(new Error(type, message));

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
    
    public Result<T> Tap(Action<T> action)
    {
        if (IsSuccess) action(Value);
        return this;
    }

    public Result<T> TapError(Action<Error> action)
    {
        if (!IsSuccess) action(Error!);
        return this;
    }
}