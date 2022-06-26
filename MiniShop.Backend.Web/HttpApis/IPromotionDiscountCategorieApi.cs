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
    public interface IPromotionDiscountCategorieApi : IHttpApi
    {
        [HttpGet("/api/PromotionDiscountCategorie/GetByIdAsync")]
        ITask<ResultModel<PromotionDiscountCategorieDto>> GetByIdAsync(int id);

        [HttpGet("/api/PromotionDiscountCategorie/GetListAllByShopIdPromotionOderIdAsync")]
        ITask<ResultModel<List<PromotionDiscountCategorieDto>>> GetListAllByShopIdPromotionOderIdAsync(Guid shopId, int promotionOderId, bool isDescending = false);
        
        [HttpGet("/api/PromotionDiscountCategorie/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PromotionDiscountCategorieDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PromotionDiscountCategorie/GetPageByShopIdPromotionOderIdAsync")]
        ITask<ResultModel<PagedList<PromotionDiscountCategorieDto>>> GetPageByShopIdPromotionOderIdAsync(int pageIndex, int pageSize, Guid shopId, int promotionOderId);

        [HttpDelete("/api/PromotionDiscountCategorie/DeleteAsync")]
        ITask<ResultModel<PromotionDiscountCategorieDto>> DeleteAsync(int id);

        [HttpDelete("/api/PromotionDiscountCategorie/BatchDeleteAsync")]
        ITask<ResultModel<PromotionDiscountCategorieDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PromotionDiscountCategorie/InsertAsync")]
        ITask<ResultModel<PromotionDiscountCategorieCreateDto>> InsertAsync([JsonContent] PromotionDiscountCategorieCreateDto model);

        [HttpPut("/api/PromotionDiscountCategorie/UpdateAsync")]
        ITask<ResultModel<PromotionDiscountCategorieUpdateDto>> UpdateAsync([JsonContent] PromotionDiscountCategorieUpdateDto model);

        [HttpPatch("/api/PromotionDiscountCategorie/PatchAsync")]
        ITask<ResultModel<PromotionDiscountCategorieDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PromotionDiscountCategorieUpdateDto> doc);    
    }
}
