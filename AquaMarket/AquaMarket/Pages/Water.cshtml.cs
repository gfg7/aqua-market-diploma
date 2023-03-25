using AquaMarket_DTO;
using AquaServer.Domain.Market.Catalog.Filters;
using AquaServer.Models.Entities;
using AquaServer.Services.Market;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaServer.Pages
{
    public class WaterConstructorModel : PageModel
    {
        public List<MarketTypeWithCost> PackageTypes { get; set; }
        public List<MarketTypeWithCost> WaterTypes { get; set; }

        private readonly CartService _cartService;
        private readonly MarketFilterService<WaterType> _waterType;
        private readonly MarketFilterService<PackageType> _packageType;

        public WaterConstructorModel(CartService cartService, MarketFilterService<WaterType> waterType, MarketFilterService<PackageType> packageType)
        {
            _cartService = cartService;
            _packageType = packageType;
            _waterType = waterType;
        }
        public async Task<ActionResult> OnGet()
        {
            WaterTypes = await _waterType.GetAll();
            PackageTypes = await _packageType.GetAll();
            return Page();
        }
    }
}
