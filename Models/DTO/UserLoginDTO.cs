using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models.DTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters.")]
        public string Username { get; set; } = null!;
        
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = false;
    }
}
