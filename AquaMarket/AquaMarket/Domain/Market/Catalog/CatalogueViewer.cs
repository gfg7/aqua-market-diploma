using AquaMarket_DTO;
using AquaServer.Extensions.Helper;
using AquaServer.Extensions.Mappers;
using AquaServer.Interfaces.Database;
using AquaServer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AquaServer.Domain.Market.Catalog
{
    public class CatalogueViewer
    {
        private readonly IRepository<Accessory> _repository;
        private readonly Map _map;
        private readonly WWWRootResources _resources;
        public CatalogueViewer(IRepository<Accessory> repository, Map map, WWWRootResources resources)
        {
            _map = map;
            _resources = resources;
            _repository = repository;
        }

        public async Task<List<AccessoryInfo>> GetAllByFilter(int[]? productType, decimal? minPrice, decimal? maxPrice)
        {
            var found = await (await _repository.Get()).Include(x=> x.AccessoryType).ToListAsync();

            if (productType?.Length>0)
            {
                found = found.Where(x => productType.Contains(x.AccessoryTypeId)).ToList();
            }

            if (minPrice != null)
            {
                found = found.Where(x => x.Price >= minPrice).ToList();
            }

            if (maxPrice != null)
            {
                found = found.Where(x => x.Price <= maxPrice).ToList();
            }

            var result = found.Select(x => _map.ToDto(x)).ToList();

            return result;
        }

        public async Task<AccessoryInfo> GetById(IComparable id) => _map.ToDto(await (await _repository.Get()).Include(x=> x.AccessoryType).FirstAsync(x=> x.Article == id.ToString()));

        public async Task<AccessoryInfo> Update(string article, AccessoryInfo info, bool hide = false)
        {
            var upd = await _repository.GetById(article);

            upd.Title = info.Title;
            upd.Description= info.Description;
            upd.Price = info.Price;
            upd.IsDeleted= hide;

            await _repository.Update(upd);
            
            return await GetById(article);
        }

        public async Task<AccessoryInfo> Create(AccessoryInfo info)
        {
            var product = _map.ToEntity(info);

            product = await _repository.Create(product);

            info = _map.ToDto(product);

            return info;
        }

        public async Task SavePic(string article, IFormFile uploadedFile) => await _resources.WriteResource(article, uploadedFile);
    }
}
