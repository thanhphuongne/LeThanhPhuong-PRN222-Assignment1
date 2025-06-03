using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.BusinessObjects
{
    public class NewsArticle
    {
        [Key]
        [StringLength(20)]
        public string NewsArticleID { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string NewsTitle { get; set; } = string.Empty;

        [StringLength(4000)]
        public string? Headline { get; set; }

        [StringLength(4000)]
        public string? NewsContent { get; set; }

        [StringLength(800)]
        public string? NewsSource { get; set; }

        [Required]
        public short CategoryID { get; set; }

        public bool NewsStatus { get; set; } = true; // 1: Active, 0: Inactive

        [Required]
        public short CreatedByID { get; set; }

        public short? UpdatedByID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; } = null!;

        [ForeignKey("CreatedByID")]
        public virtual SystemAccount CreatedBy { get; set; } = null!;

        [ForeignKey("UpdatedByID")]
        public virtual SystemAccount? UpdatedBy { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
