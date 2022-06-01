using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using Weick.Orm.Core;
using Weick.Orm.Core.Result;
using System;
using System.Collections.Generic;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IPurchaseReceiveOderApi : IHttpApi
    {
        [HttpGet("/api/PurchaseReceiveOder/GetByIdAsync")]
        ITask<ResultModel<PurchaseReceiveOderDto>> GetByIdAsync(int id);

        [HttpGet("/api/PurchaseReceiveOder/GetByShopIdOderNoAsync")]
        ITask<ResultModel<PurchaseReceiveOderDto>> GetByShopIdOderNoAsync(Guid shopId, string oderNo);

        [HttpGet("/api/PurchaseReceiveOder/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PurchaseReceiveOderDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);
        
        [HttpGet("/api/PurchaseReceiveOder/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PurchaseReceiveOderDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string oderNo);

        [HttpGet("/api/PurchaseReceiveOder/GetAuditedUnReturnedPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetAuditedUnReturnedPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PurchaseReceiveOder/GetAuditedUnReturnedPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetAuditedUnReturnedPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string oderNo);

        [HttpDelete("/api/PurchaseReceiveOder/DeleteAsync")]
        ITask<ResultModel<PurchaseReceiveOderDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseReceiveOder/BatchDeleteAsync")]
        ITask<ResultModel<PurchaseReceiveOderDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseReceiveOder/InsertAsync")]
        ITask<ResultModel<PurchaseReceiveOderCreateDto>> InsertAsync([JsonContent] PurchaseReceiveOderCreateDto model);

        [HttpPut("/api/PurchaseReceiveOder/UpdateAsync")]
        ITask<ResultModel<PurchaseReceiveOderUpdateDto>> UpdateAsync([JsonContent] PurchaseReceiveOderUpdateDto model);

        [HttpPatch("/api/PurchaseReceiveOder/PatchAsync")]
        ITask<ResultModel<PurchaseReceiveOderDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PurchaseReceiveOderUpdateDto> doc);

        [HttpPut("/api/PurchaseReceiveOder/UpdateReceiveOderAmountAsync")]
        ITask<ResultModel<decimal>> UpdateReceiveOderAmountAsync([FormContent]int id, [FormContent]decimal oderAmount);
    }
}
