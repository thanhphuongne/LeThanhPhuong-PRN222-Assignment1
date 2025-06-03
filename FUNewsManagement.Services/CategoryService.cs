using FUNewsManagement.BusinessObjects;
using FUNewsManagement.DataAccess.Repositories;

namespace FUNewsManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _categoryRepository.GetActiveCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(short categoryId)
        {
            return await _categoryRepository.GetByIdAsync(categoryId);
        }

        public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        {
            return await _categoryRepository.GetParentCategoriesAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(short parentCategoryId)
        {
            return await _categoryRepository.GetSubCategoriesAsync(parentCategoryId);
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            try
            {
                await _categoryRepository.AddAsync(category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                await _categoryRepository.UpdateAsync(category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(short categoryId)
        {
            try
            {
                if (!await CanDeleteCategoryAsync(categoryId))
                {
                    return false;
                }

                await _categoryRepository.DeleteByIdAsync(categoryId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CanDeleteCategoryAsync(short categoryId)
        {
            // Check if category has news articles
            if (await _categoryRepository.HasNewsArticlesAsync(categoryId))
            {
                return false;
            }

            // Check if category has subcategories
            if (await _categoryRepository.HasSubCategoriesAsync(categoryId))
            {
                return false;
            }

            return true;
        }
    }
}
