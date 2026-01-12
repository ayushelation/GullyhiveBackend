namespace GullyHive.Seller.Models
{
    public class HelpCategoryDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Icon { get; set; } = "";
        public int Articles { get; set; }
    }

    public class HelpFaqDto
    {
        public long Id { get; set; }
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
    }

}
