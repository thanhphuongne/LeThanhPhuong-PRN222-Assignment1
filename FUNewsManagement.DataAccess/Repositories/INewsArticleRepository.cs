using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface INewsArticleRepository : IRepository<NewsArticle>
    {
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId);
        Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId);
        Task<IEnumerable<NewsArticle>> SearchNewsAsync(string searchTerm);
        Task<IEnumerable<NewsArticle>> GetNewsWithTagsAsync();
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<NewsArticle?> GetNewsWithDetailsAsync(string newsId);
        Task<string> GenerateNewsIdAsync();
    }
}
