using System.ComponentModel.DataAnnotations;

namespace MediBuyApi.Models.DTO
{
    public class StatusUpdateDTO
    {
        [Range(1, 7, ErrorMessage = "OrderStatusId should be between 1 and 7.")]
        public int OrderStatusId { get; set; }
    }
}
