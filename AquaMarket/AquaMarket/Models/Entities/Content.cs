#nullable disable

using AquaServer.Models.Abstract;

namespace AquaServer.Models.Entities
{
    public partial class Content : Entity
    {
        public int OrderNum { get; set; }
        public string Article { get; set; }
        public int Count { get; set; }
        public virtual Order Order { get; set; }
        public virtual Accessory Accessory { get; set; }
    }
}
