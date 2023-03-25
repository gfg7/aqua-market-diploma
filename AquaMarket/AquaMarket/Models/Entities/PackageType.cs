using AquaMarket.Models.Abstract;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class PackageType : MarketType
    {
        public PackageType()
        {
            WaterProducts = new HashSet<WaterProduct>();
        }
        public virtual ICollection<WaterProduct> WaterProducts { get; set; }
    }
}
