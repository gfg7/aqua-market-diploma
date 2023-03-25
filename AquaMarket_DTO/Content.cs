#nullable disable


using System.ComponentModel.DataAnnotations;

namespace AquaMarket_DTO
{
    public partial class Content
    {
        public int Id { get; set; } 
        public int Count { get; set; } = 1;
        [Required]
        public virtual AccessoryInfo Accessory { get; set; }
    }
}
