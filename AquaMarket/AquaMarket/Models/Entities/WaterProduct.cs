using AquaServer.Models.Abstract;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class WaterProduct : Entity
    {
        public int OrderNum { get; set; }
        public int PackageTypeId { get; set; }
        public int WaterTypeId { get; set; }
        public int Count { get; set; } = 1;
        public decimal Volume { get; set; }
        public virtual Order Order { get; set; }
        public virtual PackageType PackageType { get; set; }
        public virtual WaterType WaterType { get; set; }
    }
}
