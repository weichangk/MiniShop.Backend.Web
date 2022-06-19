using System.Security.Cryptography.X509Certificates;
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
    public class PurchaseOderController : BaseController
    {
        private readonly IPurchaseOderApi _purchaseOderApi;
        private readonly ISupplierApi _supplierApi;
        public PurchaseOderController(ILogger<PurchaseOderController> logger, IMapper mapper, IUserInfo userInfo,
            IPurchaseOderApi purchaseOderApi,
            ISupplierApi supplierApi) : base(logger, mapper, userInfo)
        {
            _purchaseOderApi = purchaseOderApi;
            _supplierApi = supplierApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SelectAuditedUnReceivedPurchaseOder()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SelectAuditedUnReturnedPurchaseOder()
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
        public async Task<IActionResult> GetByShopIdOderNoAsync(string oderNo)
        {
            var result =  await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetByShopIdOderNoAsync(_userInfo.ShopId, oderNo); });
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            PurchaseOderCreateDto model = await Task.FromResult(
                new PurchaseOderCreateDto
                {
                    ShopId = _userInfo.ShopId,
                    OderNo = Guid.NewGuid().ToString(),//生成唯一单号
                    OperatorName = _userInfo.UserName,
                }
            );
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(PurchaseOderCreateDto model)
        {
            var result =  ExecuteApiResultModelAsync(() => { return _purchaseOderApi.InsertAsync(model); }).Result;
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetByIdAsync(id); });
            if (result.Success)
            {
                // result.Data.AuditOperatorName = _userInfo.UserName;
                // result.Data.AuditTime = DateTime.Now;
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PurchaseOderDto model)
        {
            var dto = _mapper.Map<PurchaseOderUpdateDto>(model);
            dto.AuditOperatorName = _userInfo.UserName;
            dto.AuditTime = DateTime.Now;
            dto.AuditState = EnumAuditStatus.Audited;
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> AuditAsync(PurchaseOderDto model)
        {
            var dto = _mapper.Map<PurchaseOderAuditDto>(model);
            dto.AuditOperatorName = _userInfo.UserName;
            dto.AuditTime = DateTime.Now;
            dto.AuditState = EnumAuditStatus.Audited;
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.AuditAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        } 

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetAuditedUnReceivedPageByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetAuditedUnReceivedPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetAuditedUnReturnPageByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetAuditedUnReturnPageByShopIdAsync(page, limit, _userInfo.ShopId); });
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
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, storeId, oderNo); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetAuditedUnReceivedPageByShopIdWhereQueryAsync(int page, int limit, string oderNo)
        {
            oderNo = System.Web.HttpUtility.UrlEncode(oderNo);
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetAuditedUnReceivedPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, oderNo); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetAuditedUnReturnPageByShopIdWhereQueryAsync(int page, int limit, string oderNo)
        {
            oderNo = System.Web.HttpUtility.UrlEncode(oderNo);
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.GetAuditedUnReturnPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, oderNo); });
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

        [HttpPost]
        public async Task<IActionResult> UpdateOderAmountAsync([FromForm]int id, [FromForm]decimal oderAmount)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.UpdateOderAmountAsync(id, oderAmount); });
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePurchaseOderStatusAsync([FromForm]int id, [FromForm]EnumPurchaseOrderStatus state)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _purchaseOderApi.UpdatePurchaseOderStatusAsync(id, state); });
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }
    }
}
