using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using Weick.Orm.Core;
using Weick.Orm.Core.Result;
using System;
using System.Collections.Generic;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using MiniShop.Backend.Model.Enums;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IVipScoreSettingApi : IHttpApi
    {
        [HttpGet("/api/VipScoreSetting/GetByIdAsync")]
        ITask<ResultModel<VipScoreSettingDto>> GetByIdAsync(int id);

        [HttpGet("/api/VipScoreSetting/GetByShopIdVipTypeIdVipScoreWayAsync")]
        ITask<ResultModel<VipScoreSettingDto>> GetByShopIdVipTypeIdVipScoreWayAsync(Guid shopId, int vipTypeId, EnumVipScoreWay vipScoreWay);

        [HttpGet("/api/VipScoreSetting/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<VipScoreSettingDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpDelete("/api/VipScoreSetting/DeleteAsync")]
        ITask<ResultModel<VipScoreSettingDto>> DeleteAsync(int id);

        [HttpDelete("/api/VipScoreSetting/BatchDeleteAsync")]
        ITask<ResultModel<VipScoreSettingDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/VipScoreSetting/InsertAsync")]
        ITask<ResultModel<VipScoreSettingCreateDto>> InsertAsync([JsonContent] VipScoreSettingCreateDto model);

        [HttpPut("/api/VipScoreSetting/UpdateAsync")]
        ITask<ResultModel<VipScoreSettingUpdateDto>> UpdateAsync([JsonContent] VipScoreSettingUpdateDto model);

        [HttpPatch("/api/VipScoreSetting/PatchAsync")]
        ITask<ResultModel<VipScoreSettingDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<VipScoreSettingUpdateDto> doc);
    }
}
