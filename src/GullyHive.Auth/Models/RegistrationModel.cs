using System.ComponentModel.DataAnnotations;

namespace GullyHive.Auth.Models
{
    //public class RegistrationModel
    //{
    //    // Step 1: Basic Info
    //    [Required]
    //    public string FullName { get; set; }

    //    [Required]
    //    [EmailAddress]
    //    public string Email { get; set; }

    //    [Required]
    //    [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter valid 10-digit mobile")]
    //    public string Mobile { get; set; }

    //    [Required]
    //    public string CoverageArea { get; set; }

    //    [Required]
    //    public string ProfessionalType { get; set; }

    //    public string[] ServiceCategory { get; set; }

    //    // Step 2: Business Info
    //    public string BusinessName { get; set; }
    //    public string RegistrationType { get; set; }
    //    public string RegistrationNumber { get; set; }

    //    public IFormFile RegistrationDocument { get; set; }
    //    public IFormFile AddressProof { get; set; }

    //    public string BusinessAddress { get; set; }
    //    public string City { get; set; }
    //    public string PinCode { get; set; }

    //    // Step 3: Professional Details
    //    public string SelfOverview { get; set; }
    //    public string SkillsBackground { get; set; }
    //    public string Achievements { get; set; }

    //    // Optional: profile picture
    //    public IFormFile ProfilePicture { get; set; }
    //}

    public class RegistrationModel
    {
        [Required] public string FullName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, RegularExpression(@"^\d{10}$")] public string Mobile { get; set; }

        [Required] public string CoverageArea { get; set; }
        [Required] public string ProfessionalType { get; set; }

        public string[] ServiceCategory { get; set; }

        public string BusinessName { get; set; }
        public string RegistrationType { get; set; }
        public string RegistrationNumber { get; set; }

        public IFormFile RegistrationDocument { get; set; }
        public IFormFile AddressProof { get; set; }
        public IFormFile ProfilePicture { get; set; }

        public string BusinessAddress { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }

        public string SelfOverview { get; set; }
        public string SkillsBackground { get; set; }
        public string Achievements { get; set; }
    }

}
