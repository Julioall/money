using Application.Domain.Interfaces;
using Application.Domain.Models;

namespace Application.Infrastructure.Repository
{
    public class CreditCardRepository : ICreditCardsRepository
    {
        private readonly IServiceRavenDB _serviceRavenDB;

        public CreditCardRepository(IServiceRavenDB ServiceRavenDB)
        {
            _serviceRavenDB = ServiceRavenDB;
        }

        public async Task<CreditCard> AddAsync(string userId, CreditCard creditCard)
        {
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
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
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
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
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
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
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                return user?.Finance.CreditCards ?? new List<CreditCard>();
            }
        }

        public async Task<CreditCard> GetByIdAsync(string userId, string creditCardId)
        {
            using (var session = _serviceRavenDB.Store.OpenAsyncSession())
            {
                var user = await session.LoadAsync<User>(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                return user?.Finance?.CreditCards.FirstOrDefault(c => c.Id == creditCardId);
            }
        }
    }
}
