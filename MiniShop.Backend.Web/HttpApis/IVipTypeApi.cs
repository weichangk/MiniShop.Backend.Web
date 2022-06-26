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
    public interface IVipTypeApi : IHttpApi
    {
        [HttpGet("/api/VipType/GetByIdAsync")]
        ITask<ResultModel<VipTypeDto>> GetByIdAsync(int id);

        [HttpGet("/api/VipType/GetByShopIdCodeAsync")]
        ITask<ResultModel<VipTypeDto>> GetByShopIdCodeAsync(Guid shopId, int code);

        [HttpGet("/api/VipType/GetMaxCodeByShopIdAsync")]
        ITask<ResultModel<int>> GetMaxCodeByShopIdAsync(Guid shopId);

        [HttpGet("/api/VipType/GetByShopIdAsync")]
        ITask<ResultModel<List<VipTypeDto>>> GetByShopIdAsync(Guid shopId);

        [HttpGet("/api/VipType/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<VipTypeDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/VipType/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<VipTypeDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/VipType/DeleteAsync")]
        ITask<ResultModel<VipTypeDto>> DeleteAsync(int id);

        [HttpDelete("/api/VipType/BatchDeleteAsync")]
        ITask<ResultModel<VipTypeDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/VipType/InsertAsync")]
        ITask<ResultModel<VipTypeCreateDto>> InsertAsync([JsonContent] VipTypeCreateDto model);

        [HttpPut("/api/VipType/UpdateAsync")]
        ITask<ResultModel<VipTypeUpdateDto>> UpdateAsync([JsonContent] VipTypeUpdateDto model);

        [HttpPatch("/api/VipType/PatchAsync")]
        ITask<ResultModel<VipTypeDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<VipTypeUpdateDto> doc);
    }
}
