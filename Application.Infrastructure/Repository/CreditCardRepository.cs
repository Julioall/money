using Application.Domain.Interface;
using Application.Domain.Model;
using Raven.Client.Documents;

namespace Application.Infrastructure.Repository
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly IDocumentStore _documentStore;

        public CreditCardRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task AddAsync(string userId, CreditCard creditCard)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    user.Finance.CreditCards.Add(creditCard);
                    await session.StoreAsync(user);
                    await session.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateAsync(string userId, CreditCard creditCard)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    var existingCreditCard = user.Finance.CreditCards.FirstOrDefault(c => c.Id == creditCard.Id);
                    if (existingCreditCard != null)
                    {
                        existingCreditCard.Name = creditCard.Name;
                        existingCreditCard.Limit = creditCard.Limit;
                        existingCreditCard.ClosingDate = creditCard.ClosingDate;
                        existingCreditCard.DueDate = creditCard.DueDate;
                        existingCreditCard.PaymentAccountId = creditCard.PaymentAccountId;
                        existingCreditCard.AnnualFee = creditCard.AnnualFee;
                        existingCreditCard.IconUrl = creditCard.IconUrl;

                        await session.StoreAsync(user);
                        await session.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task DeleteAsync(string userId, string creditCardId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user != null)
                {
                    var creditCardToRemove = user.Finance.CreditCards.FirstOrDefault(c => c.Id == creditCardId);
                    if (creditCardToRemove != null)
                    {
                        user.Finance.CreditCards.Remove(creditCardToRemove);
                        await session.StoreAsync(user);
                        await session.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<IEnumerable<CreditCard>> GetAllAsync(string userId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                return user?.Finance.CreditCards ?? new List<CreditCard>();
            }
        }

        public async Task<CreditCard> GetByIdAsync(string userId, string creditCardId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                return user?.Finance?.CreditCards.FirstOrDefault(c => c.Id == creditCardId);
            }
        }

    }
}
