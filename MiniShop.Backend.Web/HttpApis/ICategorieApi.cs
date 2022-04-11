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
        [HttpGet("/api/categorie")]
        ITask<ResultModel<CategorieDto>> GetByIdAsync(int id);

        [HttpGet("/api/categorie/GetByCodeOnShop")]
        ITask<ResultModel<CategorieDto>> GetByCodeOnShop(Guid shopId, int code);

        [HttpGet("/api/categorie/GetMaxCodeByLevelOnShop")]
        ITask<ResultModel<int>> GetMaxCodeByLevelOnShop(Guid shopId, int level);
        
        [HttpGet("/api/categorie/GetPageOnShop")]
        ITask<ResultModel<PagedList<CategorieDto>>> GetPageOnShopAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/categorie/GetPageOnShopWhereQueryCodeOrName")]
        ITask<ResultModel<PagedList<CategorieDto>>> GetPageOnShopWhereQueryCodeOrName(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/categorie")]
        ITask<ResultModel<CategorieDto>> DeleteAsync(int id);

        [HttpDelete("/api/categorie/BatchDelete")]
        ITask<ResultModel<CategorieDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/categorie")]
        ITask<ResultModel<CategorieCreateDto>> AddAsync([JsonContent] CategorieCreateDto model);

        [HttpPut("/api/categorie")]
        ITask<ResultModel<CategorieUpdateDto>> UpdateAsync([JsonContent] CategorieUpdateDto model);

        [HttpPatch("/api/categorie")]
        ITask<ResultModel<CategorieDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<CategorieUpdateDto> doc);
    }
}
