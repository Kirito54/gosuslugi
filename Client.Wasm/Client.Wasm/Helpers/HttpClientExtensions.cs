using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Wasm.Helpers;

public static class HttpClientExtensions
{
    public static async Task<T?> GetFromJsonSafeAsync<T>(this HttpClient http, string uri, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await http.GetAsync(uri, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                Console.Error.WriteLine($"GET {uri} failed: {response.StatusCode}");
                return default;
            }

            if (response.Content.Headers.ContentLength == 0)
                return default;

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            if (stream == null || stream.Length == 0)
                return default;

            return await JsonSerializer.DeserializeAsync<T>(stream, options ?? new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"GET {uri} threw HttpRequestException: {ex}");
            return default;
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"GET {uri} returned invalid JSON: {ex}");
            return default;
        }
    }
}
