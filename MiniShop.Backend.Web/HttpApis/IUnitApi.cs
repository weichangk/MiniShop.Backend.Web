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
    public interface IUnitApi : IHttpApi
    {
        [HttpGet("/api/unit")]
        ITask<ResultModel<UnitDto>> GetByIdAsync(int id);

        [HttpGet("/api/unit/GetByCodeOnShop")]
        ITask<ResultModel<CategorieDto>> GetByCodeOnShop(Guid shopId, int code);

        [HttpGet("/api/unit/GetMaxCodeByShopId")]
        ITask<ResultModel<int>> GetMaxCodeByShopId(Guid shopId);
        
        [HttpGet("/api/unit/GetPageOnShop")]
        ITask<ResultModel<PagedList<UnitDto>>> GetPageOnShopAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/unit/GetPageOnShopWhereQueryCodeOrName")]
        ITask<ResultModel<PagedList<UnitDto>>> GetPageOnShopWhereQueryCodeOrName(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/unit")]
        ITask<ResultModel<UnitDto>> DeleteAsync(int id);

        [HttpDelete("/api/unit/BatchDelete")]
        ITask<ResultModel<UnitDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/unit")]
        ITask<ResultModel<UnitCreateDto>> AddAsync([JsonContent] UnitCreateDto model);

        [HttpPut("/api/unit")]
        ITask<ResultModel<UnitUpdateDto>> UpdateAsync([JsonContent] UnitUpdateDto model);

        [HttpPatch("/api/unit")]
        ITask<ResultModel<UnitDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<UnitUpdateDto> doc);
    }
}
