namespace Application.Core.Results;

public record Result<T>(bool IsSuccess, T? Value = default, string? Error = null)
{
    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Failure(string? error) => new(false, default, error);
    
    public bool IsFailure => !IsSuccess;
    public bool HasError => !string.IsNullOrEmpty(Error);
    public string ErrorOrDefault => Error ?? "Unknown error";
}