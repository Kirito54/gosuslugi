using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using GovServices.Server.Data;
using GovServices.Server.DTOs;

namespace GovServices.Server.Services.Integrations.Base;

public abstract class IntegrationServiceBase<TCreateDto, TResponseDto>
{
    private readonly IHttpClientFactory _httpClientFactory;
    protected readonly ApplicationDbContext Context;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    private string? _token;

    protected IntegrationServiceBase(
        IHttpClientFactory httpClientFactory,
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger logger)
    {
        _httpClientFactory = httpClientFactory;
        Context = context;
        _configuration = configuration;
        _logger = logger;
    }

    protected abstract string ServiceName { get; }
    protected abstract string ApiUrl { get; }
    protected abstract string Login { get; }
    protected abstract string Password { get; }

    public abstract Task<TResponseDto> SendRequestAsync(TCreateDto dto);
    public abstract Task<TResponseDto> GetStatusAsync(string requestId);

    private async Task<string> GetTokenAsync(bool force = false)
    {
        if (!force && !string.IsNullOrWhiteSpace(_token))
            return _token!;

        var client = _httpClientFactory.CreateClient();
        var body = new { Login, Password };
        _logger.LogInformation("[{Service}] Requesting token", ServiceName);
        var response = await client.PostAsJsonAsync($"{ApiUrl}/Token", body);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("[{Service}] Token request failed: {Status}", ServiceName, response.StatusCode);
            response.EnsureSuccessStatusCode();
        }

        var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>() ??
                       throw new InvalidOperationException("Invalid token response");
        _token = tokenDto.Token;
        return _token!;
    }

    protected async Task<HttpClient> CreateAuthorizedClientAsync(bool refresh = false)
    {
        var token = await GetTokenAsync(refresh);
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    protected async Task<TResponse> SendWithAuthAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest body)
    {
        var client = await CreateAuthorizedClientAsync();
        HttpResponseMessage response;
        var request = new HttpRequestMessage(method, url);
        if (body != null)
            request.Content = JsonContent.Create(body);
        _logger.LogInformation("[{Service}] {Method} {Url}", ServiceName, method, url);
        response = await client.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            client = await CreateAuthorizedClientAsync(true);
            request = new HttpRequestMessage(method, url);
            if (body != null)
                request.Content = JsonContent.Create(body);
            _logger.LogInformation("[{Service}] Retrying after token refresh", ServiceName);
            response = await client.SendAsync(request);
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("[{Service}] Error response {Status}", ServiceName, response.StatusCode);
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>() ?? throw new InvalidOperationException("Invalid response");
    }

    private record TokenDto(string Token);
}
