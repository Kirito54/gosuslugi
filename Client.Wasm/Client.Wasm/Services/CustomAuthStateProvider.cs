using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.ProtectedBrowserStorage;
using Client.Wasm.Helpers;

namespace Client.Wasm.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _storage;
    private readonly HttpClient _httpClient;
    private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public CustomAuthStateProvider(ProtectedLocalStorage storage, HttpClient httpClient)
    {
        _storage = storage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var result = await _storage.GetAsync<string>("authToken");
        if (!result.Success || string.IsNullOrWhiteSpace(result.Value))
        {
            return _anonymous;
        }

        var token = result.Value!;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task SetTokenAsync(string token)
    {
        await _storage.SetAsync("authToken", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task RemoveTokenAsync()
    {
        await _storage.DeleteAsync("authToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }
}
