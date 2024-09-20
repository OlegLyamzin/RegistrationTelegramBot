using RegistrationTelegramBot.DL.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistrationTelegramBot.DL.Services
{
    public class CurrencyService
    {
        private readonly AppDbContext _context;

        public CurrencyService(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Currency> CreateCurrencyAsync(Currency currency)
        {
            _context.Currency.Add(currency);
            await _context.SaveChangesAsync();
            return currency;
        }

        // Read
        public async Task<Currency> GetCurrencyByIdAsync(int id)
        {
            return await _context.Currency.FindAsync(id);
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return await _context.Currency.ToListAsync();
        }

        // Update
        public async Task<Currency> UpdateCurrencyAsync(Currency currency)
        {
            _context.Currency.Update(currency);
            await _context.SaveChangesAsync();
            return currency;
        }

        // Delete
        public async Task<bool> DeleteCurrencyAsync(int id)
        {
            var currency = await _context.Currency.FindAsync(id);
            if (currency == null) return false;

            _context.Currency.Remove(currency);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Currency>> GetAllCurrenciesByUserIdAsync(int userId)
        {

            return await _context.Currency.Where(categoty => categoty.UserId == userId).ToListAsync();
        }
    }

}
