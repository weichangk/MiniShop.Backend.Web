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
    public interface ICategorieApi : IHttpApi
    {
        [HttpGet("/api/categorie/GetByIdAsync")]
        ITask<ResultModel<CategorieDto>> GetByIdAsync(int id);

        [HttpGet("/api/categorie/GetByShopIdCodeAsync")]
        ITask<ResultModel<CategorieDto>> GetByShopIdCodeAsync(Guid shopId, int code);

        [HttpGet("/api/categorie/GetMaxCodeByShopIdLevelAsync")]
        ITask<ResultModel<int>> GetMaxCodeByShopIdLevelAsync(Guid shopId, int level);
        
        [HttpGet("/api/categorie/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<CategorieDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/categorie/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<CategorieDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/categorie/DeleteAsync")]
        ITask<ResultModel<CategorieDto>> DeleteAsync(int id);

        [HttpDelete("/api/categorie/BatchDeleteAsync")]
        ITask<ResultModel<CategorieDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/categorie/InsertAsync")]
        ITask<ResultModel<CategorieCreateDto>> InsertAsync([JsonContent] CategorieCreateDto model);

        [HttpPut("/api/categorie/UpdateAsync")]
        ITask<ResultModel<CategorieUpdateDto>> UpdateAsync([JsonContent] CategorieUpdateDto model);

        [HttpPatch("/api/categorie/PatchAsync")]
        ITask<ResultModel<CategorieDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<CategorieUpdateDto> doc);
    }
}
