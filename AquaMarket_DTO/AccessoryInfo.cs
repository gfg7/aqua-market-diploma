using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AquaMarket_DTO
{
    public class AccessoryInfo
    {
        [Required]
        public string Article { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string Description { get; set; }
        [Required]
        public EntityType AccessoryType { get; set; }
    }
}
