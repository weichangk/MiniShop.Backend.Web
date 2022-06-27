using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniShop.Backend.Web.Controllers
{
    public class PromotionOderDiscountItemController : BaseController
    {
        private readonly IPromotionDiscountItemApi _promotionDiscountItemApi;

        public PromotionOderDiscountItemController(ILogger<PromotionOderDiscountItemController> logger, IMapper mapper, IUserInfo userInfo,
            IPromotionDiscountItemApi promotionDiscountItemApi) : base(logger, mapper, userInfo)
        {
            _promotionDiscountItemApi = promotionDiscountItemApi;
        }

        [HttpGet]
        public IActionResult Add()
        {
            PromotionDiscountItemCreateDto model = new PromotionDiscountItemCreateDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(PromotionDiscountItemCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionDiscountItemApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionDiscountItemApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PromotionDiscountItemDto model)
        {
            var dto = _mapper.Map<PromotionDiscountItemUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _promotionDiscountItemApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }


        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionDiscountItemApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        public async Task<IActionResult> GetPageByShopIdPromotionOderIdAsync(int page, int limit, int promotionOderId)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionDiscountItemApi.GetPageByShopIdPromotionOderIdAsync(page, limit, _userInfo.ShopId, promotionOderId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionDiscountItemApi.DeleteAsync(id); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string ids)
        {
            List<string> idsStrList = ids.Split(",").ToList();
            List<int> idsIntList = new List<int>();
            foreach (var id in idsStrList)
            {
                idsIntList.Add(int.Parse(id));
            }

            if (idsIntList.Count > 0)
            {
                var result = await ExecuteApiResultModelAsync(() => { return _promotionDiscountItemApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的商品", Status = (int)HttpStatusCode.NotFound });
            }

        }

    }
}
