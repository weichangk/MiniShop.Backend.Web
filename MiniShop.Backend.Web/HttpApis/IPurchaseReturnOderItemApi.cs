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
    public interface IPurchaseReturnOderItemApi : IHttpApi
    {
        [HttpGet("/api/PurchaseReturnOderItem/GetByIdAsync")]
        ITask<ResultModel<PurchaseReturnOderItemDto>> GetByIdAsync(int id);
        
        [HttpGet("/api/PurchaseReturnOderItem/GetPageByShopIdPurchaseReturnOderIdAsync")]
        ITask<ResultModel<PagedList<PurchaseReturnOderItemDto>>> GetPageByShopIdPurchaseReturnOderIdAsync(int pageIndex, int pageSize, Guid shopId, int purchaseReturnOderId);

        [HttpDelete("/api/PurchaseReturnOderItem/DeleteAsync")]
        ITask<ResultModel<PurchaseReturnOderItemDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseReturnOderItem/BatchDeleteAsync")]
        ITask<ResultModel<PurchaseReturnOderItemDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseReturnOderItem/InsertAsync")]
        ITask<ResultModel<PurchaseReturnOderItemCreateDto>> InsertAsync([JsonContent] PurchaseReturnOderItemCreateDto model);

        [HttpPut("/api/PurchaseReturnOderItem/UpdateAsync")]
        ITask<ResultModel<PurchaseReturnOderItemUpdateDto>> UpdateAsync([JsonContent] PurchaseReturnOderItemUpdateDto model);

        [HttpPatch("/api/PurchaseReturnOderItem/PatchAsync")]
        ITask<ResultModel<PurchaseReturnOderItemDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PurchaseReturnOderItemUpdateDto> doc);

        [HttpGet("/api/PurchaseReturnOderItem/GetSumAmountByPurchaseReturnOderIdAsync")]
        ITask<ResultModel<decimal>> GetSumAmountByPurchaseReturnOderIdAsync(int purchaseReturnOderId);

        [HttpGet("/api/PurchaseReturnOderItem/GetListAllByShopIdPurchaseReturnOderIdAsync")]
        ITask<ResultModel<List<PurchaseReturnOderItemDto>>> GetListAllByShopIdPurchaseReturnOderIdAsync(Guid shopId, int purchaseReturnOderId);     

        [HttpPut("/api/Stock/AddOrSubStockNumberAsync")]
        ITask<ResultModel<decimal>> AddOrSubStockNumberAsync([FormContent] Guid shopId, [FormContent] int itemId, [FormContent] bool isAdd, [FormContent] decimal number);      
    }
}
