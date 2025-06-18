namespace Client.Wasm.Services;

public class ErrorHandlingMessageHandler : DelegatingHandler
{
    private readonly ErrorHandlerService _errorHandler;

    public ErrorHandlingMessageHandler(ErrorHandlerService errorHandler)
    {
        _errorHandler = errorHandler;
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _errorHandler.Handle(new HttpRequestException($"{response.StatusCode}: {content}"));
            }
            return response;
        }
        catch (Exception ex)
        {
            _errorHandler.Handle(ex);
            throw;
        }
    }
}
