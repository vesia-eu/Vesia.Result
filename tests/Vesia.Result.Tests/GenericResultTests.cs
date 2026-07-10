namespace Vesia.Result.Tests;

public class GenericResultTests
{
    // Test 1 - IsSuccess is true
    [Fact]
    public void Success_ShouldHave_IsSuccessTrue()
    {
        var result = Result<string>.Success("Correct!");
        Assert.True(result.IsSuccess);
    }

    // Test 2 - Value is correct
    [Fact]
    public void Success_ShouldHave_CorrectValue()
    {
        var result = Result<string>.Success("Correct!");
        Assert.Equal("Correct!", result.Value);
    }
    
    // Test 3 - IsSuccess is false
    [Fact]
    public void ShouldHave_IsSuccessFalse()
    {
        var result = Result<string>.Failure(new Error(ErrorType.Unavailable ,"Error"));
        Assert.False(result.IsSuccess);
    }

    // Test 4 - Error is null
    [Fact]
    public void Success_ShouldHave_NullError()
    {
        var result = Result<string>.Success("Correct!");
        Assert.Null(result.Error);
    }
    
    // Test 5 - Should have error
    [Fact]
    public void ShouldHave_Error()
    {
        var error = new Error(ErrorType.Unavailable, "Error");
        var result = Result<string>.Failure(error);
        Assert.Equal(error, result.Error);
    }
    
    // Test 6 - Successful Match Test
    [Fact]
    public void Success_Match_Test()
    {
        var result = Result<string>.Success("Correct!");

        var matchResult = result.Match(
            onSuccess: value => "CorrectMatch!",
            onFailure: error => "FailedMatch!"
        );
        
        Assert.Equal("CorrectMatch!", matchResult);
    }
    
    // Test 7 - Failed Match Test
    [Fact]
    public void Failed_Match_Test()
    {
        var result = Result<string>.Failure(new Error(ErrorType.Unavailable ,"Error"));

        var matchResult = result.Match(
            onSuccess: value => "CorrectMatch!",
            onFailure: error => "FailedMatch!"
        );
        
        Assert.Equal("FailedMatch!", matchResult);
    }
    
    // Test 8 - Successful Map Test
    [Fact]
    public void Success_Map_Test()
    {
        var resultString = Result<string>.Success("Correct!");

        var resultInt = resultString.Map(res => 1);
        
        Assert.Equal(1, resultInt.Value);
    }
    
    // Test 9 - Short Circuit Map Test
    [Fact]
    public void Short_Circuit_Map_Test()
    {
        var error = new Error(ErrorType.Unavailable ,"Error");
        
        var result = Result<string>.Failure(error);
        var mapped = result.Map(res => 1);
        
        Assert.False(mapped.IsSuccess);
        Assert.True(mapped.IsFailure);
        Assert.Equal(error, mapped.Error);
    }
    
    // Test 10 - Successful Bind Test
    [Fact]
    public void Success_Bind_Test()
    {
        var resultString = Result<string>.Success("Correct!");
        
        var bindTest = resultString.Bind(res =>  Result<int>.Success(0));
        
        Assert.Equal(0, bindTest.Value);
        Assert.True(bindTest.IsSuccess);
        Assert.False(bindTest.IsFailure);
    }
    
    // Test 11 - Failed Bind Test
    [Fact]
    public void Failed_Bind_Test()
    {
        var error = new Error(ErrorType.Unavailable ,"Error");
        var resultString = Result<string>.Success("Correct!");
        
        var bindTest = resultString.Bind(res =>  Result<int>.Failure(error));
        
        Assert.Equal(error, bindTest.Error);
        Assert.False(bindTest.IsSuccess);
        Assert.True(bindTest.IsFailure);
    }
    
    // Test 12 - Short_Circuit Bind test
    [Fact]
    public void Short_Circuit_Bind_Test()
    {
        var error = new Error(ErrorType.Unavailable ,"Error");
        
        var resultString = Result<string>.Failure(error);
        var bindTest = resultString.Bind(res =>  Result<int>.Success(0));
        
        Assert.Equal(error, bindTest.Error);
        Assert.False(bindTest.IsSuccess);
        Assert.True(bindTest.IsFailure);
    }
    
    // Test 13 - Failed - default Value
    [Fact]
    public void Failure_Value_ShouldBeDefault()
    {
        var result = Result<string>.Failure(new Error(ErrorType.Unavailable ,"Error"));
        
        Assert.Null(result.Value);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
    }
    
    // Test 14 - Failed - Error Factory
    [Fact]
    public void Error_Factory()
    {
        var result = Result<string>.Failure(ErrorType.Unavailable ,"Error");
        
        Assert.Null(result.Value);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
    }
    
    // Test 15 - Success - Tap executes action and passes through unchanged
    [Fact]
    public void Tap_Test()
    {
        var tapValue = "";
        var resultString = Result<string>.Success("Correct!");
    
        resultString.Tap(p => tapValue = p);
    
        Assert.Equal("Correct!", tapValue);
        Assert.True(resultString.IsSuccess);
        Assert.False(resultString.IsFailure);
    }
    
    [Fact]
    public void Tap_OnSuccess_ExecutesActionAndReturnsOriginalResult()
    {
        var tapValue = "";
        var result = Result<string>.Success("Correct!");
    
        var returned = result.Tap(v => tapValue = v);
    
        Assert.Equal("Correct!", tapValue);
        Assert.Same(result, returned);
    }

    [Fact]
    public void Tap_OnFailure_DoesNotExecuteAction()
    {
        var tapValue = "";
        var result = Result<string>.Failure(ErrorType.NotFound, "Not found");
    
        result.Tap(v => tapValue = v);
    
        Assert.Empty(tapValue);
    }
}
