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

namespace MiniShop.Backend.Web.Controllers
{
    public class PurchaseReceiveOderController : BaseController
    {
        private readonly IPurchaseReceiveOderApi _purchaseReceiveOderApi;
        private readonly IPurchaseOderItemApi _purchaseOderItemApi;
        private readonly ISupplierApi _supplierApi;
        public PurchaseReceiveOderController(ILogger<PurchaseReceiveOderController> logger, IMapper mapper, IUserInfo userInfo,
            IPurchaseReceiveOderApi purchaseReceiveOderApi,
            ISupplierApi supplierApi,
            IPurchaseOderItemApi purchaseOderItemApi) : base(logger, mapper, userInfo)
        {
            _purchaseReceiveOderApi = purchaseReceiveOderApi;
            _supplierApi = supplierApi;
            _purchaseOderItemApi = purchaseOderItemApi;
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
            // PurchaseReceiveOderCreateDto model = await Task.FromResult(
            //     new PurchaseReceiveOderCreateDto
            //     {
            //         ShopId = _userInfo.ShopId,
            //         OderNo = Guid.NewGuid().ToString(),//生成唯一单号
            //         OperatorName = _userInfo.UserName,
            //     }
            // );
            // return View(model);
            ViewBag.OderNo = Guid.NewGuid().ToString();//生成唯一单号
            ViewBag.OperatorName = _userInfo.UserName;
            return View();
        }

        [HttpPost]
        public IActionResult Add(PurchaseReceiveOderCreateDto model)
        {
            var result =  ExecuteApiResultModelAsync(() => { return _purchaseReceiveOderApi.InsertAsync(model); }).Result;
            if(result.Success)
            {
                var purchaseOderItemListResult =  ExecuteApiResultModelAsync(() => { return _purchaseOderItemApi.GetListAllByShopIdPurchaseOderIdAsync(model.ShopId, model.PurchaseOderId); }).Result;
                if(purchaseOderItemListResult.Success)
                {
                    foreach (var item in purchaseOderItemListResult.Data)
                    {
                        
                    }
                }
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            //return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReceiveOderApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PurchaseReceiveOderDto model)
        {
            var dto = _mapper.Map<PurchaseReceiveOderUpdateDto>(model);
            dto.AuditOperatorName = _userInfo.UserName;
            dto.AuditTime = DateTime.Now;
            dto.AuditState = EnumAuditStatus.Audited;
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReceiveOderApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReceiveOderApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
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
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReceiveOderApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, oderNo); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseReceiveOderApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _purchaseReceiveOderApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的采购退货单", Status = (int)HttpStatusCode.NotFound });
            }

        }

        // [HttpPost]
        // public IActionResult CreatePurchaseReceiveOder(PurchaseReceiveOderCreateDto model)
        // {
        //     var result =  ExecuteApiResultModelAsync(() => { return _purchaseReceiveOderApi.InsertAsync(model); }).Result;
        //     return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        // }

    }
}
