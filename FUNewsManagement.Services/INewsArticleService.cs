using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.Services
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticle>> GetAllNewsAsync();
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
        Task<NewsArticle?> GetNewsByIdAsync(string newsId);
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId);
        Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId);
        Task<IEnumerable<NewsArticle>> SearchNewsAsync(string searchTerm);
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> CreateNewsAsync(NewsArticle news, List<int> tagIds);
        Task<bool> UpdateNewsAsync(NewsArticle news, List<int> tagIds, short updatedByID);
        Task<bool> DeleteNewsAsync(string newsId);
        Task<string> GenerateNewsIdAsync();
    }
}
