namespace GullyHive.Admin.Models
{
    public class QuestionDto
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public bool IsMandatory { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
