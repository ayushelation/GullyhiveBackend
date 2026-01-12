namespace GullyHive.Admin.Models
{
    public class ProviderStatusMasterDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
