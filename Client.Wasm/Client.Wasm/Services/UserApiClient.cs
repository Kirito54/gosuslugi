namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;

public interface IUserApiClient
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task UpdateAsync(string id, UpdateUserDto dto);
    Task DeleteAsync(string id);
    Task ChangePasswordAsync(string id, ChangePasswordDto dto);
}

public class UserApiClient : IUserApiClient
{
    private readonly HttpClient _http;

    public UserApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<UserDto>>("api/users") ?? new();
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        return await _http.GetFromJsonAsync<UserDto>($"api/users/{id}");
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/users", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<UserDto>())!;
    }

    public async Task UpdateAsync(string id, UpdateUserDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/users/{id}", dto);
        res.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string id)
    {
        var res = await _http.DeleteAsync($"api/users/{id}");
        res.EnsureSuccessStatusCode();
    }

    public async Task ChangePasswordAsync(string id, ChangePasswordDto dto)
    {
        var res = await _http.PostAsJsonAsync($"api/users/{id}/changePassword", dto);
        res.EnsureSuccessStatusCode();
    }
}
