using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.Notifications;

namespace Client.Wasm.Services;

public class ErrorHandlerService
{
    private Exception? _lastError;
    public event Action? OnError;

    public void Handle(Exception ex)
    {
        _lastError = ex;
        OnError?.Invoke();
    }

    public Exception? GetError() => _lastError;
}
