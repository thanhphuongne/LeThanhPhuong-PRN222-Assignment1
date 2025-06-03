using System.ComponentModel.DataAnnotations;
using FUNewsManagement.BusinessObjects;

namespace LeThanhPhuongMVC.Models
{
    public class NewsViewModel
    {
        public string? NewsArticleID { get; set; }

        [Required(ErrorMessage = "News title is required")]
        [StringLength(200, ErrorMessage = "News title cannot exceed 200 characters")]
        public string NewsTitle { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Headline cannot exceed 4000 characters")]
        public string? Headline { get; set; }

        [StringLength(4000, ErrorMessage = "News content cannot exceed 4000 characters")]
        public string? NewsContent { get; set; }

        [StringLength(800, ErrorMessage = "News source cannot exceed 800 characters")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public short CategoryID { get; set; }

        public bool NewsStatus { get; set; } = true;

        public short CreatedByID { get; set; }

        public short? UpdatedByID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties for display
        public Category? Category { get; set; }
        public SystemAccount? CreatedBy { get; set; }
        public SystemAccount? UpdatedBy { get; set; }
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        // For form handling
        public List<int> SelectedTagIds { get; set; } = new List<int>();
    }

    public class CategoryViewModel
    {
        public short CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Category description cannot exceed 250 characters")]
        public string? CategoryDescription { get; set; }

        public short? ParentCategoryID { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties for display
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public int NewsArticleCount { get; set; }
    }

    public class TagViewModel
    {
        public int TagID { get; set; }

        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters")]
        public string TagName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Note cannot exceed 250 characters")]
        public string? Note { get; set; }

        public int NewsArticleCount { get; set; }
    }

    public class ReportViewModel
    {
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public IEnumerable<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

        public int TotalArticles { get; set; }
        public int ActiveArticles { get; set; }
        public int InactiveArticles { get; set; }
        public Dictionary<string, int> ArticlesByCategory { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ArticlesByAuthor { get; set; } = new Dictionary<string, int>();
    }
}
