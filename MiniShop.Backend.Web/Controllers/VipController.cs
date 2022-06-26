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
using System;

namespace MiniShop.Backend.Web.Controllers
{
    public class VipController : BaseController
    {
        private readonly IVipApi _vipApi;
        public VipController(ILogger<VipController> logger, IMapper mapper, IUserInfo userInfo,
            IVipApi vipApi) : base(logger, mapper, userInfo)
        {
            _vipApi = vipApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAutoVipCode()
        {
            byte[] buffer = _userInfo.ShopId.ToByteArray();
            long shopCode = BitConverter.ToInt64(buffer, 0);
            Random rd = new Random();
            int rdCode = rd.Next(0, 999999);
            string vipCode = $"{shopCode.ToString().Substring(0,4)}{rdCode.ToString().PadLeft(6,'0')}";
            var queryVip = await ExecuteApiResultModelAsync(() => { return _vipApi.GetByShopIdCodeAsync(_userInfo.ShopId, vipCode); });
            if (queryVip.Data != null)
            {
                await GetAutoVipCode();
            }
            return Json(new Result() { Success = true, Data = vipCode });
        }

        [HttpGet]
        public IActionResult GetSexs()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumSex>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }

        [HttpGet]
        public IActionResult GetVipStatus()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumVipStatus>();
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
            VipCreateDto model = await Task.FromResult(
                new VipCreateDto
                {
                    ShopId = _userInfo.ShopId,   
                    ValidDate = DateTime.Now.AddYears(99),
                    State = EnumVipStatus.Normal,
                }
            );
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(VipCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _vipApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _vipApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(VipDto model)
        {
            var dto = _mapper.Map<VipUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _vipApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _vipApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdWhereQueryAsync(int page, int limit, string code, string name, string phone)
        {
            code = System.Web.HttpUtility.UrlEncode(code);
            name = System.Web.HttpUtility.UrlEncode(name);
            phone = System.Web.HttpUtility.UrlEncode(phone);
            var result = await ExecuteApiResultModelAsync(() => { return _vipApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, code, name, phone); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _vipApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _vipApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "没有选择要删除的会员积分设置", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
