using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MiniShop.Backend.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IShopApi _shopApi;
        private readonly IRenewPackageApi _renewPackageApi;
        public ShopController(ILogger<ShopController> logger, IMapper mapper, IUserInfo userInfo,
            IShopApi shopApi, IRenewPackageApi renewPackageApi) : base(logger, mapper, userInfo)
        {
            _shopApi = shopApi;
            _renewPackageApi = renewPackageApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await ExecuteApiResultModelAsync(() => { return _shopApi.GetByShopIdAsync(_userInfo.ShopId); });
            if (result.Success)
            {
                if (result.Data == null)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = (int)HttpStatusCode.NotFound, errorMsg = "商店不存在！" });
                }
                return View(result.Data);
            }
            return RedirectToAction("Error", "Error", new { statusCode = result.Status, errorMsg = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(ShopDto model)
        {
            var dto = _mapper.Map<ShopUpdateDto>(model);
            var result = await ExecuteApiAsync(() => { return _shopApi.UpdateAsync(dto); });
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> GetValidDateAsync()
        {
            var result = await ExecuteApiResultModelAsync(() => { return _shopApi.GetByShopIdAsync(_userInfo.ShopId); });
            if (result.Success)
            {
                if (result.Data == null)
                {
                    return Json(new Result() { Success = false, Msg = "商店不存在！", Status = (int)HttpStatusCode.NotFound });
                }
                return Json(new Result() { Success = result.Success, Data = result.Data.ValidDate.Format("yyyy-MM-dd") });
            }
            return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
        }

        [HttpGet]
        public async Task<IActionResult> Renew()
        {
            var result = await ExecuteApiResultModelAsync(() => { return _shopApi.GetByShopIdAsync(_userInfo.ShopId); });
            if (result.Success)
            {
                var shop = result.Data;
                if (shop == null)
                {
                    return Json(new Result() { Success = false, Msg = "商店不存在！", Status = (int)HttpStatusCode.NotFound });
                }
                ViewBag.ShopKey = shop.Id;
                ViewBag.ShopValidDate = shop.ValidDate;
                return View();
            }
            return RedirectToAction("Error", "Error", new { statusCode = result.Status, errorMsg = result.Status });
        }

        [HttpGet]
        public async Task<IActionResult> GetRenews()
        {
            var result = await ExecuteApiResultModelAsync(() => { return _renewPackageApi.GetRenewPackagesAsync(); });
            if (result.Success)
            {
                List<CardInfo> cards = new List<CardInfo>();
                foreach (var item in result.Data)
                {
                    CardInfo card = new CardInfo
                    {
                        Id = item.Id,
                        Title = $"￥{item.Price:F2}元 / {item.Name}",
                        Image = item.Image,
                        Remark = item.Remark,
                        Time = $"{item.Price:F2}元 / {item.Months}月",
                    };
                    cards.Add(card);
                }
                return Json(new Card() { Count = cards.Count, Data = cards });
            }
            return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
        }

        [HttpPatch]
        public async Task<IActionResult> RenewAsync(int id, DateTime validDate, int day)
        {
            var doc = new JsonPatchDocument<ShopUpdateDto>();
            doc.Replace(item => item.ValidDate, validDate.AddDays(day));
            var result = await ExecuteApiResultModelAsync(() => { return _shopApi.PatchAsync(id, doc); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
    }
}
