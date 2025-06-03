using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Services;
using FUNewsManagement.BusinessObjects;

namespace LeThanhPhuongMVC.Controllers
{
    public class StaffController : StaffOnlyController
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public StaffController(INewsArticleService newsService, ICategoryService categoryService, ITagService tagService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var myNews = await _newsService.GetNewsByAuthorAsync((short)CurrentUserId!);
            return View(myNews);
        }

        [HttpGet]
        public async Task<IActionResult> ManageNews()
        {
            var myNews = await _newsService.GetNewsByAuthorAsync((short)CurrentUserId!);
            return View(myNews);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNews()
        {
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Tags = await _tagService.GetAllTagsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(NewsArticle model, List<int> selectedTags)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
                ViewBag.Tags = await _tagService.GetAllTagsAsync();
                return View(model);
            }

            model.CreatedByID = (short)CurrentUserId!;
            model.CreatedDate = DateTime.Now;

            var result = await _newsService.CreateNewsAsync(model, selectedTags ?? new List<int>());
            if (result)
            {
                TempData["SuccessMessage"] = "News article created successfully.";
                return RedirectToAction("ManageNews");
            }

            ModelState.AddModelError("", "Failed to create news article.");
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Tags = await _tagService.GetAllTagsAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditNews(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null || news.CreatedByID != CurrentUserId)
            {
                return NotFound();
            }

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Tags = await _tagService.GetAllTagsAsync();
            ViewBag.SelectedTags = news.Tags.Select(t => t.TagID).ToList();

            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> EditNews(NewsArticle model, List<int> selectedTags)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
                ViewBag.Tags = await _tagService.GetAllTagsAsync();
                return View(model);
            }

            var existingNews = await _newsService.GetNewsByIdAsync(model.NewsArticleID);
            if (existingNews == null || existingNews.CreatedByID != CurrentUserId)
            {
                return NotFound();
            }

            var result = await _newsService.UpdateNewsAsync(model, selectedTags ?? new List<int>());
            if (result)
            {
                TempData["SuccessMessage"] = "News article updated successfully.";
                return RedirectToAction("ManageNews");
            }

            ModelState.AddModelError("", "Failed to update news article.");
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Tags = await _tagService.GetAllTagsAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null || news.CreatedByID != CurrentUserId)
            {
                return NotFound();
            }

            var result = await _newsService.DeleteNewsAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "News article deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete news article.";
            }

            return RedirectToAction("ManageNews");
        }

        [HttpGet]
        public async Task<IActionResult> ManageCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            ViewBag.ParentCategories = await _categoryService.GetParentCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = await _categoryService.GetParentCategoriesAsync();
                return View(model);
            }

            var result = await _categoryService.CreateCategoryAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Category created successfully.";
                return RedirectToAction("ManageCategories");
            }

            ModelState.AddModelError("", "Failed to create category.");
            ViewBag.ParentCategories = await _categoryService.GetParentCategoriesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(short id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.ParentCategories = await _categoryService.GetParentCategoriesAsync();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = await _categoryService.GetParentCategoriesAsync();
                return View(model);
            }

            var result = await _categoryService.UpdateCategoryAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Category updated successfully.";
                return RedirectToAction("ManageCategories");
            }

            ModelState.AddModelError("", "Failed to update category.");
            ViewBag.ParentCategories = await _categoryService.GetParentCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(short id)
        {
            if (!await _categoryService.CanDeleteCategoryAsync(id))
            {
                TempData["ErrorMessage"] = "Cannot delete category. It has associated news articles or subcategories.";
                return RedirectToAction("ManageCategories");
            }

            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Category deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete category.";
            }

            return RedirectToAction("ManageCategories");
        }

        [HttpGet]
        public async Task<IActionResult> ManageTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }

        [HttpGet]
        public IActionResult CreateTag()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(Tag model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _tagService.CreateTagAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Tag created successfully.";
                return RedirectToAction("ManageTags");
            }

            ModelState.AddModelError("", "Failed to create tag. Tag name may already exist.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditTag(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> EditTag(Tag model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _tagService.UpdateTagAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Tag updated successfully.";
                return RedirectToAction("ManageTags");
            }

            ModelState.AddModelError("", "Failed to update tag. Tag name may already exist.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var result = await _tagService.DeleteTagAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Tag deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete tag.";
            }

            return RedirectToAction("ManageTags");
        }
    }
}
