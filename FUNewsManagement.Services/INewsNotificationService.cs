using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.Services
{
    public interface INewsNotificationService
    {
        event Func<NewsArticle, Task>? NewsCreated;
        event Func<NewsArticle, Task>? NewsUpdated;
        event Func<string, string, Task>? NewsDeleted;

        Task NotifyNewsCreatedAsync(NewsArticle news);
        Task NotifyNewsUpdatedAsync(NewsArticle news);
        Task NotifyNewsDeletedAsync(string newsId, string newsTitle);
    }
}
