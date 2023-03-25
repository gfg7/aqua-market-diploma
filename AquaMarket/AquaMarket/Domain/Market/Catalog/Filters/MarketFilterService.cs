using AquaMarket_DTO;
using AquaServer.Extensions.Mappers;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaServer.Domain.Market.Catalog.Filters
{
    public class MarketFilterService<T> where T : IMarketEntity, new()
    {
        private readonly IRepository<T> _repository;
        private readonly Map _map;
        public MarketFilterService(IRepository<T> repository, Map map)
        {
            _map = map;
            _repository = repository;
        }

        public async Task<List<MarketTypeWithCost>> GetAll()
        {
            var types = await _repository.Get();

            return types.Select(x => _map.ToDto(x)).ToList();
        }
    }
}
