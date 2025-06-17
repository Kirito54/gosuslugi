using System.Net.Http.Json;
using Client.Wasm.DTOs;

namespace Client.Wasm.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginRequestDto dto);
    Task LogoutAsync();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly CustomAuthStateProvider _authProvider;

    public AuthService(HttpClient http, CustomAuthStateProvider authProvider)
    {
        _http = http;
        _authProvider = authProvider;
    }

    public async Task<bool> LoginAsync(LoginRequestDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/auth/login", dto);

        if (!res.IsSuccessStatusCode)
        {
            return false;
        }

        if (res.Content == null || res.Content.Headers.ContentLength == 0)
        {
            return false;
        }

        var result = await res.Content.ReadFromJsonAsync<AuthResultDto>();
        if (result == null)
        {
            return false;
        }

        await _authProvider.SetTokenAsync(result.Token);
        return true;
    }

    public async Task LogoutAsync()
    {
        await _authProvider.RemoveTokenAsync();
    }
}
