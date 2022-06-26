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
    public interface IVipApi : IHttpApi
    {
        [HttpGet("/api/Vip/GetByIdAsync")]
        ITask<ResultModel<VipDto>> GetByIdAsync(int id);

        [HttpGet("/api/Vip/GetByShopIdCodeAsync")]
        ITask<ResultModel<VipDto>> GetByShopIdCodeAsync(Guid shopId, string code);

        [HttpGet("/api/Vip/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<VipDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/Vip/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<VipDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name, string phone);

        [HttpDelete("/api/Vip/DeleteAsync")]
        ITask<ResultModel<VipDto>> DeleteAsync(int id);

        [HttpDelete("/api/Vip/BatchDeleteAsync")]
        ITask<ResultModel<VipDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/Vip/InsertAsync")]
        ITask<ResultModel<VipCreateDto>> InsertAsync([JsonContent] VipCreateDto model);

        [HttpPut("/api/Vip/UpdateAsync")]
        ITask<ResultModel<VipUpdateDto>> UpdateAsync([JsonContent] VipUpdateDto model);

        [HttpPatch("/api/Vip/PatchAsync")]
        ITask<ResultModel<VipDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<VipUpdateDto> doc);
    }
}
