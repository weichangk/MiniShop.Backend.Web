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
    public interface IPurchaseReceiveOderItemApi : IHttpApi
    {
        [HttpGet("/api/PurchaseReceiveOderItem/GetByIdAsync")]
        ITask<ResultModel<PurchaseReceiveOderItemDto>> GetByIdAsync(int id);
        
        [HttpGet("/api/PurchaseReceiveOderItem/GetPageByShopIdPurchaseReceiveOderIdAsync")]
        ITask<ResultModel<PagedList<PurchaseReceiveOderItemDto>>> GetPageByShopIdPurchaseReceiveOderIdAsync(int pageIndex, int pageSize, Guid shopId, int purchaseReceiveOderId);

        [HttpDelete("/api/PurchaseReceiveOderItem/DeleteAsync")]
        ITask<ResultModel<PurchaseReceiveOderItemDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseReceiveOderItem/BatchDeleteAsync")]
        ITask<ResultModel<PurchaseReceiveOderItemDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseReceiveOderItem/InsertAsync")]
        ITask<ResultModel<PurchaseReceiveOderItemCreateDto>> InsertAsync([JsonContent] PurchaseReceiveOderItemCreateDto model);

        [HttpPut("/api/PurchaseReceiveOderItem/UpdateAsync")]
        ITask<ResultModel<PurchaseReceiveOderItemUpdateDto>> UpdateAsync([JsonContent] PurchaseReceiveOderItemUpdateDto model);

        [HttpPatch("/api/PurchaseReceiveOderItem/PatchAsync")]
        ITask<ResultModel<PurchaseReceiveOderItemDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PurchaseReceiveOderItemUpdateDto> doc);

        [HttpGet("/api/PurchaseReceiveOderItem/GetSumNumberByPurchaseReceiveOderIdAsync")]
        ITask<ResultModel<decimal>> GetSumNumberByPurchaseReceiveOderIdAsync(int purchaseOderId);
        
    }
}
