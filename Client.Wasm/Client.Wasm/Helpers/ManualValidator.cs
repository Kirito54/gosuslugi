using System.ComponentModel.DataAnnotations;

namespace Client.Wasm.Helpers;

public static class ManualValidator
{
    public static bool TryValidate(object model, out IEnumerable<string> errors)
    {
        var ctx = new ValidationContext(model);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, ctx, results, true);
        errors = results.Select(r => r.ErrorMessage ?? string.Empty);
        return isValid;
    }
}
