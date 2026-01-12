namespace GullyHive.Admin.Models
{
    public class QuestionCreateDto
    {
        public long CategoryId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public bool IsMandatory { get; set; }
        public int DisplayOrder { get; set; }
    }
}
