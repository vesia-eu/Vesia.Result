namespace Vesia.Result;

public record Error(ErrorType ErrorType, string Message)
{
    public override string ToString() => $"[{ErrorType}] {Message}";
}