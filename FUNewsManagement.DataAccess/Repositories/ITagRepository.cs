using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId);
        Task<Tag?> GetByNameAsync(string tagName);
        Task<bool> IsTagNameExistsAsync(string tagName, int? excludeTagId = null);
    }
}
