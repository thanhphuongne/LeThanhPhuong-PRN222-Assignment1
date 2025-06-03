using System.ComponentModel.DataAnnotations;

namespace LeThanhPhuongMVC.Models
{
    public class AccountViewModel
    {
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(60, ErrorMessage = "Account name cannot exceed 60 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(90, ErrorMessage = "Email cannot exceed 90 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string AccountEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public int AccountRole { get; set; }

        [StringLength(90, ErrorMessage = "Password cannot exceed 90 characters")]
        [DataType(DataType.Password)]
        public string? AccountPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("AccountPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }

        public string RoleName => AccountRole switch
        {
            1 => "Admin",
            2 => "Staff",
            3 => "Lecturer",
            _ => "Unknown"
        };
    }
}
