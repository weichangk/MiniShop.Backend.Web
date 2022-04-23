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
        [HttpGet("/api/unit/GetByIdAsync")]
        ITask<ResultModel<UnitDto>> GetByIdAsync(int id);

        [HttpGet("/api/unit/GetByShopIdCodeAsync")]
        ITask<ResultModel<CategorieDto>> GetByShopIdCodeAsync(Guid shopId, int code);

        [HttpGet("/api/unit/GetMaxCodeByShopIdAsync")]
        ITask<ResultModel<int>> GetMaxCodeByShopIdAsync(Guid shopId);
        
        [HttpGet("/api/unit/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<UnitDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/unit/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<UnitDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/unit/DeleteAsync")]
        ITask<ResultModel<UnitDto>> DeleteAsync(int id);

        [HttpDelete("/api/unit/BatchDeleteAsync")]
        ITask<ResultModel<UnitDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/unit/InsertAsync")]
        ITask<ResultModel<UnitCreateDto>> InsertAsync([JsonContent] UnitCreateDto model);

        [HttpPut("/api/unit/UpdateAsync")]
        ITask<ResultModel<UnitUpdateDto>> UpdateAsync([JsonContent] UnitUpdateDto model);

        [HttpPatch("/api/unit/PatchAsync")]
        ITask<ResultModel<UnitDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<UnitUpdateDto> doc);
    }
}
