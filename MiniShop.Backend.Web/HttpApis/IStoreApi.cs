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
        [HttpGet("/api/store/GetByIdAsync")]
        ITask<ResultModel<StoreDto>> GetByIdAsync(int id);

        [HttpGet("/api/store/GetByStoreIdAsync")]
        ITask<ResultModel<StoreDto>> GetByStoreIdAsync(Guid storeId);

        [HttpGet("/api/store/GetByShopIdAsync")]
        ITask<ResultModel<List<StoreDto>>> GetByShopIdAsync(Guid shopId);

        [HttpGet("/api/store/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<StoreDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/store/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<StoreDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name);

        [HttpDelete("/api/store/DeleteAsync")]
        ITask<ResultModel<StoreDto>> DeleteAsync(int id);

        [HttpDelete("/api/store/BatchDeleteAsync")]
        ITask<ResultModel<StoreDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/store/InsertAsync")]
        ITask<ResultModel<StoreCreateDto>> InsertAsync([JsonContent] StoreCreateDto model);

        [HttpPut("/api/store/UpdateAsync")]
        ITask<ResultModel<StoreUpdateDto>> UpdateAsync([JsonContent] StoreUpdateDto model);

        [HttpPatch("/api/store/PatchAsync")]
        ITask<ResultModel<StoreDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<StoreUpdateDto> doc);
    }
}
