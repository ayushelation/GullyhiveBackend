using GullyHive.Admin.Models;
using GullyHive.Admin.Repositories;

namespace GullyHive.Admin.Services
{
    public class StateMasterService : IStateMasterService
    {
        private readonly IStateMasterRepository _repo;

        public StateMasterService(IStateMasterRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<StateDto>> GetStatesAsync() => _repo.GetStatesAsync();

        public async Task<StateDto> GetStateByIdAsync(long id)
        {
            var state = await _repo.GetStateByIdAsync(id);
            if (state == null)
                throw new KeyNotFoundException("State not found");

            return state;
        }

        public Task<long> CreateStateAsync(StateCreateDto dto) => _repo.InsertStateAsync(dto);

        public Task UpdateStateAsync(long id, StateUpdateDto dto) => _repo.UpdateStateAsync(id, dto);

        public Task DeleteStateAsync(long id) => _repo.DeleteStateAsync(id);
    }
 }
