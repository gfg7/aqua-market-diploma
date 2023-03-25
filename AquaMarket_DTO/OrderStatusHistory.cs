#nullable disable

using System.ComponentModel.DataAnnotations;

namespace AquaMarket_DTO
{
    public partial class OrderStatusHistory
    {
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        [Required]
        public virtual EntityType Status { get; set; }
    }
}
