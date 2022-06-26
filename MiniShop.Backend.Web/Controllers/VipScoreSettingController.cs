using System.Runtime.Serialization.Formatters;
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
    public class VipScoreSettingController : BaseController
    {
        private readonly IVipScoreSettingApi _vipScoreSettingApi;
        public VipScoreSettingController(ILogger<VipScoreSettingController> logger, IMapper mapper, IUserInfo userInfo,
            IVipScoreSettingApi vipScoreSettingApi) : base(logger, mapper, userInfo)
        {
            _vipScoreSettingApi = vipScoreSettingApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetVipScoreWays()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumVipScoreWay>();
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
            VipScoreSettingCreateDto model = await Task.FromResult(
                new VipScoreSettingCreateDto
                {
                    ShopId = _userInfo.ShopId,       
                }
            );
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(VipScoreSettingCreateDto model)
        {
            var getVipScoreSettingResult = await _vipScoreSettingApi.GetByShopIdVipTypeIdVipScoreWayAsync(_userInfo.ShopId, model.VipTypeId, model.VipScoreWay);
            if (!getVipScoreSettingResult.Success)
            {
                return Json(new Result() { Success = getVipScoreSettingResult.Success, Status = getVipScoreSettingResult.Status, Msg = getVipScoreSettingResult.Msg });
            }
            if(getVipScoreSettingResult.Data != null)
            {
                return Json(new Result() { Success = true, Msg = "该会员类别积分规则已存在，请重新选择会员类别", Status = (int)HttpStatusCode.BadRequest });
            }
            var result = await ExecuteApiResultModelAsync(() => { return _vipScoreSettingApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _vipScoreSettingApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(VipScoreSettingDto model)
        {
            var dto = _mapper.Map<VipScoreSettingUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _vipScoreSettingApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _vipScoreSettingApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _vipScoreSettingApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _vipScoreSettingApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "没有选择要删除的会员积分设置", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
