namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IUserProfileApiClient
{
    Task<List<UserProfileDto>> GetAllAsync();
}

public class UserProfileApiClient : IUserProfileApiClient
{
    private readonly HttpClient _http;

    public UserProfileApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UserProfileDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<UserProfileDto>>("api/userprofiles");
        return result ?? new();
    }
}
