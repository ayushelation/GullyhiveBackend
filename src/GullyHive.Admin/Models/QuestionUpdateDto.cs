namespace GullyHive.Admin.Models
{
    public class QuestionUpdateDto
    {
        public string QuestionText { get; set; } = null!;
        public bool IsMandatory { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
