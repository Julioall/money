using Application.Domain.Models.Enums;

namespace Application.Domain.Models
{
    public class Transaction
    {
        public string? Id { get; set; }
        public required string UserId { get; set; }
        public required decimal Amount { get; set; }
        public required string CategoryId { get; set; }
        public required DateTime Date { get; set; }
        public string? Description { get; set; }
        public required TransactionType Type { get; set; }
        public required string AccountDestinationId { get; set; }
        public string? CreditCardDestinationId { get; set; }
        public bool IsRecurring { get; set; }
        public int? TotalInstallments { get; set; }
        public int? CurrentInstallment { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public string? ParentTransactionId { get; set; }
        public string? Tag { get; set; }
    }
}
