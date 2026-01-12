using System.ComponentModel.DataAnnotations;

namespace GullyHive.Auth.Models
{
    public class AuthUser
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CoverageArea { get; set; }
        public string ProfessionalType { get; set; }
        public string ServiceCategory { get; set; }
        public string BusinessName { get; set; }
        public string RegistrationType { get; set; }
        public string RegistrationNumber { get; set; }
        public string SelfOverview { get; set; }
        public string SkillsBackground { get; set; }
        public string Achievements { get; set; }

        public string RegistrationDocumentPath { get; set; }
        public string AddressProofPath { get; set; }
    }

}
