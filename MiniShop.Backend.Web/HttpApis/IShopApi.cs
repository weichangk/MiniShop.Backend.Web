using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using System;
using Weick.Orm.Core.Result;
using Microsoft.AspNetCore.JsonPatch;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IShopApi : IHttpApi
    {
        [HttpGet("/api/Shop/GetByShopIdAsync")]
        ITask<ResultModel<ShopDto>> GetByShopIdAsync(Guid shopId);
 
        [HttpPost("/api/Shop/InsertAsync")]
        ITask<ResultModel<ShopCreateDto>> InsertAsync([JsonContent] ShopCreateDto model);

        [HttpPut("/api/Shop/UpdateAsync")]
        ITask<ResultModel<ShopUpdateDto>> UpdateAsync([JsonContent] ShopUpdateDto model);

        [HttpPatch("/api/Shop/PatchAsync")]
        ITask<ResultModel<UserDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<ShopUpdateDto> doc);
    }
}
