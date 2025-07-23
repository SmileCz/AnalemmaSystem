namespace Application.Core.Results;

public record ValidationResult(bool IsValid, string? Error = null)
{
    public static ValidationResult Success() => new(true);
    public static ValidationResult Fail(string error) => new(false, error);
    
    public bool IsInvalid => !IsValid;
    public bool HasError => !string.IsNullOrEmpty(Error);
    public string ErrorOrDefault => Error ?? "Unknown validation error";
}
