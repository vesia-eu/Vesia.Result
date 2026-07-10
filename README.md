# Vesia.Result

A lightweight, dependency-free Result type for .NET. Model success and failure explicitly — no exceptions, no nulls, no surprises.

## Installation

```bash
dotnet add package Vesia.Result
```

## Basic Usage

```csharp
// Non-generic — for operations with no return value
Result success = Result.Success();
Result failure = Result.Failure(Error.NotFound("User not found"));

// Generic — for operations that return a value
Result<User> success = Result<User>.Success(user);
Result<User> failure = Result<User>.Failure(Error.NotFound("User not found"));
```

## Errors

`Error` is a record with a `Type` and a `Message`. Use the static factory methods instead of constructing it directly:

```csharp
Error.NotFound("User not found");
Error.Validation("Name is required");
Error.Conflict("Email already in use");
Error.Unauthorized("Sign in required");
Error.Forbidden("You don't have access to this resource");
Error.Internal("Something went wrong");
Error.Unavailable("Service is temporarily unavailable");
```

### Error Types

```csharp
public enum ErrorType
{
    NotFound,
    Validation,
    Conflict,
    Unauthorized,
    Forbidden,
    Internal,
    Unavailable
}
```

## IsSuccess / IsFailure

```csharp
if (result.IsFailure)
{
    logger.LogWarning("{Error}", result.Error);
}
```

## Match

Branch on success or failure, always produces a value:

```csharp
var message = result.Match(
    onSuccess: user => $"Welcome, {user.Name}",
    onFailure: error => $"Failed: {error.Message}"
);
```

## Map

Transform the value if successful, pass failure through untouched:

```csharp
Result<UserDto> dto = result.Map(user => new UserDto(user));
```

## Bind

Chain into another operation that can itself fail:

```csharp
Result<UserDto> dto = result.Bind(user => GetUserProfile(user.Id));
```

## Tap

Run a side effect (logging, events) on success without breaking the chain. Returns the original result unchanged:

```csharp
result
    .Tap(user => logger.LogInformation("Loaded {Id}", user.Id))
    .Map(user => new UserDto(user));
```

## Async

`BindAsync` and `MapAsync` let you chain async operations without manually awaiting between every step. They work whether the previous step was a plain `Result<T>` or a `Task<Result<T>>`:

```csharp
Result<UserDto> dto = await GetUserAsync(id)
    .BindAsync(user => ValidateAsync(user))
    .MapAsync(user => Task.FromResult(new UserDto(user)));
```

## License

MIT © [Vesia](https://Vesia.eu)