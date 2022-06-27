using System.Security.Cryptography.X509Certificates;
using System;
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
using MiniShop.Backend.Model.Enums;
using MiniShop.Backend.Model.Code;

namespace MiniShop.Backend.Web.Controllers
{
    public class PromotionOderController : BaseController
    {
        private readonly IPromotionOderApi _promotionOderApi;
        private readonly IVipTypeApi _vipTypeApi;

        public PromotionOderController(ILogger<PromotionOderController> logger, IMapper mapper, IUserInfo userInfo,
            IPromotionOderApi promotionOderApi,
            IVipTypeApi vipTypeApi) : base(logger, mapper, userInfo)
        {
            _promotionOderApi = promotionOderApi;
            _vipTypeApi = vipTypeApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetVipTypesByCurrentShopAsync()
        {
            var result = await ExecuteApiResultModelAsync(() => { return _vipTypeApi.GetByShopIdAsync(_userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            if (result.Data != null)
            {
                List<dynamic> supplierSelect = new List<dynamic>();
                foreach (var item in result.Data)
                {
                    var op = new { opValue = item.Id, opName = item.Name };
                    supplierSelect.Add(op);
                }
                return Json(new Result() { Success = true, Data = supplierSelect });
            }
            return Json(new Result() { Success = false });
        }

        [HttpGet]
        public IActionResult GetPromotionSpecialWays()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumPromotionSpecialOfferWay>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }

        [HttpGet]
        public IActionResult GetPromotionSpecialScopes()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumPromotionSpecialOfferScope>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }

        [HttpGet]
        public IActionResult GetPromotionDiscountWays()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumPromotionDiscountWay>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }

        [HttpGet]
        public IActionResult GetPromotionDiscountScopes()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumPromotionDiscountScope>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }
        
        private async Task<string> GetAutoOderNo()
        {
            byte[] buffer = _userInfo.ShopId.ToByteArray();
            long shopCode = BitConverter.ToInt64(buffer, 0);
            Random rd = new Random();
            int rdCode = rd.Next(0, 9999);
            string oderNo = $"PP-{shopCode.ToString().Substring(0,4)}-{DateTime.Now.Format("yyMMdd")}-{rdCode.ToString().PadLeft(4,'0')}";
            var queryPromotionOder = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.GetByShopIdOderNoAsync(_userInfo.ShopId, oderNo); });
            if (queryPromotionOder.Data != null)
            {
                await GetAutoOderNo();
            }
            return oderNo;
        }

        [HttpGet]
        public async Task<IActionResult> AddPromotionSpecialAsync()
        {
            PromotionOderCreateDto model = await Task.FromResult(
                new PromotionOderCreateDto
                {
                    ShopId = _userInfo.ShopId,
                    PromotionType = EnumPromotionType.SpecialOffer,
                    OderNo = await GetAutoOderNo(),
                    OperatorName = _userInfo.UserName,
                }
            );
            return View(model);
        }

        [HttpPost]
        public IActionResult AddPromotionSpecialAsync(PromotionOderCreateDto model)
        {
            var result =  ExecuteApiResultModelAsync(() => { return _promotionOderApi.InsertAsync(model); }).Result;
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

        [HttpGet]
        public async Task<IActionResult> AddPromotionDiscountAsync()
        {
            PromotionOderCreateDto model = await Task.FromResult(
                new PromotionOderCreateDto
                {
                    ShopId = _userInfo.ShopId,
                    PromotionType = EnumPromotionType.Discount,
                    OderNo = await GetAutoOderNo(),
                    OperatorName = _userInfo.UserName,
                }
            );
            return View(model);
        }

        [HttpPost]
        public IActionResult AddPromotionDiscountAsync(PromotionOderCreateDto model)
        {
            var result =  ExecuteApiResultModelAsync(() => { return _promotionOderApi.InsertAsync(model); }).Result;
            return Json(new Result() { Success = result.Success, Data = result.Data, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdatePromotionSpecialAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdatePromotionDiscountAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }  

        [HttpPost]
        public async Task<IActionResult> UpdatePromotionSpecialAsync(PromotionOderDto model)
        {
            var dto = _mapper.Map<PromotionOderUpdateDto>(model);
            dto.AuditOperatorName = _userInfo.UserName;
            dto.AuditTime = DateTime.Now;
            dto.AuditState = EnumAuditStatus.Audited;
            var result = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        } 

        [HttpPost]
        public async Task<IActionResult> UpdatePromotionDiscountAsync(PromotionOderDto model)
        {
            var dto = _mapper.Map<PromotionOderUpdateDto>(model);
            dto.AuditOperatorName = _userInfo.UserName;
            dto.AuditTime = DateTime.Now;
            dto.AuditState = EnumAuditStatus.Audited;
            var result = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        } 

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdWhereQueryAsync(int page, int limit, int storeId, string oderNo)
        {
            oderNo = System.Web.HttpUtility.UrlEncode(oderNo);
            var result = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, storeId, oderNo); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _promotionOderApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的促销订单", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
