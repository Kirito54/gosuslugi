using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface ISubjectService
{
    Task<List<IndividualDto>> GetIndividualsAsync();
    Task<List<LegalEntityDto>> GetLegalEntitiesAsync();
}
