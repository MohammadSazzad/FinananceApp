using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models.DTO
{
    public class UserRegistrationDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters.")]
        public string Username { get; set; } = null!;
        
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        
        [StringLength(20)]
        public string? Role { get; set; }
    }
}