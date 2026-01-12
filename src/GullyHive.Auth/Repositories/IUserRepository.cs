using GullyHive.Admin.Models;
using GullyHive.Auth.Models;

namespace GullyHive.Auth.Repositories
{
    public interface IUserRepository
    {
        Task<long> AddUserAsync(RegisterRequest dto);
      //  Task AddUserAsync(User user);
    }
}
