namespace GullyHive.Admin.Models
{
    public class SubCategoryMasterDto
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
