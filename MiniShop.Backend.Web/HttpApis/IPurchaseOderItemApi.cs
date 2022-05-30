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
    public interface IPurchaseOderItemApi : IHttpApi
    {
        [HttpGet("/api/PurchaseOderItem/GetByIdAsync")]
        ITask<ResultModel<PurchaseOderItemDto>> GetByIdAsync(int id);

        [HttpGet("/api/PurchaseOderItem/GetListAllByShopIdPurchaseOderIdAsync")]
        ITask<ResultModel<List<PurchaseOderItemDto>>> GetListAllByShopIdPurchaseOderIdAsync(Guid shopId, int purchaseOderId, bool isDescending = false);
        
        [HttpGet("/api/PurchaseOderItem/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PurchaseOderItemDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PurchaseOderItem/GetPageByShopIdPurchaseOderIdAsync")]
        ITask<ResultModel<PagedList<PurchaseOderItemDto>>> GetPageByShopIdPurchaseOderIdAsync(int pageIndex, int pageSize, Guid shopId, int purchaseOderId);

        [HttpDelete("/api/PurchaseOderItem/DeleteAsync")]
        ITask<ResultModel<PurchaseOderItemDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseOderItem/BatchDeleteAsync")]
        ITask<ResultModel<PurchaseOderItemDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseOderItem/InsertAsync")]
        ITask<ResultModel<PurchaseOderItemCreateDto>> InsertAsync([JsonContent] PurchaseOderItemCreateDto model);

        [HttpPut("/api/PurchaseOderItem/UpdateAsync")]
        ITask<ResultModel<PurchaseOderItemUpdateDto>> UpdateAsync([JsonContent] PurchaseOderItemUpdateDto model);

        [HttpPatch("/api/PurchaseOderItem/PatchAsync")]
        ITask<ResultModel<PurchaseOderItemDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PurchaseOderItemUpdateDto> doc);

        [HttpGet("/api/PurchaseOderItem/GetSumAmountByPurchaseOderIdAsync")]
        ITask<ResultModel<decimal>> GetSumAmountByPurchaseOderIdAsync(int purchaseOderId);
        
    }
}
