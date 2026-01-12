namespace GullyHive.Admin.Models
{
    public class QuestionOptionDto
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string OptionText { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
