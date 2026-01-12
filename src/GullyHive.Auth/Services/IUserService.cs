using GullyHive.Admin.Models;
using GullyHive.Auth.Models;

namespace GullyHive.Auth.Services
{
    public interface IUserService
    {
        Task<long> RegisterUserAsync(RegisterRequest dto);
       // Task RegisterUserAsync(RegistrationModel model);

    }
}
