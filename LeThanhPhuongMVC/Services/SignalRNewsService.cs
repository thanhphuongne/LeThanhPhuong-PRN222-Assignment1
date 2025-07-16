using Microsoft.AspNetCore.SignalR;
using FUNewsManagement.BusinessObjects;
using FUNewsManagement.Services;
using LeThanhPhuongMVC.Hubs;

namespace LeThanhPhuongMVC.Services
{
    public class SignalRNewsService
    {
        private readonly IHubContext<NewsHub> _hubContext;
        private readonly INewsNotificationService _notificationService;

        public SignalRNewsService(IHubContext<NewsHub> hubContext, INewsNotificationService notificationService)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
            
            // Subscribe to notification events
            _notificationService.NewsCreated += OnNewsCreated;
            _notificationService.NewsUpdated += OnNewsUpdated;
            _notificationService.NewsDeleted += OnNewsDeleted;
        }

        private async Task OnNewsCreated(NewsArticle news)
        {
            await _hubContext.Clients.Group("NewsUpdates").SendAsync("NewsCreated", new
            {
                Id = news.NewsArticleID,
                Title = news.NewsTitle,
                CategoryId = news.CategoryID,
                CreatedBy = news.CreatedBy?.AccountName ?? "Unknown",
                CreatedDate = news.CreatedDate,
                Message = $"New article '{news.NewsTitle}' has been published."
            });
        }

        private async Task OnNewsUpdated(NewsArticle news)
        {
            await _hubContext.Clients.Group("NewsUpdates").SendAsync("NewsUpdated", new
            {
                Id = news.NewsArticleID,
                Title = news.NewsTitle,
                CategoryId = news.CategoryID,
                UpdatedBy = news.UpdatedBy?.AccountName ?? "Unknown",
                ModifiedDate = news.ModifiedDate,
                Message = $"Article '{news.NewsTitle}' has been updated."
            });
        }

        private async Task OnNewsDeleted(string newsId, string newsTitle)
        {
            await _hubContext.Clients.Group("NewsUpdates").SendAsync("NewsDeleted", new
            {
                Id = newsId,
                Title = newsTitle,
                Message = $"Article '{newsTitle}' has been deleted."
            });
        }
    }
}
