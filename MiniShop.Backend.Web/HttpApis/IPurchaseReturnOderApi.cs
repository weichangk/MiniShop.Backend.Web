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
    public interface IPurchaseReturnOderApi : IHttpApi
    {
        [HttpGet("/api/PurchaseReturnOder/GetByIdAsync")]
        ITask<ResultModel<PurchaseReturnOderDto>> GetByIdAsync(int id);

        [HttpGet("/api/PurchaseReturnOder/GetByShopIdOderNoAsync")]
        ITask<ResultModel<PurchaseReturnOderDto>> GetByShopIdOderNoAsync(Guid shopId, string oderNo);

        [HttpGet("/api/PurchaseReturnOder/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PurchaseReturnOderDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);
        
        [HttpGet("/api/PurchaseReturnOder/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PurchaseReturnOderDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string oderNo);

        [HttpDelete("/api/PurchaseReturnOder/DeleteAsync")]
        ITask<ResultModel<PurchaseReturnOderDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseReturnOder/BatchDeleteAsync")]
        ITask<ResultModel<PurchaseReturnOderDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseReturnOder/InsertAsync")]
        ITask<ResultModel<PurchaseReturnOderCreateDto>> InsertAsync([JsonContent] PurchaseReturnOderCreateDto model);

        [HttpPut("/api/PurchaseReturnOder/UpdateAsync")]
        ITask<ResultModel<PurchaseReturnOderUpdateDto>> UpdateAsync([JsonContent] PurchaseReturnOderUpdateDto model);

        [HttpPatch("/api/PurchaseReturnOder/PatchAsync")]
        ITask<ResultModel<PurchaseReturnOderDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PurchaseReturnOderUpdateDto> doc);

        [HttpPut("/api/PurchaseReturnOder/UpdateReturnOderAmountAsync")]
        ITask<ResultModel<decimal>> UpdateReturnOderAmountAsync([FormContent]int id, [FormContent]decimal oderAmount);
    }
}
