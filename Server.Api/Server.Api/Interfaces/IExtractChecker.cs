namespace GovServices.Server.Interfaces;

public interface IExtractChecker
{
    Task CheckAllPendingRequestsAsync();
}
