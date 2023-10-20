using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediBuyApi.Models.Domain
{
    [Table("Cart")]
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        //Navigation Properties
        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
