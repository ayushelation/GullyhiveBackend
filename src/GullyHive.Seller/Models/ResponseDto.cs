namespace GullyHive.Seller.Models
{
    public class ResponseDto
    {
        public long Id { get; set; }
        public long LeadId { get; set; }
        public string LeadName { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public decimal QuoteAmount { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
    public class CreateResponseDto
    {
        public long LeadId { get; set; }
        public decimal QuoteAmount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
    public class UpdateResponseDto
    {
        public decimal QuoteAmount { get; set; }
        public string Message { get; set; } = string.Empty;
    }


}
