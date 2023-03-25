namespace AquaServer.Interfaces.Models
{
    public interface IMarketEntity:IEntity
    {
        string GetName();
        float? GetCost();
    }
}
