using AquaMarket_DTO;
using AquaServer.Domain.Market.Catalog.Filters;
using AquaServer.Extensions.Mappers;
using AquaServer.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaServer.Controllers.Market
{
    [Route("api/market")]
    [ApiController]
    [AllowAnonymous]
    public class MarketFilterController : ControllerBase
    {
        private readonly MarketFilterService<AccessoryType> _productType;
        private readonly MarketFilterService<WaterType> _waterType;
        private readonly MarketFilterService<PackageType> _packageType;
        private readonly MarketFilterService<City> _cityList;
        private readonly MarketFilterService<Status> _statusList;
        public MarketFilterController(MarketFilterService<AccessoryType> productType,
            MarketFilterService<WaterType> waterType, MarketFilterService<PackageType> packageType,
             MarketFilterService<City> cityList, MarketFilterService<Status> statusList)
        {
            _cityList = cityList;
            _statusList = statusList;
            _productType = productType;
            _packageType = packageType;
            _waterType = waterType;
        }

        [HttpGet]
        [Route("product")]
        public async Task<List<EntityType>> GetProductTypes() => (await _productType.GetAll()).Select(x=> new EntityType(x)).ToList();

        [HttpGet]
        [Route("water")]
        public async Task<List<MarketTypeWithCost>> GetWaterTypes() => await _waterType.GetAll();

        [HttpGet]
        [Route("package")]
        public async Task<List<MarketTypeWithCost>> GetPackageTypes() => await _packageType.GetAll();
        
        [HttpGet]
        [Route("city")]
        public async Task<List<EntityType>> GetCityList() => (await _cityList.GetAll()).Select(x => new EntityType(x)).ToList();

        [HttpGet]
        [Route("status")]
        [Authorize]
        public async Task<List<EntityType>> GetStatusList() => (await _statusList.GetAll()).Select(x => new EntityType(x)).ToList();
    }
}
