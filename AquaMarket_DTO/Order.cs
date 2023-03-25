#nullable disable

using System.ComponentModel.DataAnnotations;

namespace AquaMarket_DTO
{
    public partial class Order
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public EntityType City { get; set; }
        public Profile Client { get; set; }
        public List<OrderStatusHistory> OrderStatusHistories { get; set; }
        public List<Content> Contents { get; set; }
        public List<WaterProductInfo> WaterProducts { get; set; }
    }
}
