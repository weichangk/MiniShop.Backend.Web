using System;
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
using MiniShop.Backend.Model.Enums;
using Weick.Orm.Core.Result;

namespace MiniShop.Backend.Web.Controllers
{
    public class PurchaseReturnOderController : BaseController
    {
        private readonly IPurchaseReturnOderApi _purchaseReturnOderApi;
        private readonly IPurchaseReturnOderItemApi _purchaseReturnOderItemApi;
        private readonly IPurchaseOderItemApi _purchaseOderItemApi;
        private readonly ISupplierApi _supplierApi;
        public PurchaseReturnOderController(ILogger<PurchaseReturnOderController> logger, IMapper mapper, IUserInfo userInfo,
            IPurchaseReturnOderApi purchaseReturnOderApi,
            ISupplierApi supplierApi,
            IPurchaseOderItemApi purchaseOderItemApi,
            IPurchaseReturnOderItemApi purchaseReturnOderItemApi) : base(logger, mapper, userInfo)
        {
            _purchaseReturnOderApi = purchaseReturnOderApi;
            _supplierApi = supplierApi;
            _purchaseOderItemApi = purchaseOderItemApi;
            _purchaseReturnOderItemApi = purchaseReturnOderItemApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliersByCurrentShopAsync()
        {
            var result = await ExecuteApiResultModelAsync(() => { return _supplierApi.GetByShopIdAsync(_userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            if (result.Data != null)
            {
                List<dynamic> supplierSelect = new List<dynamic>();
                foreach (var item in result.Data)
                {
                    var op = new { opValue = item.Id, opName = item.Name };
                    supplierSelect.Add(op);
                }
                return Json(new Result() { Success = true, Data = supplierSelect });
            }
            return Json(new Result() { Success = false });
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.OderNo = Guid.NewGuid().ToString();//生成唯一单号
            ViewBag.OperatorName = _userInfo.UserName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(PurchaseReturnOderCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderApi.InsertAsync(model); });
            if(result.Success)
            {
                var purchaseOderItemListResult =  ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.GetListAllByShopIdPurchaseOderIdAsync(model.ShopId, model.PurchaseOderId); }).Result;
                if(purchaseOderItemListResult.Success)
                {
                    foreach (var item in purchaseOderItemListResult.Data)
                    {
                        PurchaseReturnOderItemCreateDto purchaseReturnOderItemCreateDto = new PurchaseReturnOderItemCreateDto
                        {
                            ShopId = item.ShopId,
                            ItemId = item.ItemId,
                            PurchaseReturnOderId = result.Data.Id,
                            Number = item.Number,
                            GiveNumber = item.GiveNumber,
                            Amount = item.Amount,
                            RealPurchasePrice = item.RealPurchasePrice,
                        };
                        var result1 = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderItemApi.InsertAsync(purchaseReturnOderItemCreateDto); });
                        if(!result1.Success)
                        {
                            return Json(new Result() { Success = result1.Success, Msg = result1.Msg, Status = result1.Status });
                        }
                    }
                }
            }
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PurchaseReturnOderDto model)
        {
            var dto = _mapper.Map<PurchaseReturnOderUpdateDto>(model);
            dto.AuditOperatorName = _userInfo.UserName;
            dto.AuditTime = DateTime.Now;
            dto.AuditState = EnumAuditStatus.Audited;
            dto.OrderState = EnumPurchaseOrderStatus.Returned;
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopWhereQueryAsync(int page, int limit, string oderNo)
        {
            oderNo = System.Web.HttpUtility.UrlEncode(oderNo);
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, oderNo); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的采购退货单", Status = (int)HttpStatusCode.NotFound });
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateReturnOderAmountAsync([FromForm]int id, [FromForm]decimal oderAmount)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReturnOderApi.UpdateReturnOderAmountAsync(id, oderAmount); });
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> SubStockNumber([FromForm]int id)
        {
            var getPurchaseReturnOderItem = (ResultModel<List<PurchaseReturnOderItemDto>>)(await _purchaseReturnOderItemApi.GetListAllByShopIdPurchaseReturnOderIdAsync(_userInfo.ShopId, id));
            if(getPurchaseReturnOderItem.Success)
            {
                foreach(var item in getPurchaseReturnOderItem.Data)
                {
                    var addStockNumber =  await _purchaseReturnOderItemApi.AddOrSubStockNumberAsync(item.ShopId, item.ItemId, false, item.Number);
                    if(!addStockNumber.Success)
                    {
                        return Json(new Result() { Success = addStockNumber.Success, Msg = addStockNumber.Msg, Status = addStockNumber.Status });
                    }
                }
                return Json(new Result() { Success = true, Msg = "Success",  Status = 200 });
            }
            return Json(new Result() { Success = getPurchaseReturnOderItem.Success, Msg = getPurchaseReturnOderItem.Msg, Status = getPurchaseReturnOderItem.Status });
        }
    }
}
