using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Models
{
    public class UpdateProductRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Range(0.01, 100000)]
        public decimal Price { get; set; }
        [Required]
        [RegularExpression("^[A-Z]{3}-\\d{4}$", ErrorMessage = "SKU must look like ABC-1234.")]
        public string Sku { get; set; } = string.Empty;
    }
}
