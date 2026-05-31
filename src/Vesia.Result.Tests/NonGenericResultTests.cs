namespace Vesia.Result.Tests;

public class NonGenericResultTests
{
    // Test 1 - IsSuccess is true
    [Fact]
    public void Success_ShouldHave_IsSuccessTrue()
    {
        var result = Result.Success();
        Assert.True(result.IsSuccess);
    }
    
    // Test 2 - Error is null
    [Fact]
    public void Success_ShouldHave_NullError()
    {
        var result = Result.Success();
        Assert.Null(result.Error);
    }
    
    // Test 3 - IsSuccess is false
    [Fact]
    public void ShouldHave_IsSuccessFalse()
    {
        var result = Result.Failure(new Error(ErrorType.Unavailable ,"Error"));
        Assert.False(result.IsSuccess);
    }
    
    // Test 4 - Should have correct error
    [Fact]
    public void ShouldHave_Error()
    {
        var error = new Error(ErrorType.Unavailable, "Error");
        var result = Result.Failure(error);
        Assert.Equal(error, result.Error);
    }
    
    // Test 5 - Successful Match Test
    [Fact]
    public void Success_Match_Test()
    {
        var result = Result.Success();

        var matchResult = result.Match(
            onSuccess: () => "CorrectMatch!",
            onFailure: error => "FailedMatch!"
        );
        
        Assert.Equal("CorrectMatch!", matchResult);
    }
    
    // Test 6 - Failed Match Test
    [Fact]
    public void Failed_Match_Test()
    {
        var result = Result.Failure(new Error(ErrorType.Unavailable ,"Error"));

        var matchResult = result.Match(
            onSuccess: () => "CorrectMatch!",
            onFailure: error => "FailedMatch!"
        );
        
        Assert.Equal("FailedMatch!", matchResult);
    }
}