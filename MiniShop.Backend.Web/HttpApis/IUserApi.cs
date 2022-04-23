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
        [HttpGet("/api/User/GetByNameAsync")]
        ITask<ResultModel<UserDto>> GetByNameAsync(string name);

        [HttpGet("/api/User/GetPageByShopIdRankAsync")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdRankAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank);

        [HttpGet("/api/User/GetPageByShopIdStoreIdRankAsync")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdStoreIdRankAsync(int pageIndex, int pageSize, Guid shopId, Guid storeId, EnumRole rank);

        [HttpGet("/api/User/GetPageByShopIdStoreIdRankWhereQueryAsync")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdStoreIdRankWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank, EnumRole? queryRank, string queryName, string queryPhone);

        [HttpGet("/api/User/GetPageByShopIdStoreIdRankWhereQueryWithStoreIdAsync")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdStoreIdRankWhereQueryWithStoreIdAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank, Guid? queryStore, EnumRole? queryRank, string queryName, string queryPhone);

        [HttpDelete("/api/User/DeleteByRankNameAsync")]
        ITask<ResultModel<string>> DeleteByRankNameAsync(EnumRole rank, string name);

        [HttpDelete("/api/User/BatchDeleteByRankNamesAsync")]
        ITask<ResultModel<string>> BatchDeleteByRankNamesAsync(EnumRole rank, [JsonContent] List<string> names);

        [HttpPost("/api/User/InsertByRankAsync")]
        ITask<ResultModel<UserDto>> InsertByRankAsync(EnumRole rank, [JsonContent] UserCreateDto model);

        [HttpPut("/api/User/UpdateByRankAsync")]
        ITask<ResultModel<UserDto>> UpdateByRankAsync(EnumRole rank, [JsonContent] UserUpdateDto model);

        [HttpPatch("/api/User/PatchUpdateByRankNameAsync")]
        ITask<ResultModel<UserDto>> PatchUpdateByRankNameAsync(EnumRole rank, string name, JsonPatchDocument<UserUpdateDto> doc);
    }
}
