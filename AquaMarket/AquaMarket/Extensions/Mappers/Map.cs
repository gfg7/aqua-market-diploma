using AquaMarket_DTO;
using AquaServer.Interfaces.Models;
using AquaServer.Models.Entities;
using D = AquaMarket_DTO;
using E = AquaServer.Models.Entities;

namespace AquaServer.Extensions.Mappers
{
    public class Map
    {
        private readonly Map<UserAccount, Profile> acc_to_prof;
        private readonly Map<Profile, UserAccount> prof_to_acc;
        private readonly Map<Accessory, AccessoryInfo> product_to_info;
        private readonly Map<E.Content, D.Content> content_to_info;
        private readonly Map<D.Content, E.Content> info_to_content;
        private readonly Map<AccessoryInfo, Accessory> info_to_product;
        private readonly Map<WaterProduct, WaterProductInfo> water_to_info;
        private readonly Map<WaterProductInfo, WaterProduct> info_to_water;
        private readonly Map<E.Order, D.Order> order_to_dto;
        private readonly Map<E.OrderStatusHistory, D.OrderStatusHistory> history_to_dto;
        public Map(
            Map<UserAccount, Profile> acc_to_prof,
            Map<E.Content, D.Content> content_to_info,
            Map<D.Content, E.Content> info_to_content,
            Map<Profile, UserAccount> prof_to_acc,
            Map<Accessory, AccessoryInfo> product_to_info,
            Map<AccessoryInfo, Accessory> info_to_product,
            Map<WaterProduct, WaterProductInfo> water_to_info,
            Map<WaterProductInfo, WaterProduct> info_to_water,
            Map<E.Order, D.Order> order_to_dto,
            Map<E.OrderStatusHistory, D.OrderStatusHistory> history_to_dto)
        {
            this.acc_to_prof = acc_to_prof;
            this.prof_to_acc = prof_to_acc;
            this.water_to_info = water_to_info;
            this.info_to_water = info_to_water;
            this.content_to_info = content_to_info;
            this.info_to_content = info_to_content;
            this.info_to_product = info_to_product;
            this.product_to_info = product_to_info;
            this.order_to_dto = order_to_dto;
            this.history_to_dto = history_to_dto;
        }

        public MarketTypeWithCost ToDto(IMarketEntity type) => new()
        {
            Id = (int)type.GetId(),
            Title = type.GetName(),
            AddToCost = type.GetCost()
        };
        public D.OrderStatusHistory ToDto(E.OrderStatusHistory source)
        {
            var status = ToDto(source.Status);
            source.Status = null;
            var dto = history_to_dto.From(source);
            dto.Status = status;
            return dto;
        }
        public Profile ToDto(UserAccount source) => acc_to_prof.From(source);
        public D.Order ToDto(E.Order source)
        {
            var content = source?.Contents?.Select(x => ToDto(x)).ToList();
            source.Contents = null;
            var water = source?.WaterProducts?.Select(x => ToDto(x)).ToList();
            source.WaterProducts = null;
            var client = acc_to_prof.From(source.Client);
            source.Client = null;
            var city = source.City is null ? null : new EntityType(ToDto(source.City));
            source.City = null;
            var history = source?.OrderStatusHistories?.Select(x => ToDto(x)).ToList();
            source.OrderStatusHistories = null;
            var dto = order_to_dto.From(source);
            dto.OrderStatusHistories = history;
            dto.City = city;
            dto.WaterProducts = water;
            dto.Contents = content;
            dto.Client = client;
            return dto;
        }
        public UserAccount ToEntity(Profile source) => prof_to_acc.From(source);
        public D.Content ToDto(E.Content source)
        {
            var accessory = ToDto(source.Accessory);
            source.Accessory = null;
            var dto = content_to_info.From(source);
            dto.Accessory = accessory;
            return dto;
        }
        public E.Content ToEntity(D.Content source) => info_to_content.From(source);
        public AccessoryInfo ToDto(Accessory source)
        {
            var type = new EntityType(ToDto(source.AccessoryType));
            source.AccessoryType = null;
            var accessory = product_to_info.From(source);
            accessory.AccessoryType = type;
            return accessory;
        }
        public Accessory ToEntity(AccessoryInfo source)
        {
            var type = new AccessoryType()
            {
                Id = source.AccessoryType.Id,
                Title = source.AccessoryType.Title
            };
            source.AccessoryType = null;
            var accessory = info_to_product.From(source);
            accessory.AccessoryTypeId = type.Id;
            accessory.AccessoryType = type;
            return accessory;
        }
        public WaterProduct ToEntity(WaterProductInfo source) => info_to_water.From(source);
        public WaterProductInfo ToDto(WaterProduct source)
        {
            var waterType = ToDto(source.WaterType);
            var packageType = ToDto(source.PackageType);
            source.PackageType = null;
            source.WaterType = null;
            var water = water_to_info.From(source);
            water.WaterType = waterType;
            water.PackageType = packageType;
            return water;
        }
    }
}
