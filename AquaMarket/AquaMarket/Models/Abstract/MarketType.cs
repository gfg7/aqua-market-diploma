using AquaServer.Interfaces.Models;
using AquaServer.Models.Abstract;

namespace AquaMarket.Models.Abstract
{
    public abstract class MarketType : MarketEntity, IMarketEntity
    {
        public float AddToCost { get; set; }

        public new float? GetCost() => AddToCost;
    }
}
