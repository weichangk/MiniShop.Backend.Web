using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Backend.Web.Code;
using Weick.Orm.Core;
using Weick.Orm.Core.Result;
using System;
using System.Collections.Generic;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using MiniShop.Backend.Model.Dto;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IItemApi : IHttpApi
    {
        [HttpGet("/api/item")]
        ITask<ResultModel<ItemDto>> GetByIdAsync(int id);

        [HttpGet("/api/item/GetByCodeOnShop")]
        ITask<ResultModel<ItemDto>> GetByCodeOnShop(Guid shopId, string code);
        
        [HttpGet("/api/item/GetPageOnShop")]
        ITask<ResultModel<PagedList<ItemDto>>> GetPageOnShopAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/item/GetPageOnShopWhereQuery")]
        ITask<ResultModel<PagedList<ItemDto>>> GetPageOnShopWhereQuery(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/item")]
        ITask<ResultModel<ItemDto>> DeleteAsync(int id);

        [HttpDelete("/api/item/BatchDelete")]
        ITask<ResultModel<ItemDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/item")]
        ITask<ResultModel<ItemCreateDto>> AddAsync([JsonContent] ItemCreateDto model);

        [HttpPut("/api/item")]
        ITask<ResultModel<ItemUpdateDto>> UpdateAsync([JsonContent] ItemUpdateDto model);

        [HttpPatch("/api/item")]
        ITask<ResultModel<ItemDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<ItemUpdateDto> doc);
    }
}
