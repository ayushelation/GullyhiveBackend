namespace GullyHive.Admin.Models
{
    public class SystemSettingCreateUpdateDto
    {
        public string Key { get; set; } = null!;
        public string? Value { get; set; }
    }
}
