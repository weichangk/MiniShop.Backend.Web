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
    public interface IPromotionSpecialOfferItemApi : IHttpApi
    {
        [HttpGet("/api/PromotionSpecialOfferItem/GetByIdAsync")]
        ITask<ResultModel<PromotionSpecialOfferItemDto>> GetByIdAsync(int id);

        [HttpGet("/api/PromotionSpecialOfferItem/GetListAllByShopIdPromotionOderIdAsync")]
        ITask<ResultModel<List<PromotionSpecialOfferItemDto>>> GetListAllByShopIdPromotionOderIdAsync(Guid shopId, int promotionOderId, bool isDescending = false);
        
        [HttpGet("/api/PromotionSpecialOfferItem/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PromotionSpecialOfferItemDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PromotionSpecialOfferItem/GetPageByShopIdPromotionOderIdAsync")]
        ITask<ResultModel<PagedList<PromotionSpecialOfferItemDto>>> GetPageByShopIdPromotionOderIdAsync(int pageIndex, int pageSize, Guid shopId, int promotionOderId);

        [HttpDelete("/api/PromotionSpecialOfferItem/DeleteAsync")]
        ITask<ResultModel<PromotionSpecialOfferItemDto>> DeleteAsync(int id);

        [HttpDelete("/api/PromotionSpecialOfferItem/BatchDeleteAsync")]
        ITask<ResultModel<PromotionSpecialOfferItemDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PromotionSpecialOfferItem/InsertAsync")]
        ITask<ResultModel<PromotionSpecialOfferItemCreateDto>> InsertAsync([JsonContent] PromotionSpecialOfferItemCreateDto model);

        [HttpPut("/api/PromotionSpecialOfferItem/UpdateAsync")]
        ITask<ResultModel<PromotionSpecialOfferItemUpdateDto>> UpdateAsync([JsonContent] PromotionSpecialOfferItemUpdateDto model);

        [HttpPatch("/api/PromotionSpecialOfferItem/PatchAsync")]
        ITask<ResultModel<PromotionSpecialOfferItemDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PromotionSpecialOfferItemUpdateDto> doc);    
    }
}
