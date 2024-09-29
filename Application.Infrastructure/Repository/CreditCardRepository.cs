using Application.Domain.Interface;
using Application.Domain.Model;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Infrastructure.Repository
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly IDocumentStore _documentStore;

        public CreditCardRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public async Task<CreditCard> AddAsync(string userId, CreditCard creditCard)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                user.Finance.CreditCards.Add(creditCard);
                await session.StoreAsync(user);
                await session.SaveChangesAsync();
                return creditCard;

            }
        }

        public async Task<CreditCard> UpdateAsync(string userId, CreditCard creditCard)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                var existingCreditCard = user.Finance.CreditCards.FirstOrDefault(c => c.Id == creditCard.Id);
                if (existingCreditCard == null)
                    throw new InvalidOperationException("Credit card not found.");

                existingCreditCard.CardName = creditCard.CardName;
                existingCreditCard.Limit = creditCard.Limit;
                existingCreditCard.ClosingDate = creditCard.ClosingDate;
                existingCreditCard.DueDate = creditCard.DueDate;
                existingCreditCard.PaymentAccountId = creditCard.PaymentAccountId;
                existingCreditCard.AnnualFee = creditCard.AnnualFee;
                existingCreditCard.IconUrl = creditCard.IconUrl;

                await session.SaveChangesAsync();
                return existingCreditCard;
            }
        }


        public async Task<bool> DeleteAsync(string userId, string creditCardId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    return false;

                var creditCardToRemove = user.Finance.CreditCards.FirstOrDefault(c => c.Id == creditCardId);
                if (creditCardToRemove == null)
                    return false;

                user.Finance.CreditCards.Remove(creditCardToRemove);
                await session.StoreAsync(user);
                await session.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<CreditCard>> GetAllAsync(string userId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                return user?.Finance.CreditCards ?? new List<CreditCard>();
            }
        }

        public async Task<CreditCard> GetByIdAsync(string userId, string creditCardId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                return user?.Finance?.CreditCards.FirstOrDefault(c => c.Id == creditCardId);
            }
        }
    }
}
