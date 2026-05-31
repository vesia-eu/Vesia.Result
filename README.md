# Vesia.Result

A lightweight, dependency-free Result type for .NET. Model success and failure explicitly — no exceptions, no nulls, no surprises.

## Installation

```bash
dotnet add package Vesia.Result
```

## Basic Usage

```csharp
// Non-generic — for operations with no return value
Result result = Result.Success();
Result result = Result.Failure(new Error(ErrorType.NotFound, "User not found"));
 
// Generic — for operations that return a value
Result<User> result = Result<User>.Success(user);
Result<User> result = Result<User>.Failure(new Error(ErrorType.NotFound, "User not found"));
```

## Error Types

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

## License

MIT © [Vesia](https://Vesia.eu)
 
