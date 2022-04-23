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
        [HttpGet("/api/PurchaseOderItem")]
        ITask<ResultModel<PurchaseOderItemDto>> GetByIdAsync(int id);

        [HttpGet("/api/PurchaseOderItem/GetByOderNoOnShop")]
        ITask<ResultModel<PurchaseOderItemDto>> GetByOderNoOnShop(Guid shopId, string oderNo);
        
        [HttpGet("/api/PurchaseOderItem/GetPageOnShop")]
        ITask<ResultModel<PagedList<PurchaseOderItemDto>>> GetPageOnShopAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PurchaseOderItem/GetPageByShopIdOderNoAsync")]
        ITask<ResultModel<PagedList<PurchaseOderItemDto>>> GetPageByShopIdOderNoAsync(int pageIndex, int pageSize, Guid shopId, string oderNo);

        [HttpGet("/api/PurchaseOderItem/GetPageOnShopWhereQuery")]
        ITask<ResultModel<PagedList<PurchaseOderItemDto>>> GetPageOnShopWhereQuery(int pageIndex, int pageSize, Guid shopId, string oderNo);

        [HttpDelete("/api/PurchaseOderItem")]
        ITask<ResultModel<PurchaseOderItemDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseOderItem/BatchDelete")]
        ITask<ResultModel<PurchaseOderItemDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseOderItem")]
        ITask<ResultModel<PurchaseOderItemCreateDto>> AddAsync([JsonContent] PurchaseOderItemCreateDto model);

        [HttpPut("/api/PurchaseOderItem")]
        ITask<ResultModel<PurchaseOderItemUpdateDto>> UpdateAsync([JsonContent] PurchaseOderItemUpdateDto model);

        [HttpPatch("/api/PurchaseOderItem")]
        ITask<ResultModel<PurchaseOderItemDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<PurchaseOderItemUpdateDto> doc);
    }
}
