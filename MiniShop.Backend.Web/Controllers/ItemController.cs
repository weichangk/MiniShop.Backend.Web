using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Code;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Model.Enums;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniShop.Backend.Web.Controllers
{
    public class ItemController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IItemApi _itemApi;
        public ItemController(ILogger<ItemController> logger, IMapper mapper, IUserInfo userInfo, IConfiguration configuration,
            IItemApi itemApi) : base(logger, mapper, userInfo)
        {
            _configuration = configuration;
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
        public async Task<IActionResult> GetAutoItemCode()
        {
            byte[] buffer = _userInfo.ShopId.ToByteArray();
            long shopCode = BitConverter.ToInt64(buffer, 0);
            Random rd = new Random();
            int rdCode = rd.Next(0, 99999);
            string itemCode = $"690{shopCode.ToString().Substring(0,4)}{rdCode.ToString().PadLeft(5,'0')}";
            
            //EAN13条形码校验位计算规则为：奇数位和的三倍+偶数位和对10取余，若余数为0，则校验位为0，若余数不为零，则校验位为10-余数。
            int EAN = ((itemCode[1] + itemCode[3] + itemCode[5] + itemCode[7] + itemCode[9] + itemCode[11]) * 3 + (itemCode[0] + itemCode[2] + itemCode[4] + itemCode[6] + itemCode[8] + itemCode[10])) % 10;
            int EAN13 = EAN == 0 ? 0 : 10 - EAN;

            itemCode = $"{itemCode}{EAN13}";
            var queryItem = await ExecuteApiResultModelAsync(() => { return _itemApi.GetByShopIdCodeAsync(_userInfo.ShopId, itemCode); });
            if (queryItem.Data != null)
            {
                await GetAutoItemCode();
            }
            return Json(new Result() { Success = true, Data = itemCode });
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
            if(!string.IsNullOrEmpty(model.PictureBase64))
            {
                model.Picture = $"{_userInfo.ShopId}-{model.Code}";
            }
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.InsertAsync(model); });
            if(result.Success && !string.IsNullOrEmpty(model.PictureBase64))
            {
                var uploadImg = await ExecuteApiResultModelAsync(() => { return _itemApi.UploadImgAsync(model.Picture, model.PictureBase64); });
                if(!uploadImg.Success)
                {
                    result.Msg = "图片上传失败";
                }
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            ViewBag.MinishopItemimgCosDomain = _configuration["MinishopItemimgCosDomain"];
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
            dto.Picture = $"{_userInfo.ShopId}-{dto.Code}";
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.UpdateAsync(dto); });
            if(result.Success && !string.IsNullOrEmpty(dto.PictureBase64))
            {
                var uploadImg = await ExecuteApiResultModelAsync(() => { return _itemApi.UploadImgAsync(dto.Picture, dto.PictureBase64); });
                if(!uploadImg.Success)
                {
                    result.Msg = "图片上传失败";
                }
            }
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
            var item = await ExecuteApiResultModelAsync(() => { return _itemApi.GetByIdAsync(id); });
            var result = await ExecuteApiResultModelAsync(() => { return _itemApi.DeleteAsync(id); });
            if(result.Success && item.Success)
            {            
                var deleteImg = await ExecuteApiResultModelAsync(() => { return _itemApi.DeleteImgAsync(item.Data.Picture); });
                if(!deleteImg.Success)
                {
                    result.Msg = "图片删除失败";
                }
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string ids)
        {
            List<string> idsStrList = ids.Split(",").ToList();
            List<int> idsIntList = new List<int>();
            List<string> pictures = new List<string>();
            foreach (var id in idsStrList)
            {
                idsIntList.Add(int.Parse(id));
                var item = await ExecuteApiResultModelAsync(() => { return _itemApi.GetByIdAsync(int.Parse(id)); });
                if(item.Success)
                {
                    pictures.Add(item.Data.Picture);
                }
            }

            if (idsIntList.Count > 0)
            {
                var result = await ExecuteApiResultModelAsync(() => { return _itemApi.BatchDeleteAsync(idsIntList); });
                if(result.Success)
                {
                    var deleteImg = await ExecuteApiResultModelAsync(() => { return _itemApi.BatchDeleteImgAsync(pictures); });
                    if(!deleteImg.Success)
                    {
                        result.Msg = "图片删除失败";
                    }
                }
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的商品", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
