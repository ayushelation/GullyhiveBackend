namespace GullyHive.Admin.Models
{
    public class SubCategoryMasterUpdateDto
    {
        public long CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
