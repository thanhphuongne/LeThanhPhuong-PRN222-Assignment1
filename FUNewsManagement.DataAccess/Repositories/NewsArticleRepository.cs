using FUNewsManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class NewsArticleRepository : Repository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        {
            return await _dbSet.Include(n => n.Category)
                              .Include(n => n.CreatedBy)
                              .Include(n => n.Tags)
                              .Where(n => n.NewsStatus)
                              .OrderByDescending(n => n.CreatedDate)
                              .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId)
        {
            return await _dbSet.Include(n => n.Category)
                              .Include(n => n.CreatedBy)
                              .Include(n => n.Tags)
                              .Where(n => n.CategoryID == categoryId)
                              .OrderByDescending(n => n.CreatedDate)
                              .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId)
        {
            return await _dbSet.Include(n => n.Category)
                              .Include(n => n.CreatedBy)
                              .Include(n => n.Tags)
                              .Where(n => n.CreatedByID == authorId)
                              .OrderByDescending(n => n.CreatedDate)
                              .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsAsync(string searchTerm)
        {
            return await _dbSet.Include(n => n.Category)
                              .Include(n => n.CreatedBy)
                              .Include(n => n.Tags)
                              .Where(n => n.NewsTitle.Contains(searchTerm) || 
                                         (n.NewsContent != null && n.NewsContent.Contains(searchTerm)))
                              .OrderByDescending(n => n.CreatedDate)
                              .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsWithTagsAsync()
        {
            return await _dbSet.Include(n => n.Category)
                              .Include(n => n.CreatedBy)
                              .Include(n => n.Tags)
                              .OrderByDescending(n => n.CreatedDate)
                              .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Include(n => n.Category)
                              .Include(n => n.CreatedBy)
                              .Include(n => n.Tags)
                              .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                              .OrderByDescending(n => n.CreatedDate)
                              .ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsWithDetailsAsync(string newsId)
        {
            return await _dbSet.Include(n => n.Category)
                              .Include(n => n.CreatedBy)
                              .Include(n => n.UpdatedBy)
                              .Include(n => n.Tags)
                              .FirstOrDefaultAsync(n => n.NewsArticleID == newsId);
        }

        public async Task<string> GenerateNewsIdAsync()
        {
            var lastNews = await _dbSet.OrderByDescending(n => n.NewsArticleID).FirstOrDefaultAsync();
            if (lastNews == null)
            {
                return "NEWS001";
            }

            var lastIdNumber = int.Parse(lastNews.NewsArticleID.Substring(4));
            var newIdNumber = lastIdNumber + 1;
            return $"NEWS{newIdNumber:D3}";
        }

        public override async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await GetNewsWithTagsAsync();
        }

        public override async Task<NewsArticle?> GetByIdAsync(object id)
        {
            return await GetNewsWithDetailsAsync(id.ToString()!);
        }
    }
}
