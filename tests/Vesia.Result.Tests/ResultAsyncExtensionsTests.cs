namespace Vesia.Result.Tests;

public class ResultAsyncExtensionsTests
{
    // ---------- BindAsync (Task<Result<T>> source) ----------

    [Fact]
    public async Task BindAsync_TaskSource_Success_InvokesTransformAndReturnsResult()
    {
        var resultTask = Task.FromResult(Result<string>.Success("Correct!"));

        var final = await resultTask.BindAsync(value => Task.FromResult(Result<int>.Success(value.Length)));

        Assert.True(final.IsSuccess);
        Assert.Equal(8, final.Value);
    }

    [Fact]
    public async Task BindAsync_TaskSource_Failure_SkipsTransformAndPropagatesError()
    {
        var error = new Error(ErrorType.NotFound,"User not found");
        
        var resultTask = Task.FromResult(Result<string>.Failure(error));
        var transformCalled = false;

        var final = await resultTask.BindAsync(value =>
        {
            transformCalled = true;
            return Task.FromResult(Result<int>.Success(value.Length));
        });

        Assert.False(transformCalled);
        Assert.True(final.IsFailure);
        Assert.Equal(error, final.Error);
    }

    [Fact]
    public async Task BindAsync_TaskSource_InnerTransformFails_PropagatesInnerError()
    {
        var resultTask = Task.FromResult(Result<string>.Success("Correct!"));
        var innerError = new Error(ErrorType.Validation,("too long"));

        var final = await resultTask.BindAsync(_ => Task.FromResult(Result<int>.Failure(innerError)));

        Assert.True(final.IsFailure);
        Assert.Equal(innerError, final.Error);
    }

    // ---------- BindAsync (Result<T> source) ----------

    [Fact]
    public async Task BindAsync_ResultSource_Success_InvokesTransformAndReturnsResult()
    {
        var result = Result<string>.Success("Correct!");

        var final = await result.BindAsync(value => Task.FromResult(Result<int>.Success(value.Length)));

        Assert.True(final.IsSuccess);
        Assert.Equal(8, final.Value);
    }

    [Fact]
    public async Task BindAsync_ResultSource_Failure_SkipsTransformAndPropagatesError()
    {
        var error = new Error(ErrorType.NotFound,"User not found");
        var result = Result<string>.Failure(error);
        var transformCalled = false;

        var final = await result.BindAsync(value =>
        {
            transformCalled = true;
            return Task.FromResult(Result<int>.Success(value.Length));
        });

        Assert.False(transformCalled);
        Assert.True(final.IsFailure);
        Assert.Equal(error, final.Error);
    }

    // ---------- MapAsync (Task<Result<T>> source) ----------

    [Fact]
    public async Task MapAsync_TaskSource_Success_TransformsValue()
    {
        var resultTask = Task.FromResult(Result<string>.Success("Correct!"));

        var final = await resultTask.MapAsync(value => Task.FromResult(value.Length));

        Assert.True(final.IsSuccess);
        Assert.Equal(8, final.Value);
    }

    [Fact]
    public async Task MapAsync_TaskSource_Failure_SkipsTransformAndPropagatesError()
    {
        var error = new Error(ErrorType.NotFound,"User not found");
        var resultTask = Task.FromResult(Result<string>.Failure(error));
        var transformCalled = false;

        var final = await resultTask.MapAsync(value =>
        {
            transformCalled = true;
            return Task.FromResult(value.Length);
        });

        Assert.False(transformCalled);
        Assert.True(final.IsFailure);
        Assert.Equal(error, final.Error);
    }

    // ---------- MapAsync (Result<T> source) ----------

    [Fact]
    public async Task MapAsync_ResultSource_Success_TransformsValue()
    {
        var result = Result<string>.Success("Correct!");

        var final = await result.MapAsync(value => Task.FromResult(value.Length));

        Assert.True(final.IsSuccess);
        Assert.Equal(8, final.Value);
    }

    [Fact]
    public async Task MapAsync_ResultSource_Failure_SkipsTransformAndPropagatesError()
    {
        var error = new Error(ErrorType.NotFound,"User not found");
        var result = Result<string>.Failure(error);
        var transformCalled = false;

        var final = await result.MapAsync(value =>
        {
            transformCalled = true;
            return Task.FromResult(value.Length);
        });

        Assert.False(transformCalled);
        Assert.True(final.IsFailure);
        Assert.Equal(error, final.Error);
    }

    // ---------- Chaining ----------

    [Fact]
    public async Task BindAsync_ChainedAcrossMultipleSteps_ShortCircuitsOnFirstFailure()
    {
        var secondStepCalled = false;

        var final = await GetInitialAsync()
            .BindAsync(FailingStepAsync)
            .BindAsync(value =>
            {
                secondStepCalled = true;
                return Task.FromResult(Result<int>.Success(value.Length));
            });

        Assert.False(secondStepCalled);
        Assert.True(final.IsFailure);

        return;

        static Task<Result<string>> GetInitialAsync() => Task.FromResult(Result<string>.Success("Correct!"));
        static Task<Result<string>> FailingStepAsync(string _) => Task.FromResult(Result<string>.Failure(ErrorType.Validation, "bad input"));
    }

    [Fact]
    public async Task BindAsync_ChainedAcrossMultipleSteps_AllSuccessPropagatesFinalValue()
    {
        var final = await GetInitialAsync()
            .BindAsync(value => Task.FromResult(Result<int>.Success(value.Length)))
            .BindAsync(length => Task.FromResult(Result<string>.Success($"Length: {length}")));

        Assert.True(final.IsSuccess);
        Assert.Equal("Length: 8", final.Value);

        return;

        static Task<Result<string>> GetInitialAsync() => Task.FromResult(Result<string>.Success("Correct!"));
    }
}