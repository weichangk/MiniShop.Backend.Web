using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Model.Enums;
using MiniShop.Backend.Web.Code;
using Weick.Orm.Core;
using Weick.Orm.Core.Result;
using System;
using System.Collections.Generic;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using WebApiClientCore.Parameters;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        [HttpGet("/api/User")]
        ITask<ResultModel<UserDto>> GetByNameAsync(string name);

        [HttpGet("/api/User/GetPageByRankOnShop")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByRankOnShopAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank);

        [HttpGet("/api/User/GetPageByRankOnStore")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByRankOnStoreAsync(int pageIndex, int pageSize, Guid shopId, Guid storeId, EnumRole rank);

        [HttpGet("/api/User/GetPageByRankOnShopWhereQueryRankOrNameOrPhone")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByRankOnShopWhereQueryRankOrNameOrPhoneAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank, EnumRole? queryRank, string queryName, string queryPhone);

        [HttpGet("/api/User/GetPageByRankOnShopWhereQueryStoreOrRankOrNameOrPhone")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByRankOnShopWhereQueryStoreOrRankOrNameOrPhoneAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank, Guid? queryStore, EnumRole? queryRank, string queryName, string queryPhone);

        [HttpDelete("/api/User")]
        ITask<ResultModel<string>> DeleteByNameAsync(EnumRole rank, string name);

        [HttpDelete("/api/User/BatchDelete")]
        ITask<ResultModel<string>> BatchDeleteByNamesAsync(EnumRole rank, [JsonContent] List<string> names);

        [HttpPost("/api/User")]
        ITask<ResultModel<UserDto>> AddAsync(EnumRole rank, [JsonContent] UserCreateDto model);

        [HttpPut("/api/User")]
        ITask<ResultModel<UserDto>> PutUpdateAsync(EnumRole rank, [JsonContent] UserUpdateDto model);

        [HttpPatch("/api/User")]
        ITask<ResultModel<UserDto>> PatchUpdateByNameAsync(EnumRole rank, string name, JsonPatchDocument<UserUpdateDto> doc);
    }
}
