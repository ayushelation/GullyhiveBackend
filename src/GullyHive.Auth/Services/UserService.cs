using GullyHive.Admin.Models;
using GullyHive.Auth.Models;
using GullyHive.Auth.Repositories;

namespace GullyHive.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _env;

        public UserService(IUserRepository userRepository, IWebHostEnvironment env)
        {
            _userRepository = userRepository;
            _env = env;
        }
        public Task<long> RegisterUserAsync(RegisterRequest dto) => _userRepository.AddUserAsync(dto);

        ////public async Task RegisterUserAsync(RegistrationModel model)
        ////{
        ////    // Save files
        ////    string registrationDocPath = null;
        ////    string addressProofPath = null;

        ////    if (model.RegistrationDocument != null)
        ////    {
        ////        registrationDocPath = Path.Combine(_env.WebRootPath, "Uploads", model.RegistrationDocument.FileName);
        ////        using var stream = new FileStream(registrationDocPath, FileMode.Create);
        ////        await model.RegistrationDocument.CopyToAsync(stream);
        ////    }

        ////    if (model.AddressProof != null)
        ////    {
        ////        addressProofPath = Path.Combine(_env.WebRootPath, "Uploads", model.AddressProof.FileName);
        ////        using var stream = new FileStream(addressProofPath, FileMode.Create);
        ////        await model.AddressProof.CopyToAsync(stream);
        ////    }

        ////    // Map to User
        ////    var user = new User
        ////    {
        ////        FullName = model.FullName,
        ////        Email = model.Email,
        ////        Mobile = model.Mobile,
        ////        CoverageArea = model.CoverageArea,
        ////        ProfessionalType = model.ProfessionalType,
        ////        ServiceCategory = model.ServiceCategory != null ? string.Join(",", model.ServiceCategory) : "",
        ////        BusinessName = model.BusinessName,
        ////        RegistrationType = model.RegistrationType,
        ////        RegistrationNumber = model.RegistrationNumber,
        ////        SelfOverview = model.SelfOverview,
        ////        SkillsBackground = model.SkillsBackground,
        ////        Achievements = model.Achievements,
        ////        RegistrationDocumentPath = registrationDocPath,
        ////        AddressProofPath = addressProofPath
        ////    };

        ////    // Call repository
        ////    await _userRepository.AddUserAsync(user);
        ////}
    }

}
