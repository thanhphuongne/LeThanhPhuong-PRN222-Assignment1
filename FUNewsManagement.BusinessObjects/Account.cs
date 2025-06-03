using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.BusinessObjects
{
    [Table("SystemAccount")]
    public class SystemAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short AccountID { get; set; }

        [Required]
        [StringLength(60)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [StringLength(90)]
        [EmailAddress]
        public string AccountEmail { get; set; } = string.Empty;

        [Required]
        public int AccountRole { get; set; } // 1: Admin, 2: Staff, 3: Lecturer

        [Required]
        [StringLength(90)]
        public string AccountPassword { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
