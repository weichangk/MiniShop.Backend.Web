using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using System;
using Weick.Orm.Core.Result;
using Microsoft.AspNetCore.JsonPatch;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using Weick.Orm.Core;
using System.Collections.Generic;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IStockApi : IHttpApi
    {
        [HttpGet("/api/Stock/GetByIdAsync")]
        ITask<ResultModel<StockDto>> GetByIdAsync(int id);

        [HttpGet("/api/Stock/GetByShopIdAndItemIdAsync")]
        ITask<ResultModel<StockDto>> GetByShopIdAndItemIdAsync(Guid shopId, int itemId);

        [HttpGet("/api/Stock/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<StockDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/Stock/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<StockDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);
        
        [HttpPost("/api/Stock/InsertAsync")]
        ITask<ResultModel<StockCreateDto>> InsertAsync([JsonContent] StockCreateDto model);

        [HttpPut("/api/Stock/UpdateAsync")]
        ITask<ResultModel<StockUpdateDto>> UpdateAsync([JsonContent] StockUpdateDto model);

        [HttpPatch("/api/Stock/PatchAsync")]
        ITask<ResultModel<StockDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<StockUpdateDto> doc);

        [HttpDelete("/api/Stock/DeleteAsync")]
        ITask<ResultModel<StockDto>> DeleteAsync(int id);

        [HttpDelete("/api/Stock/BatchDeleteAsync")]
        ITask<ResultModel<StoreDto>> BatchDeleteAsync([JsonContent] List<int> ids);
    }
}
