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
    public interface IPromotionOderApi : IHttpApi
    {
        [HttpGet("/api/PromotionOder/GetByIdAsync")]
        ITask<ResultModel<PromotionOderDto>> GetByIdAsync(int id);

        [HttpGet("/api/PromotionOder/GetByShopIdOderNoAsync")]
        ITask<ResultModel<PromotionOderDto>> GetByShopIdOderNoAsync(Guid shopId, string oderNo);
        
        [HttpGet("/api/PromotionOder/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PromotionOderDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PromotionOder/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PromotionOderDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, int storeId, string oderNo);

        [HttpDelete("/api/PromotionOder/DeleteAsync")]
        ITask<ResultModel<PromotionOderDto>> DeleteAsync(int id);

        [HttpDelete("/api/PromotionOder/BatchDeleteAsync")]
        ITask<ResultModel<PromotionOderDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PromotionOder/InsertAsync")]
        ITask<ResultModel<PromotionOderCreateDto>> InsertAsync([JsonContent] PromotionOderCreateDto model);

        [HttpPut("/api/PromotionOder/UpdateAsync")]
        ITask<ResultModel<PromotionOderUpdateDto>> UpdateAsync([JsonContent] PromotionOderUpdateDto model);
    }
}
