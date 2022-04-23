using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Code;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Model.Enums;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniShop.Backend.Web.Controllers
{
    public class ItemController : BaseController
    {
        private readonly IItemApi _itemApi;
        public ItemController(ILogger<ItemController> logger, IMapper mapper, IUserInfo userInfo,
            IItemApi itemApi) : base(logger, mapper, userInfo)
        {
            _itemApi = itemApi;
        }

        [HttpGet]
        public IActionResult GetItemTypes()
        {
            List<dynamic> selects = new List<dynamic>(){
                new { opValue = EnumItemType.Normal.ToString(), opName = EnumItemType.Normal.ToDescription()},
            };
            return Json(new Result() { Success = true, Data = selects });
        }

        [HttpGet]
        public IActionResult GetItemPriceTypes()
        {
            List<dynamic> selects = new List<dynamic>(){
                new { opValue = EnumPriceType.General.ToString(), opName = EnumPriceType.General.ToDescription()},
                new { opValue = EnumPriceType.Count.ToString(), opName = EnumPriceType.Count.ToDescription()},
                new { opValue = EnumPriceType.Weight.ToString(), opName = EnumPriceType.Weight.ToDescription()},
            };
            return Json(new Result() { Success = true, Data = selects });
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SelectItem()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            ItemCreateDto model = new ItemCreateDto
            {
                //Code = "0000000000000",
                ShopId = _userInfo.ShopId,
                State = EnumItemStatus.Normal,
                Type = EnumItemType.Normal,
                PriceType = EnumPriceType.General,
                CategorieId = 0,
                UnitId = 0,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ItemCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(ItemDto model)
        {
            var dto = _mapper.Map<ItemUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopWhereQueryAsync(int page, int limit, string code, string name)
        {
            code = System.Web.HttpUtility.UrlEncode(code);
            name = System.Web.HttpUtility.UrlEncode(name);
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, code, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _itemApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的商品", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
