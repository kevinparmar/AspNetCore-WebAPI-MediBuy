namespace MediBuyApi.Models.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ProductWithoutCategory> Products { get; set; }
    }
}
