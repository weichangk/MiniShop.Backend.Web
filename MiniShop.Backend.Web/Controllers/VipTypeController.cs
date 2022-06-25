using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Model.Code;
using MiniShop.Backend.Model.Enums;

namespace MiniShop.Backend.Web.Controllers
{
    public class VipTypeController : BaseController
    {
        private readonly IVipTypeApi _viptypeApi;
        public VipTypeController(ILogger<VipTypeController> logger, IMapper mapper, IUserInfo userInfo,
            IVipTypeApi viptypeApi) : base(logger, mapper, userInfo)
        {
            _viptypeApi = viptypeApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Select()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetDiscountWays()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumDiscountWay>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }

        [HttpGet]
        public IActionResult GetYesOrNoStatus()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumYesOrNoStatus>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }
        
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var maxCodeResult = await _viptypeApi.GetMaxCodeByShopIdAsync(_userInfo.ShopId);
            if (!maxCodeResult.Success)
            {
                return Json(new Result() { Success = maxCodeResult.Success, Status = maxCodeResult.Status, Msg = maxCodeResult.Msg });
            }
            int maxCode = maxCodeResult.Data == 0 ? 10 : maxCodeResult.Data + 1;
            if (maxCode == 99)
            {
                return Json(new Result() { Success = false, Msg = "该类别等级拥有的类别数量已达上限", Status = (int)HttpStatusCode.BadRequest });
            }
            VipTypeCreateDto model = new VipTypeCreateDto
            {
                ShopId = _userInfo.ShopId,
                Code = maxCode,
                DiscountWay = EnumDiscountWay.PriceDiscount,        
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(VipTypeCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _viptypeApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _viptypeApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(VipTypeDto model)
        {
            var dto = _mapper.Map<VipTypeUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _viptypeApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _viptypeApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdWhereQueryAsync(int page, int limit, string code, string name)
        {
            code = System.Web.HttpUtility.UrlEncode(code);
            name = System.Web.HttpUtility.UrlEncode(name);
            var result = await ExecuteApiResultModelAsync(() => { return _viptypeApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, code, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _viptypeApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _viptypeApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "没有选择要删除的会员类别", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
