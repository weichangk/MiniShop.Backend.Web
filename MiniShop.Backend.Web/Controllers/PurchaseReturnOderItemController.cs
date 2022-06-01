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
    public class PurchaseReturnOderItemController : BaseController
    {
        private readonly IPurchaseReturnOderItemApi _purchaseReturnOderItemApi;
        public PurchaseReturnOderItemController(ILogger<PurchaseReturnOderItemController> logger, IMapper mapper, IUserInfo userInfo,
            IPurchaseReturnOderItemApi purchaseReturnOderItemApi) : base(logger, mapper, userInfo)
        {
            _purchaseReturnOderItemApi = purchaseReturnOderItemApi;
        }

        [HttpGet]
        public IActionResult Add()
        {
            PurchaseReturnOderItemCreateDto model = new PurchaseReturnOderItemCreateDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(PurchaseReturnOderItemCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderItemApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderItemApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PurchaseReturnOderItemDto model)
        {
            var dto = _mapper.Map<PurchaseReturnOderItemUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderItemApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> GetPageByShopIdPurchaseReturnOderIdAsync(int page, int limit, int purchaseReturnOderId)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderItemApi.GetPageByShopIdPurchaseReturnOderIdAsync(page, limit, _userInfo.ShopId, purchaseReturnOderId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderItemApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderItemApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的采购退货商品", Status = (int)HttpStatusCode.NotFound });
            }

        }

        public async Task<IActionResult> GetSumAmountByPurchaseReturnOderIdAsync(int purchaseReturnOderId)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderItemApi.GetSumAmountByPurchaseReturnOderIdAsync(purchaseReturnOderId); });
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

    }
}
