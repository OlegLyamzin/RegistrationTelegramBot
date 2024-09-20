using RegistrationTelegramBot.DL.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistrationTelegramBot.DL.Services
{
    public class CategoryTypeService
    {
        private readonly AppDbContext _context;

        public CategoryTypeService(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<CategoryType> CreateCategoryTypeAsync(CategoryType categoryType)
        {
            _context.CategoryType.Add(categoryType);
            await _context.SaveChangesAsync();
            return categoryType;
        }

        // Read
        public async Task<CategoryType> GetCategoryTypeByIdAsync(int id)
        {
            return await _context.CategoryType.FindAsync(id);
        }

        public async Task<List<CategoryType>> GetAllCategoryTypesAsync()
        {
            return await _context.CategoryType.ToListAsync();
        }

        // Update
        public async Task<CategoryType> UpdateCategoryTypeAsync(CategoryType categoryType)
        {
            _context.CategoryType.Update(categoryType);
            await _context.SaveChangesAsync();
            return categoryType;
        }

        // Delete
        public async Task<bool> DeleteCategoryTypeAsync(int id)
        {
            var categoryType = await _context.CategoryType.FindAsync(id);
            if (categoryType == null) return false;

            _context.CategoryType.Remove(categoryType);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
