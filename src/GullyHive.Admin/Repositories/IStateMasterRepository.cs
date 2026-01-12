using Dapper;
using GullyHive.Admin.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace GullyHive.Admin.Repositories
{
    public interface IStateMasterRepository
    {
        Task<IEnumerable<StateDto>> GetStatesAsync();
        Task<StateDto?> GetStateByIdAsync(long id);
        Task<long> InsertStateAsync(StateCreateDto dto);
        Task UpdateStateAsync(long id, StateUpdateDto dto);
        Task DeleteStateAsync(long id);

    }

}
