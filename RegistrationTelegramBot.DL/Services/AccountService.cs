using RegistrationTelegramBot.DL.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistrationTelegramBot.DL.Services
{
    public class AccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Account> CreateAccountAsync(Account account)
        {
            _context.Account.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        // Read
        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Account.FindAsync(id);
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Account.ToListAsync();
        }

        // Update
        public async Task<Account> UpdateAccountAsync(Account account)
        {
            _context.Account.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        // Delete
        public async Task<bool> DeleteAccountAsync(int id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account == null) return false;

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Account>> GetAllAccountsByUserIdAsync(int userId)
        {
            return await _context.Account.Where(categoty => categoty.UserId == userId).ToListAsync();
        }
    }

}
