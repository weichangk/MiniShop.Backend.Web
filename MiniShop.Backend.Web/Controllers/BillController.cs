using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MiniShop.Backend.Web.Code;
using Microsoft.AspNetCore.JsonPatch;

namespace MiniShop.Backend.Web.Controllers
{
    public class BillController : BaseController
    {
        private readonly IBillApi _billApi;
        public BillController(ILogger<BillController> logger, IMapper mapper, IUserInfo userInfo,
            IBillApi billApi) : base(logger, mapper, userInfo)
        {
            _billApi = billApi;
        }

        [HttpGet]
        public IActionResult PayFlowIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SaleFlowIndex()
        {
            return View();
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPayFlowBillInfoByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _billApi.GetPayFlowBillInfoByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPayFlowBillInfoByShopIdWhereQueryAsync(int page, int limit, DateTime? startTime, DateTime? endTime)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _billApi.GetPayFlowBillInfoByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, startTime, endTime); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetSaleFlowBillInfoByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _billApi.GetSaleFlowBillInfoByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetSaleFlowBillInfoByShopIdWhereQueryAsync(int page, int limit, DateTime? startTime, DateTime? endTime)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _billApi.GetSaleFlowBillInfoByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, startTime, endTime); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }
    }
}
