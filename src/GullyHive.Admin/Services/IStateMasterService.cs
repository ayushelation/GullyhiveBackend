using GullyHive.Admin.Models;

namespace GullyHive.Admin.Services
{
    public interface IStateMasterService
    {  
            Task<IEnumerable<StateDto>> GetStatesAsync();
            Task<StateDto> GetStateByIdAsync(long id);
            Task<long> CreateStateAsync(StateCreateDto dto);
            Task UpdateStateAsync(long id, StateUpdateDto dto);
            Task DeleteStateAsync(long id);

    }
}
