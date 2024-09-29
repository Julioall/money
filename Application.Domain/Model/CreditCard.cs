namespace Application.Domain.Model
{
    public class CreditCard
    {
        public string? Id { get; set; }
        public required string UserId { get; set; }
        public required string CardName { get; set; }
        public required decimal Limit { get; set; }
        public required DateTime ClosingDate { get; set; }
        public required DateTime DueDate { get; set; }
        public required string PaymentAccountId { get; set; }
        public decimal? AnnualFee { get; set; }
        public required string IconUrl { get; set; }

    }
}
