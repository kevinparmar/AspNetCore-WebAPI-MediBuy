using System.ComponentModel.DataAnnotations.Schema;

namespace MediBuyApi.Models.Domain
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        //Navigation Properties
        public ICollection<Product> Product { get; set; }
    }
}
