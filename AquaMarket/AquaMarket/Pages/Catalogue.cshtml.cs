using AquaMarket_DTO;
using AquaServer.Domain.Market.Catalog;
using AquaServer.Domain.Market.Catalog.Filters;
using AquaServer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AquaMarketWebFront.Pages
{
    public class CatalogueModel : PageModel
    {
        
        private readonly CatalogueViewer _catalogueService;
        private readonly MarketFilterService<AccessoryType> _accessoryTypesFilterService;
        public CatalogueModel(CatalogueViewer catalogueService, MarketFilterService<AccessoryType> accessoryTypesFilterService)
        {
            _catalogueService = catalogueService;
            _accessoryTypesFilterService = accessoryTypesFilterService;
        }

        public async Task<ActionResult> OnGetFilter(int[]? productType, decimal? minPrice, decimal? maxPrice)
        {
            var accessories = await _catalogueService.GetAllByFilter(productType,minPrice,maxPrice);
            return Partial("ProductPanel", accessories);
        }

        public async Task<ActionResult> OnGetType()
        {
            var types = (await _accessoryTypesFilterService.GetAll()).Select(x=>new EntityType(x)).ToList();
            return Partial("AccessoryTypePanel", types);
        }

        public async Task<ActionResult> OnGetDetail(string article)
        {
            var accessory = await _catalogueService.GetById(article);
            return Partial("ProductDetail", accessory);
        }
    }
}
