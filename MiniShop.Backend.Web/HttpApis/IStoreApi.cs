using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using System;
using System.Collections.Generic;
using Weick.Orm.Core;
using Weick.Orm.Core.Result;
using WebApiClientCore.Attributes;
using WebApiClientCore;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IStoreApi : IHttpApi
    {
        [HttpGet("/api/store")]
        ITask<ResultModel<StoreDto>> GetByIdAsync(int id);

        [HttpGet("/api/store/GetByStoreId")]
        ITask<ResultModel<StoreDto>> GetByStoreIdAsync(Guid storeId);

        [HttpGet("/api/store/GetPageOnShop")]
        ITask<ResultModel<PagedList<StoreDto>>> GetPageOnShopAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/store/GetPageOnShopWhereQueryName")]
        ITask<ResultModel<PagedList<StoreDto>>> GetPageOnShopWhereQueryNameAsync(int pageIndex, int pageSize, Guid shopId, string name);

        [HttpGet("/api/store/GetByShopId")]
        ITask<ResultModel<List<StoreDto>>> GetByShopIdAsync(Guid shopId);

        [HttpDelete("/api/store")]
        ITask<ResultModel<StoreDto>> DeleteAsync(int id);

        [HttpDelete("/api/store/BatchDelete")]
        ITask<ResultModel<StoreDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/store")]
        ITask<ResultModel<StoreCreateDto>> AddAsync([JsonContent] StoreCreateDto model);

        [HttpPut("/api/store")]
        ITask<ResultModel<StoreUpdateDto>> UpdateAsync([JsonContent] StoreUpdateDto model);

        [HttpPatch("/api/store")]
        ITask<ResultModel<StoreDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<StoreUpdateDto> doc);
    }
}
