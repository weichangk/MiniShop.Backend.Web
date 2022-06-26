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
    public interface IPromotionDiscountItemApi : IHttpApi
    {
        [HttpGet("/api/PromotionDiscountItem/GetByIdAsync")]
        ITask<ResultModel<PromotionDiscountItemDto>> GetByIdAsync(int id);

        [HttpGet("/api/PromotionDiscountItem/GetListAllByShopIdPromotionOderIdAsync")]
        ITask<ResultModel<List<PromotionDiscountItemDto>>> GetListAllByShopIdPromotionOderIdAsync(Guid shopId, int promotionOderId, bool isDescending = false);
        
        [HttpGet("/api/PromotionDiscountItem/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PromotionDiscountItemDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PromotionDiscountItem/GetPageByShopIdPromotionOderIdAsync")]
        ITask<ResultModel<PagedList<PromotionDiscountItemDto>>> GetPageByShopIdPromotionOderIdAsync(int pageIndex, int pageSize, Guid shopId, int promotionOderId);

        [HttpDelete("/api/PromotionDiscountItem/DeleteAsync")]
        ITask<ResultModel<PromotionDiscountItemDto>> DeleteAsync(int id);

        [HttpDelete("/api/PromotionDiscountItem/BatchDeleteAsync")]
        ITask<ResultModel<PromotionDiscountItemDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PromotionDiscountItem/InsertAsync")]
        ITask<ResultModel<PromotionDiscountItemCreateDto>> InsertAsync([JsonContent] PromotionDiscountItemCreateDto model);

        [HttpPut("/api/PromotionDiscountItem/UpdateAsync")]
        ITask<ResultModel<PromotionDiscountItemUpdateDto>> UpdateAsync([JsonContent] PromotionDiscountItemUpdateDto model);

        [HttpPatch("/api/PromotionDiscountItem/PatchAsync")]
        ITask<ResultModel<PromotionDiscountItemDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PromotionDiscountItemUpdateDto> doc);    
    }
}
