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
    public class PurchaseOderItemController : BaseController
    {
        private readonly IPurchaseOderItemApi _purchaseOderItemApi;
        public PurchaseOderItemController(ILogger<PurchaseOderItemController> logger, IMapper mapper, IUserInfo userInfo,
            IPurchaseOderItemApi purchaseOderItemApi) : base(logger, mapper, userInfo)
        {
            _purchaseOderItemApi = purchaseOderItemApi;
        }

        [HttpGet]
        public IActionResult Add()
        {
            PurchaseOderItemCreateDto model = new PurchaseOderItemCreateDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(PurchaseOderItemCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PurchaseOderItemDto model)
        {
            var dto = _mapper.Map<PurchaseOderItemUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }


        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        public async Task<IActionResult> GetPageByShopIdPurchaseOderIdAsync(int page, int limit, int purchaseOderId)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.GetPageByShopIdPurchaseOderIdAsync(page, limit, _userInfo.ShopId, purchaseOderId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的单位", Status = (int)HttpStatusCode.NotFound });
            }

        }

        public async Task<IActionResult> GetSumAmountByPurchaseOderIdAsync(int purchaseOderId)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.GetSumAmountByPurchaseOderIdAsync(purchaseOderId); });
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

    }
}
