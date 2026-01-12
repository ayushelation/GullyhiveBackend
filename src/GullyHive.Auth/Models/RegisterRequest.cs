using System.ComponentModel.DataAnnotations;

namespace GullyHive.Auth.Models
{
    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Mobile { get; set; }

     //   public string CoverageArea { get; set; }
    //    public string ProfessionalType { get; set; }
       // public string ServiceCategory { get; set; }
        public string[] ServiceCategory { get; set; }   // 🔥 FIXED
        public string BusinessName { get; set; }
        public string RegistrationType { get; set; }
        public string RegistrationNumber { get; set; }

        public string BusinessAddress { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string State { get; set; }

        public string SelfOverview { get; set; }
        public string SkillsBackground { get; set; }
        public string Achievements { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        // ✅ FILES
        public IFormFile ProfilePicture { get; set; }
        public IFormFile RegistrationDocument { get; set; }
        public IFormFile AddressProof { get; set; }
        [Required(ErrorMessage = "CoverageArea is required")]
        public string CoverageArea { get; set; }

        [Required(ErrorMessage = "ProfessionalType is required")]
        public string ProfessionalType { get; set; }

    }
}
