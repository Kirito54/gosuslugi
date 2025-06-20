using System.Security.Claims;
using System.Text.Json;

namespace Client.Wasm.Helpers;

public static class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
        {
            return Enumerable.Empty<Claim>();
        }

        var parts = jwt.Split('.');
        if (parts.Length != 3)
        {
            Console.Error.WriteLine($"JWT has invalid format: {jwt}");
            return Enumerable.Empty<Claim>();
        }

        try
        {
            var jsonBytes = ParseBase64WithoutPadding(parts[1]);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            if (keyValuePairs == null)
            {
                return Enumerable.Empty<Claim>();
            }
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to parse JWT: {ex}. Token: {jwt}");
            return Enumerable.Empty<Claim>();
        }
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
        {
            return Array.Empty<byte>();
        }

        base64 = base64.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 0:
                break;
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
            default:
                throw new FormatException("Invalid Base64 string length.");
        }

        try
        {
            return Convert.FromBase64String(base64);
        }
        catch (FormatException ex)
        {
            Console.Error.WriteLine($"Invalid base64 string: {base64}. {ex}");
            return Array.Empty<byte>();
        }
    }
}
