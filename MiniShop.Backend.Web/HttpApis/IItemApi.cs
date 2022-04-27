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
        [HttpGet("/api/item/GetByIdAsync")]
        ITask<ResultModel<ItemDto>> GetByIdAsync(int id);

        [HttpGet("/api/item/GetByShopIdCodeAsync")]
        ITask<ResultModel<ItemDto>> GetByShopIdCodeAsync(Guid shopId, string code);
        
        [HttpGet("/api/item/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<ItemDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/item/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<ItemDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/item/DeleteAsync")]
        ITask<ResultModel<ItemDto>> DeleteAsync(int id);

        [HttpDelete("/api/item/BatchDeleteAsync")]
        ITask<ResultModel<ItemDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/item/InsertAsync")]
        ITask<ResultModel<ItemCreateDto>> InsertAsync([JsonContent] ItemCreateDto model);

        [HttpPut("/api/item/UpdateAsync")]
        ITask<ResultModel<ItemUpdateDto>> UpdateAsync([JsonContent] ItemUpdateDto model);

        [HttpPatch("/api/item/PatchAsync")]
        ITask<ResultModel<ItemDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<ItemUpdateDto> doc);

        [HttpPost("/api/item/UploadImgAsync")]
        ITask<ResultModel<dynamic>> UploadImgAsync(string key, [JsonContent] string base64);

        [HttpPost("/api/item/DeleteImgAsync")]
        ITask<ResultModel<dynamic>> DeleteImgAsync(string key);

        [HttpPost("/api/item/BatchDeleteImgAsync")]
        ITask<ResultModel<dynamic>> BatchDeleteImgAsync([JsonContent] List<string> keys);
    }
}
