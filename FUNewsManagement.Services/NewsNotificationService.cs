using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.Services
{
    public class NewsNotificationService : INewsNotificationService
    {
        // This service will be injected with SignalR context in the MVC layer
        public event Func<NewsArticle, Task>? NewsCreated;
        public event Func<NewsArticle, Task>? NewsUpdated;
        public event Func<string, string, Task>? NewsDeleted;

        public async Task NotifyNewsCreatedAsync(NewsArticle news)
        {
            if (NewsCreated != null)
                await NewsCreated(news);
        }

        public async Task NotifyNewsUpdatedAsync(NewsArticle news)
        {
            if (NewsUpdated != null)
                await NewsUpdated(news);
        }

        public async Task NotifyNewsDeletedAsync(string newsId, string newsTitle)
        {
            if (NewsDeleted != null)
                await NewsDeleted(newsId, newsTitle);
        }
    }
}
