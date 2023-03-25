using AquaMarket_DTO;
using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Domain.Market.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaServer.Controllers.Market
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CatalogueController : ControllerBase
    {
        private readonly CatalogueViewer _catalogueService;
        public CatalogueController(CatalogueViewer catalogueService)
        {
            _catalogueService = catalogueService;
        }

        [HttpGet]
        [Route("product")]
        public async Task<List<AccessoryInfo>> GetAllByFilter(int[]? productType, decimal? minPrice, decimal? maxPrice) => await _catalogueService.GetAllByFilter(productType, minPrice, maxPrice);

        [HttpGet]
        [Route("product/{article}")]
        public async Task<AccessoryInfo> GetProduct(string article) => await _catalogueService.GetById(article);

        [HttpPost]
        [Route("product")]
        [Authorize(Roles = nameof(AccessRole.Manager))]
        public async Task<AccessoryInfo> CreateProduct(AccessoryInfo accessory) => await _catalogueService.Create(accessory);

        [HttpPut]
        [Route("product/{article}")]
        [Authorize(Roles = nameof(AccessRole.Manager))]
        public async Task<AccessoryInfo> EditProduct(string article, AccessoryInfo accessory) => await _catalogueService.Update(article, accessory);

        [HttpPost]
        [Route("product/{article}/picture")]
        [Authorize(Roles = nameof(AccessRole.Manager))]
        public async Task SavePic(string article, IFormFile picture) => await _catalogueService.SavePic(article, picture);
    }
}
