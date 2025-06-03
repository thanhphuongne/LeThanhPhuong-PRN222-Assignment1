using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int tagId);
        Task<Tag?> GetTagByNameAsync(string tagName);
        Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId);
        Task<bool> CreateTagAsync(Tag tag);
        Task<bool> UpdateTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(int tagId);
        Task<bool> IsTagNameExistsAsync(string tagName, int? excludeTagId = null);
    }
}
