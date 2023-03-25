using AquaMarket.Models.Abstract;
using AquaServer.Models.Abstract;
using System.Collections.Generic;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class WaterType : MarketType
    {
        public WaterType()
        {
            WaterProducts = new HashSet<WaterProduct>();
        }
        public virtual ICollection<WaterProduct> WaterProducts { get; set; }
    }
}
