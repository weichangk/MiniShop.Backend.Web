using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using System;
using Weick.Orm.Core.Result;
using Microsoft.AspNetCore.JsonPatch;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using Weick.Orm.Core;
using System.Collections.Generic;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IBillApi : IHttpApi
    {
        [HttpGet("/api/Bill/GetPayFlowBillInfoByShopIdAsync")]
        ITask<ResultModel<PagedList<PayFlowBillInfoDto>>> GetPayFlowBillInfoByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/Bill/GetSaleFlowBillInfoByShopIdAsync")]
        ITask<ResultModel<PagedList<SaleFlowBillInfoDto>>> GetSaleFlowBillInfoByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/Bill/GetPayFlowBillInfoByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PayFlowBillInfoDto>>> GetPayFlowBillInfoByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, DateTime? startTime, DateTime? endTime);
        
        [HttpGet("/api/Bill/GetSaleFlowBillInfoByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<SaleFlowBillInfoDto>>> GetSaleFlowBillInfoByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, DateTime? startTime, DateTime? endTime);
 
        [HttpPost("/api/Bill/InsertBillInfoAsync")]
        ITask<ResultModel<BillInfoCreateDto>> InsertBillInfoAsync([JsonContent] BillInfoCreateDto model);

        [HttpPost("/api/Bill/InsertPayFlowAsync")]
        ITask<ResultModel<PayFlowCreateDto>> InsertPayFlowAsync([JsonContent] PayFlowCreateDto model);

        [HttpPost("/api/Bill/InsertSaleFlowAsync")]
        ITask<ResultModel<SaleFlowCreateDto>> InsertSaleFlowAsync([JsonContent] SaleFlowCreateDto model);

    }
}
