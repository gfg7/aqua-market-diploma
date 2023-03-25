using AquaMarket_DTO;
using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Domain.Market.Cart;
using AquaServer.Domain.Market.Catalog;
using AquaServer.Extensions.Exceptions;
using AquaServer.Extensions.Helper;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AquaServer.Controllers.Market
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReceiptController : ControllerBase
    {
        private readonly IConverter _converter;
        private string _template;
        private WWWRootResources _resources;
        private readonly IOrderViewer _orderViewer;
        private readonly CatalogueViewer _catalogueViewer;

        public ReceiptController(IOrderViewer orderViewer, IConverter converter, WWWRootResources resources, CatalogueViewer catalogueViewer)
        {
            _resources = resources;
            _orderViewer = orderViewer;
            _catalogueViewer = catalogueViewer;
            _converter = converter;
        }

        private async Task<string> GetTemplate(Order order)
        {
            _template = _template.Replace("00000", order.Id.ToString())//доделать темплейт
                .Replace("Иванов Иван Иванович", string.Join(" ", order?.Client?.Surname, order?.Client?.Name, order?.Client?.Patronymic))
                .Replace("DATE1", order?.OrderStatusHistories?.OrderByDescending(x=>x.Date).First().Date.ToShortDateString())
                .Replace("DATE", DateTime.Now.Date.ToString("dd.MM.yyyy"));
            string content = "";
            for (int i = 0;i<order?.Contents?.Count;i++)
            {
                var item = order.Contents.ElementAt(i);
                content += $"<tr>   <td width=36 valign=top style= 'width:26.7pt;border:solid #B4C6E7 1.0pt;   border-top:none;padding:0cm 5.4pt 0cm 5.4pt'>   <p class=MsoNormal style='margin-bottom:0cm;line-height:normal'><span   style= 'font-size:12.0pt'>{i+1}</span></p>   </td>   <td width=418 valign=top style='width:313.25pt;border-top:none;border-left:   none;border-bottom:solid #B4C6E7 1.0pt;border-right:solid #B4C6E7 1.0pt;   padding:0cm 5.4pt 0cm 5.4pt'>   <p class=MsoNormal style='margin-bottom:0cm;line-height:normal'><span   style='font-size:12.0pt'>{item.Accessory.Title} </span></p>   </td>   <td width=95 valign=top style='width:70.9pt;border-top:none;border-left:none;   border-bottom:solid #B4C6E7 1.0pt;border-right:solid #B4C6E7 1.0pt;   padding:0cm 5.4pt 0cm 5.4pt'>   <p class=MsoNormal style='margin-bottom:0cm;line-height:normal'><span   style='font-size:12.0pt'>{item.Count}</span></p>   </td>   <td width=149 valign= top style='width:111.95pt;border-top:none;border-left:   none;border-bottom:solid #B4C6E7 1.0pt;border-right:solid #B4C6E7 1.0pt;   padding:0cm 5.4pt 0cm 5.4pt'>   <p class= MsoNormal style='margin-bottom:0cm;line-height:normal'><span   style='font-size:12.0pt'>{item.Accessory.Price}</span></p>   </td>  </tr>  ";
            }
            for (int i = 0; i < order?.WaterProducts?.Count; i++)
            {
                var item = order.WaterProducts.ElementAt(i);
                var v = item.Volume;
                if (v < 1)
                {
                    v = Math.Abs(1 - item.Volume);
                }
                var price =  v * (decimal)(item.WaterType.AddToCost + item.PackageType.AddToCost) * item.Count;
                content += $"<tr>   <td width=36 valign=top style= 'width:26.7pt;border:solid #B4C6E7 1.0pt;   border-top:none;padding:0cm 5.4pt 0cm 5.4pt'>   <p class=MsoNormal style='margin-bottom:0cm;line-height:normal'><span   style= 'font-size:12.0pt'>{i + 1 + order.Contents.Count}</span></p>   </td>   <td width=418 valign=top style='width:313.25pt;border-top:none;border-left:   none;border-bottom:solid #B4C6E7 1.0pt;border-right:solid #B4C6E7 1.0pt;   padding:0cm 5.4pt 0cm 5.4pt'>   <p class=MsoNormal style='margin-bottom:0cm;line-height:normal'><span   style='font-size:12.0pt'>{string.Join(' ', item.WaterType.Title, item.PackageType.Title, item.Volume, 'л')} </span></p>   </td>   <td width=95 valign=top style='width:70.9pt;border-top:none;border-left:none;   border-bottom:solid #B4C6E7 1.0pt;border-right:solid #B4C6E7 1.0pt;   padding:0cm 5.4pt 0cm 5.4pt'>   <p class=MsoNormal style='margin-bottom:0cm;line-height:normal'><span   style='font-size:12.0pt'>{item.Count}</span></p>   </td>   <td width=149 valign= top style='width:111.95pt;border-top:none;border-left:   none;border-bottom:solid #B4C6E7 1.0pt;border-right:solid #B4C6E7 1.0pt;   padding:0cm 5.4pt 0cm 5.4pt'>   <p class= MsoNormal style='margin-bottom:0cm;line-height:normal'><span   style='font-size:12.0pt'>{price}</span></p>   </td>  </tr>  ";
            }
            decimal cost = 0;
                var contentCost= order.Contents.Sum(x => x.Count * x.Accessory.Price);
                var waterCost = order.WaterProducts.Sum(x =>
                {
                    var v = x.Volume;
                    if (v < 1)
                    {
                        v = Math.Abs(1 - x.Volume);
                    }
                    return v * (decimal)(x.WaterType.AddToCost + x.PackageType.AddToCost) * x.Count;
                });
                cost = contentCost + waterCost;
            return _template.Replace("CONTENTS", content).Replace("N", cost.ToString());
        }

        [HttpGet]
        [Authorize]
        [Route("/{orderNum}")]
        public async Task<ActionResult> GetReceipt(int orderNum)
{
            _template = _resources.ReadResource("static/Квитанция.htm").Result;

            var order = (await _orderViewer.GetOrders(x => x.Id == orderNum)).FirstOrDefault();

            if (order is null)
            {
                throw new EntityNotFoundException();
            }

            var status = order.OrderStatusHistories?.OrderByDescending(x => x.Date).First().Status.Id;
            if (status !=4 && status != 5)
            {
                throw new Exception("Квитанции генерируются лишь при выдаче или доставке заказа.");
            }

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Квитанция №"+orderNum,
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = await GetTemplate(order),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var receipt = _converter.Convert(pdf);

            HttpContext.Response.Headers.ContentEncoding = "windows-1251";
            HttpContext.Response.Headers.Add("Content-Disposition", "inline");

            return File(receipt, "application/pdf");
        }
        
        [HttpGet]
        [Authorize(Roles =nameof(AccessRole.Manager))]
        [Route("/products")]
        public async Task<ActionResult> GetProducts()
{
            _template = _resources.ReadResource("static/Квитанция.htm").Result;

            var accessories = await _catalogueViewer.GetAllByFilter(null, null, null);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Каталог " + DateTime.Now.ToShortDateString(),
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = await GetTemplate(accessories),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var receipt = _converter.Convert(pdf);

            HttpContext.Response.Headers.ContentEncoding = "windows-1251";
            HttpContext.Response.Headers.Add("Content-Disposition", "inline");

            return File(receipt, "application/pdf");
        }

        private async Task<string> GetTemplate(List<AccessoryInfo> accessoryInfos)
        {
            var html = "<table><tr><td>Артикул</td><td>Название</td><td>Цена</td></tr>";
            foreach (var item in accessoryInfos)
            {
                html += $"<tr><td>{item.Article}</td><td>{item.Title}</td><td>{item.Price}</td></tr> ";
            }
            return html+ "</table>";
        }
    }
}
