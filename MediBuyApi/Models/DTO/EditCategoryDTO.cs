using System.ComponentModel.DataAnnotations;

namespace MediBuyApi.Models.DTO
{
    public class EditCategoryDTO
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Category Name cannot be more than 20 characters")]
        public string CategoryName { get; set; }
    }
}
