namespace AquaMarket_DTO
{
    public class EntityType
    {
        public EntityType()
        {

        }
        public EntityType(MarketTypeWithCost type)
        {
            Id = type.Id;
            Title = type.Title;
        }
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
