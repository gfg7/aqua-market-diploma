using AquaServer.Interfaces.Models;

namespace AquaServer.Models.Abstract
{
    public abstract class MarketEntity : Entity, IMarketEntity
    {
        public string Title { get; set; }

        public float? GetCost()
        {
            return null;
        }

        public string GetName() => Title;
    }
}
