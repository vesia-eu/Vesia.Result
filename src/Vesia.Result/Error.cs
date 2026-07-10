namespace Vesia.Result;

public record Error
{
    private ErrorType Type { get; }
    private string Message { get; }
    
    public static Error NotFound(string message) => new(ErrorType.NotFound, message);
    public static Error Validation(string message) => new(ErrorType.Validation, message);
    public static Error Conflict(string message) => new(ErrorType.Conflict, message);
    public static Error Unavailable(string message) => new(ErrorType.Unavailable, message);
    public static Error Unauthorized(string message) => new(ErrorType.Unauthorized, message);
    public static Error Forbidden(string message) => new(ErrorType.Forbidden, message);
    public static Error Internal(string message) => new(ErrorType.Internal, message);

    public Error(ErrorType type, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Error message cannot be empty.", nameof(message));

        Type = type;
        Message = message;
    }

    public override string ToString() => $"[{Type}] {Message}";
}