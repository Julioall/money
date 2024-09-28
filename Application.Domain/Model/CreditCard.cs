namespace Application.Domain.Model
{
    public class CreditCard
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string? Name { get; set; }
        public decimal Limit { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? PaymentAccountId { get; set; }
        public decimal? AnnualFee { get; set; }
        public string? IconUrl { get; set; }

    }
}
