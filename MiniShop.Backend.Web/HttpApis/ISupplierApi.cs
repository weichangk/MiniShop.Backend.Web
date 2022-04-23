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
    public interface ISupplierApi : IHttpApi
    {
        [HttpGet("/api/supplier")]
        ITask<ResultModel<SupplierDto>> GetByIdAsync(int id);

        [HttpGet("/api/supplier/GetByCodeOnShop")]
        ITask<ResultModel<CategorieDto>> GetByCodeOnShop(Guid shopId, int code);

        [HttpGet("/api/supplier/GetByShopId")]
        ITask<ResultModel<List<CategorieDto>>> GetByShopIdAsync(Guid shopId);

        [HttpGet("/api/supplier/GetMaxCodeByShopId")]
        ITask<ResultModel<int>> GetMaxCodeByShopIdAsync(Guid shopId);
        
        [HttpGet("/api/supplier/GetPageByShopId")]
        ITask<ResultModel<PagedList<SupplierDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/supplier/GetPageByShopIdWhereQuery")]
        ITask<ResultModel<PagedList<SupplierDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/supplier")]
        ITask<ResultModel<SupplierDto>> DeleteAsync(int id);

        [HttpDelete("/api/supplier/BatchDelete")]
        ITask<ResultModel<SupplierDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/supplier")]
        ITask<ResultModel<SupplierCreateDto>> AddAsync([JsonContent] SupplierCreateDto model);

        [HttpPut("/api/supplier")]
        ITask<ResultModel<SupplierUpdateDto>> UpdateAsync([JsonContent] SupplierUpdateDto model);

        [HttpPatch("/api/supplier")]
        ITask<ResultModel<SupplierDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<SupplierUpdateDto> doc);
    }
}
