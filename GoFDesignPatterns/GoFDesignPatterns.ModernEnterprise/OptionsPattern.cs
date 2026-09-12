using System.ComponentModel.DataAnnotations;
using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.ModernEnterprise;

public class StorageOptions
{
    public const string SectionName = "Storage";

    [Required(ErrorMessage = "DefaultContainerName is required.")]
    public string DefaultContainerName { get; set; } = "default-blobs";

    [Range(1, 100, ErrorMessage = "MaxConcurrentUploads must be between 1 and 100.")]
    public int MaxConcurrentUploads { get; set; } = 10;

    public bool EnableCompression { get; set; } = true;
    public string AllowedFileExtensions { get; set; } = ".pdf;.docx;.xlsx;.png";
}

/// <summary>
/// Senior ASP.NET Core Idiom: Demonstrates the modern Options Pattern with validation.
/// Explains architectural distinctions between:
/// - IOptions&lt;T&gt;: Registered as Singleton; evaluated once at startup; does not reload.
/// - IOptionsSnapshot&lt;T&gt;: Registered as Scoped; re-evaluates configuration per HTTP request; cannot be injected into Singletons.
/// - IOptionsMonitor&lt;T&gt;: Registered as Singleton; supports real-time configuration reload notifications via OnChange().
/// </summary>
public class StorageServiceWithValidatedOptions(StorageOptions options, List<string> logs)
{
    public Result<string> InitializeStorage()
    {
        logs.Add("[Options Pattern] Validating strongly-typed configuration settings...");

        var validationContext = new ValidationContext(options);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(options, validationContext, validationResults, validateAllProperties: true))
        {
            var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            logs.Add($"[Options Pattern - VALIDATION FAILED] {errors}");
            return Result<string>.Failure("ConfigurationError", errors);
        }

        logs.Add($"[Options Pattern - CONFIGURED] Container: '{options.DefaultContainerName}', " +
                 $"MaxConcurrent: {options.MaxConcurrentUploads}, Compression: {options.EnableCompression}");

        return Result<string>.Success("Storage subsystem successfully initialized with valid options.");
    }
}
