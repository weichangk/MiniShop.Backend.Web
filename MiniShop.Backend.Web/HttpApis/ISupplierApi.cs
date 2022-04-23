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
        [HttpGet("/api/supplier/GetByIdAsync")]
        ITask<ResultModel<SupplierDto>> GetByIdAsync(int id);

        [HttpGet("/api/supplier/GetByShopIdCodeAsync")]
        ITask<ResultModel<CategorieDto>> GetByShopIdCodeAsync(Guid shopId, int code);

        [HttpGet("/api/supplier/GetByShopIdAsync")]
        ITask<ResultModel<List<CategorieDto>>> GetByShopIdAsync(Guid shopId);

        [HttpGet("/api/supplier/GetMaxCodeByShopIdAsync")]
        ITask<ResultModel<int>> GetMaxCodeByShopIdAsync(Guid shopId);
        
        [HttpGet("/api/supplier/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<SupplierDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/supplier/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<SupplierDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/supplier/DeleteAsync")]
        ITask<ResultModel<SupplierDto>> DeleteAsync(int id);

        [HttpDelete("/api/supplier/BatchDeleteAsync")]
        ITask<ResultModel<SupplierDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/supplier/InsertAsync")]
        ITask<ResultModel<SupplierCreateDto>> InsertAsync([JsonContent] SupplierCreateDto model);

        [HttpPut("/api/supplier/UpdateAsync")]
        ITask<ResultModel<SupplierUpdateDto>> UpdateAsync([JsonContent] SupplierUpdateDto model);

        [HttpPatch("/api/supplier/PatchAsync")]
        ITask<ResultModel<SupplierDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<SupplierUpdateDto> doc);
    }
}
