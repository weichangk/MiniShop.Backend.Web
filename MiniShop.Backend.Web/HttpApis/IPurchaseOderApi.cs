using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using Weick.Orm.Core;
using Weick.Orm.Core.Result;
using System;
using System.Collections.Generic;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using MiniShop.Backend.Model.Enums;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IPurchaseOderApi : IHttpApi
    {
        [HttpGet("/api/PurchaseOder/GetByIdAsync")]
        ITask<ResultModel<PurchaseOderDto>> GetByIdAsync(int id);

        [HttpGet("/api/PurchaseOder/GetByShopIdOderNoAsync")]
        ITask<ResultModel<PurchaseOderDto>> GetByShopIdOderNoAsync(Guid shopId, string oderNo);
        
        [HttpGet("/api/PurchaseOder/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PurchaseOder/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, int storeId, string oderNo);

        [HttpGet("/api/PurchaseOder/GetAuditedUnReceivedPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetAuditedUnReceivedPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PurchaseOder/GetAuditedUnReceivedPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetAuditedUnReceivedPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string oderNo);

        [HttpGet("/api/PurchaseOder/GetAuditedUnReturnPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetAuditedUnReturnPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PurchaseOder/GetAuditedUnReturnPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetAuditedUnReturnPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string oderNo);

        [HttpDelete("/api/PurchaseOder/DeleteAsync")]
        ITask<ResultModel<PurchaseOderDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseOder/BatchDeleteAsync")]
        ITask<ResultModel<PurchaseOderDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseOder/InsertAsync")]
        ITask<ResultModel<PurchaseOderCreateDto>> InsertAsync([JsonContent] PurchaseOderCreateDto model);

        [HttpPut("/api/PurchaseOder/UpdateAsync")]
        ITask<ResultModel<PurchaseOderUpdateDto>> UpdateAsync([JsonContent] PurchaseOderUpdateDto model);

        [HttpPut("/api/PurchaseOder/AuditAsync")]
        ITask<ResultModel<PurchaseOderUpdateDto>> AuditAsync([JsonContent] PurchaseOderAuditDto model);

        [HttpPatch("/api/PurchaseOder/PatchAsync")]
        ITask<ResultModel<PurchaseOderDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PurchaseOderUpdateDto> doc);

        [HttpPut("/api/PurchaseOder/UpdateOderAmountAsync")]
        ITask<ResultModel<decimal>> UpdateOderAmountAsync([FormContent]int id, [FormContent]decimal oderAmount);

        [HttpPut("/api/PurchaseOder/UpdatePurchaseOderStatusAsync")]
        ITask<ResultModel<bool>> UpdatePurchaseOderStatusAsync([FormContent]int id, [FormContent]EnumPurchaseOrderStatus state);
    }
}
