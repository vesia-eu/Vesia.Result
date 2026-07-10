namespace Vesia.Result;

public static class ResultAsyncExtensions
{
    public static async Task<Result<TOut>> BindAsync<TInput, TOut>(
        this Task<Result<TInput>> resultTask,
        Func<TInput, Task<Result<TOut>>> transform)
    {
        var result = await resultTask;
        return result.IsSuccess ? await transform(result.Value) : Result<TOut>.Failure(result.Error!);
    }

    public static async Task<Result<TOut>> BindAsync<TInput, TOut>(
        this Result<TInput> result,
        Func<TInput, Task<Result<TOut>>> transform)
    {
        return result.IsSuccess ? await transform(result.Value) : Result<TOut>.Failure(result.Error!);
    }

    public static async Task<Result<TOut>> MapAsync<TInput, TOut>(
        this Task<Result<TInput>> resultTask,
        Func<TInput, Task<TOut>> transform)
    {
        var result = await resultTask;
        if (!result.IsSuccess) return Result<TOut>.Failure(result.Error!);
        return Result<TOut>.Success(await transform(result.Value));
    }

    public static async Task<Result<TOut>> MapAsync<TInput, TOut>(
        this Result<TInput> result,
        Func<TInput, Task<TOut>> transform)
    {
        if (!result.IsSuccess) return Result<TOut>.Failure(result.Error!);
        return Result<TOut>.Success(await transform(result.Value));
    }
}