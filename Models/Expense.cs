using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class Expense
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = null!;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount needs to be greater than 0")]
        public decimal Amount { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = null!;
        
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}