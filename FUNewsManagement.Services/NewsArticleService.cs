using FUNewsManagement.BusinessObjects;
using FUNewsManagement.DataAccess.Repositories;

namespace FUNewsManagement.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly ITagRepository _tagRepository;

        public NewsArticleService(INewsArticleRepository newsRepository, ITagRepository tagRepository)
        {
            _newsRepository = newsRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsAsync()
        {
            return await _newsRepository.GetAllAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        {
            return await _newsRepository.GetActiveNewsAsync();
        }

        public async Task<NewsArticle?> GetNewsByIdAsync(string newsId)
        {
            return await _newsRepository.GetNewsWithDetailsAsync(newsId);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId)
        {
            return await _newsRepository.GetNewsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId)
        {
            return await _newsRepository.GetNewsByAuthorAsync(authorId);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsAsync(string searchTerm)
        {
            return await _newsRepository.SearchNewsAsync(searchTerm);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _newsRepository.GetNewsByDateRangeAsync(startDate, endDate);
        }

        public async Task<bool> CreateNewsAsync(NewsArticle news, List<int> tagIds)
        {
            try
            {
                news.NewsArticleID = await GenerateNewsIdAsync();
                news.CreatedDate = DateTime.Now;

                // Get tags
                var tags = new List<Tag>();
                foreach (var tagId in tagIds)
                {
                    var tag = await _tagRepository.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        tags.Add(tag);
                    }
                }
                news.Tags = tags;

                await _newsRepository.AddAsync(news);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateNewsAsync(NewsArticle news, List<int> tagIds, short updatedByID)
        {
            try
            {
                var existingNews = await _newsRepository.GetNewsWithDetailsAsync(news.NewsArticleID);
                if (existingNews == null) return false;

                // Update properties
                existingNews.NewsTitle = news.NewsTitle;
                existingNews.Headline = news.Headline;
                existingNews.NewsContent = news.NewsContent;
                existingNews.NewsSource = news.NewsSource;
                existingNews.CategoryID = news.CategoryID;
                existingNews.NewsStatus = news.NewsStatus;
                existingNews.UpdatedByID = updatedByID;
                existingNews.ModifiedDate = DateTime.Now;

                // Update tags
                existingNews.Tags.Clear();
                var tags = new List<Tag>();
                foreach (var tagId in tagIds)
                {
                    var tag = await _tagRepository.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        tags.Add(tag);
                    }
                }
                existingNews.Tags = tags;

                await _newsRepository.UpdateAsync(existingNews);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteNewsAsync(string newsId)
        {
            try
            {
                await _newsRepository.DeleteByIdAsync(newsId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GenerateNewsIdAsync()
        {
            return await _newsRepository.GenerateNewsIdAsync();
        }
    }
}
