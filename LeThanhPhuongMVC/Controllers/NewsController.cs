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

        public async Task<IActionResult> Index(string? search, short? categoryId)
        {
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategoryId = categoryId;

            var news = await GetFilteredNews(search, categoryId);
            return View(news);
        }

        public async Task<IActionResult> Details(string id)
        {
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
