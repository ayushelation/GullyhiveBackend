namespace GullyHive.Admin.Models
{
    public class ServiceCategoryMasterCreateDto
    {
        public long? ParentId { get; set; }
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public bool IsLeaf { get; set; }
        public int DisplayOrder { get; set; }
    }
}
