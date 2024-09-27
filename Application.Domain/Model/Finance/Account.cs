using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Transactions;

namespace Application.Domain.Model.Finance
{
    public class Account
    {
        [BsonId]
        public string? Id { get; set; }
        public required string UserId { get; set; }  // Relaciona a conta ao usuário
        public required string AccountName { get; set; }  // Nome da conta (ex: Banco XYZ, Carteira Digital)
        public required decimal Balance { get; set; }  // Saldo atual da conta
        public List<FinancialTransaction> Transactions { get; set; } = new();  // Transações associadas à conta
    }
}
