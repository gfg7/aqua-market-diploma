using System.ComponentModel.DataAnnotations;

namespace AquaMarket_DTO
{
    public class WaterProductInfo
    {
        [Required]
        public int WaterTypeId { get; set; }
        public int Id{ get; set; }
        public MarketTypeWithCost? WaterType { get; set; }
        public int Count { get; set; } = 1;
        [Required]
        public decimal Volume { get; set; }
        [Required]
        public int PackageTypeId { get; set; }
        public MarketTypeWithCost? PackageType { get; set; }
    }
}
