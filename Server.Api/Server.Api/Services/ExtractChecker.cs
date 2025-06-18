using GovServices.Server.Entities;
using GovServices.Server.Interfaces;

namespace GovServices.Server.Services;

public class ExtractChecker : IExtractChecker
{
    private readonly IRegistryApi _zagcApi;
    private readonly IRegistryApi _rosreestrApi;
    private readonly IExtractRepository _repository;

    public ExtractChecker(IRegistryApi zagcApi, IRegistryApi rosreestrApi, IExtractRepository repository)
    {
        _zagcApi = zagcApi;
        _rosreestrApi = rosreestrApi;
        _repository = repository;
    }

    public async Task CheckAllPendingRequestsAsync()
    {
        var pendingRequests = await _repository.GetPendingRequestsAsync();

        foreach (var request in pendingRequests)
        {
            string? response = request.RegistrySource switch
            {
                RegistrySource.Zags => await _zagcApi.CheckStatusAsync(request.ExternalId),
                RegistrySource.Rosreestr => await _rosreestrApi.CheckStatusAsync(request.ExternalId),
                _ => null
            };

            if (!string.IsNullOrEmpty(response))
            {
                var result = await _repository.ProcessExtractAsync(request, response);
                if (result)
                    await _repository.MarkAsCompletedAsync(request.Id);
            }
        }
    }
}
