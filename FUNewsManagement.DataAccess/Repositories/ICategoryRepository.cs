using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetParentCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(short parentCategoryId);
        Task<bool> HasNewsArticlesAsync(short categoryId);
        Task<bool> HasSubCategoriesAsync(short categoryId);
    }
}
