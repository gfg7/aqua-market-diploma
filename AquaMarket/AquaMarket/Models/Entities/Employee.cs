using AquaServer.Models.Abstract;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class Employee : Person
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
        public int PositionId { get; set; }
        public bool? IsAccActive { get; set; }
        public virtual UserAccount Account { get; set; }
        public virtual Position Position { get; set; }
    }
}
