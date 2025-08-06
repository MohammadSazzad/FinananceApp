using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required, StringLength(100)]
        public string Username { get; set; } = null!;
        
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;
        
        [Required]
        public string PasswordHash { get; set; } = null!; 

        [StringLength(20)]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Expense> Expenses { get; set; } = new();
    }
}