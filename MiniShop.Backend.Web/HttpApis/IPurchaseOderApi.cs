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
    public interface IPurchaseOderApi : IHttpApi
    {
        [HttpGet("/api/PurchaseOder")]
        ITask<ResultModel<PurchaseOderDto>> GetByIdAsync(int id);

        [HttpGet("/api/PurchaseOder/GetByOderNoOnShop")]
        ITask<ResultModel<CategorieDto>> GetByOderNoOnShop(Guid shopId, string oderNo);
        
        [HttpGet("/api/PurchaseOder/GetPageOnShop")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetPageOnShopAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PurchaseOder/GetPageOnShopWhereQuery")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetPageOnShopWhereQuery(int pageIndex, int pageSize, Guid shopId, int storeId, string oderNo);

        [HttpDelete("/api/PurchaseOder")]
        ITask<ResultModel<PurchaseOderDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseOder/BatchDelete")]
        ITask<ResultModel<PurchaseOderDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseOder")]
        ITask<ResultModel<PurchaseOderCreateDto>> AddAsync([JsonContent] PurchaseOderCreateDto model);

        [HttpPut("/api/PurchaseOder")]
        ITask<ResultModel<PurchaseOderUpdateDto>> UpdateAsync([JsonContent] PurchaseOderUpdateDto model);

        [HttpPatch("/api/PurchaseOder")]
        ITask<ResultModel<PurchaseOderDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<PurchaseOderUpdateDto> doc);
    }
}
