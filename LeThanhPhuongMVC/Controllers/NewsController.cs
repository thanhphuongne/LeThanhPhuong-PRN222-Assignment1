using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Services;

namespace LeThanhPhuongMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsArticleService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        private void SetUserContext()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetInt32("UserRole");
            var userName = HttpContext.Session.GetString("UserName");

            ViewBag.CurrentUserName = userName;
            ViewBag.CurrentUserRole = userRole;
            ViewBag.IsAdmin = userRole == 3;
            ViewBag.IsStaff = userRole == 1;
            ViewBag.IsLecturer = userRole == 2;
        }

        public async Task<IActionResult> Index(string? search, short? categoryId)
        {
            // Set user context for navigation
            SetUserContext();

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategoryId = categoryId;

            var news = await GetFilteredNews(search, categoryId);
            return View(news);
        }

        public async Task<IActionResult> Details(string id)
        {
            SetUserContext();

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var newsArticle = await _newsService.GetNewsByIdAsync(id);
            if (newsArticle == null || !newsArticle.NewsStatus)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            SetUserContext();

            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.CurrentSearch = searchTerm;

            var news = await _newsService.SearchNewsAsync(searchTerm);
            return View("Index", news);
        }

        [HttpGet]
        public async Task<IActionResult> Category(short id)
        {
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.CurrentCategoryId = id;

            var news = await _newsService.GetNewsByCategoryAsync(id);
            return View("Index", news);
        }

        private async Task<IEnumerable<FUNewsManagement.BusinessObjects.NewsArticle>> GetFilteredNews(string? search, short? categoryId)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await _newsService.SearchNewsAsync(search);
            }

            if (categoryId.HasValue)
            {
                return await _newsService.GetNewsByCategoryAsync(categoryId.Value);
            }

            return await _newsService.GetActiveNewsAsync();
        }
    }
}
