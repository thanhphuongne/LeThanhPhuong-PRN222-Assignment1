using FUNewsManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId)
        {
            return await _dbSet.Where(t => t.NewsArticles.Any(n => n.NewsArticleID == newsArticleId))
                              .ToListAsync();
        }

        public async Task<Tag?> GetByNameAsync(string tagName)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.TagName == tagName);
        }

        public async Task<bool> IsTagNameExistsAsync(string tagName, int? excludeTagId = null)
        {
            var query = _dbSet.Where(t => t.TagName == tagName);
            if (excludeTagId.HasValue)
            {
                query = query.Where(t => t.TagID != excludeTagId.Value);
            }
            return await query.AnyAsync();
        }

        public override async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _dbSet.Include(t => t.NewsArticles).ToListAsync();
        }

        public override async Task<Tag?> GetByIdAsync(object id)
        {
            return await _dbSet.Include(t => t.NewsArticles)
                              .FirstOrDefaultAsync(t => t.TagID == (int)id);
        }
    }
}
