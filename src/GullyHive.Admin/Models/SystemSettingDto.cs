namespace GullyHive.Admin.Models
{
    public class SystemSettingDto
    {
        public string Key { get; set; } = null!;
        public string? Value { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
