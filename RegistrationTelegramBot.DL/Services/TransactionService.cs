using RegistrationTelegramBot.DL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationTelegramBot.DL.Services
{
    public class TransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            transaction.CreatedOn = DateTime.UtcNow;
            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        // Read
        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _context.Transaction.FindAsync(id);
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transaction.ToListAsync();
        }

        // Update
        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transaction.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        // Delete
        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null) return false;

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Transaction>> GetAllTransactionsByUserIdAsync(int userId)
        {

            return await _context.Transaction.Where(trans => trans.UserId == userId).ToListAsync();
        }
    }

}
