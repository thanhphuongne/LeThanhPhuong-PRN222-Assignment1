using FUNewsManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        {
            return await _dbSet.Where(c => c.ParentCategoryID == null).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(short parentCategoryId)
        {
            return await _dbSet.Where(c => c.ParentCategoryID == parentCategoryId).ToListAsync();
        }

        public async Task<bool> HasNewsArticlesAsync(short categoryId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryID == categoryId);
        }

        public async Task<bool> HasSubCategoriesAsync(short categoryId)
        {
            return await _dbSet.AnyAsync(c => c.ParentCategoryID == categoryId);
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet.Include(c => c.ParentCategory).ToListAsync();
        }

        public override async Task<Category?> GetByIdAsync(object id)
        {
            return await _dbSet.Include(c => c.ParentCategory)
                              .Include(c => c.SubCategories)
                              .FirstOrDefaultAsync(c => c.CategoryID == (short)id);
        }
    }
}
