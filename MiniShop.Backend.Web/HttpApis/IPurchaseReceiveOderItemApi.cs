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
        
        [HttpGet("/api/PurchaseReceiveOderItem/GetListAllByShopIdPurchaseReceiveOderIdAsync")]
        ITask<ResultModel<List<PurchaseOderItemDto>>> GetListAllByShopIdPurchaseReceiveOderIdAsync(Guid shopId, int purchaseReceiveOderId, bool isDescending = false);

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

        [HttpGet("/api/PurchaseReceiveOderItem/GetSumAmountByPurchaseReceiveOderIdAsync")]
        ITask<ResultModel<decimal>> GetSumAmountByPurchaseReceiveOderIdAsync(int purchaseReceiveOderId);

        [HttpGet("/api/PurchaseReceiveOderItem/GetListAllByShopIdPurchaseReceiveOderIdAsync")]
        ITask<ResultModel<List<PurchaseReceiveOderItemDto>>> GetListAllByShopIdPurchaseReceiveOderIdAsync(Guid shopId, int purchaseReceiveOderId);     

        [HttpPut("/api/Stock/AddOrSubStockNumberAsync")]
        ITask<ResultModel<decimal>> AddOrSubStockNumberAsync([FormContent] Guid shopId, [FormContent] int itemId, [FormContent] bool isAdd, [FormContent] decimal number);   
    }
}
