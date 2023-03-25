using AquaServer.Models.Abstract;
using System.Collections.Generic;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class Order : Entity
    {
        public Order()
        {
            Contents = new HashSet<Content>();
            OrderStatusHistories = new HashSet<OrderStatusHistory>();
        }

        public string ClientId { get; set; }
        public int? CityId { get; set; }
        //public string Street { get; set; }
        public string Address { get; set; }

        public virtual City City { get; set; }
        public virtual UserAccount Client { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual ICollection<WaterProduct> WaterProducts { get; set; }
        public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; }
    }
}
