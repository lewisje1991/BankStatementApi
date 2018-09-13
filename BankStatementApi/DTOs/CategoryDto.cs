using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.DTOs
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string TransactionNames { get; set; }
        [Required]
        public decimal Target { get; set; }
    }
}