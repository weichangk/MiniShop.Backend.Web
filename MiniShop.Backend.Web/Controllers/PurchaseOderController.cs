using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniShop.Backend.Web.Controllers
{
    public class PurchaseOderController : BaseController
    {
        private readonly IPurchaseOderApi _purchaseOderApi;
        public PurchaseOderController(ILogger<PurchaseOderController> logger, IMapper mapper, IUserInfo userInfo,
            IPurchaseOderApi purchaseOderApi) : base(logger, mapper, userInfo)
        {
            _purchaseOderApi = purchaseOderApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            PurchaseOderCreateDto model = new PurchaseOderCreateDto
            {
                //ShopId = _userInfo.ShopId,
                //Code = maxCode,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(PurchaseOderCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.AddAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PurchaseOderDto model)
        {
            var dto = _mapper.Map<PurchaseOderUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetPageOnShopAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopWhereQueryAsync(int page, int limit, int storeId, string oderNo)
        {
            oderNo = System.Web.HttpUtility.UrlEncode(oderNo);
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetPageOnShopWhereQuery(page, limit, _userInfo.ShopId, storeId, oderNo); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.DeleteAsync(id); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string ids)
        {
            List<string> idsStrList = ids.Split(",").ToList();
            List<int> idsIntList = new List<int>();
            foreach (var id in idsStrList)
            {
                idsIntList.Add(int.Parse(id));
            }

            if (idsIntList.Count > 0)
            {
                var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的单位", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
