using FUNewsManagement.BusinessObjects;
using FUNewsManagement.DataAccess.Repositories;

namespace FUNewsManagement.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int tagId)
        {
            return await _tagRepository.GetByIdAsync(tagId);
        }

        public async Task<Tag?> GetTagByNameAsync(string tagName)
        {
            return await _tagRepository.GetByNameAsync(tagName);
        }

        public async Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId)
        {
            return await _tagRepository.GetTagsByNewsArticleAsync(newsArticleId);
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            try
            {
                if (await _tagRepository.IsTagNameExistsAsync(tag.TagName))
                {
                    return false; // Tag name already exists
                }

                await _tagRepository.AddAsync(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTagAsync(Tag tag)
        {
            try
            {
                if (await _tagRepository.IsTagNameExistsAsync(tag.TagName, tag.TagID))
                {
                    return false; // Tag name already exists for another tag
                }

                await _tagRepository.UpdateAsync(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTagAsync(int tagId)
        {
            try
            {
                await _tagRepository.DeleteByIdAsync(tagId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsTagNameExistsAsync(string tagName, int? excludeTagId = null)
        {
            return await _tagRepository.IsTagNameExistsAsync(tagName, excludeTagId);
        }
    }
}
