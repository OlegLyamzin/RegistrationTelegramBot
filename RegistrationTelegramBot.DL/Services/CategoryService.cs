using RegistrationTelegramBot.DL.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistrationTelegramBot.DL.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        // Read
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Category.FindAsync(id);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Category.ToListAsync();
        }
        public async Task<List<Category>> GetAllCategoriesByUserIdAsync(int userId)
        {
            return await _context.Category.Where(categoty=>categoty.UserId == userId).ToListAsync();
        }
        // Update
        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        // Delete
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return false;

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
