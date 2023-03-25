using AquaServer.Models.Abstract;
using System.Collections.Generic;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class Accessory : UEntity
    {
        public Accessory()
        {
            Contents = new HashSet<Content>();
        }

        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int AccessoryTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual AccessoryType AccessoryType { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }
}
