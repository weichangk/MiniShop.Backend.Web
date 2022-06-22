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
    public class PosRegisterController : BaseController
    {
        private readonly IPosRegisterApi _posRegisterApi;
        public PosRegisterController(ILogger<PosRegisterController> logger, IMapper mapper, IUserInfo userInfo,
            IPosRegisterApi posRegisterApi) : base(logger, mapper, userInfo)
        {
            _posRegisterApi = posRegisterApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetEnableStatus()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumEnableStatus>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }


        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _posRegisterApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PosRegisterDto model)
        {
            var dto = _mapper.Map<PosRegisterUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _posRegisterApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _posRegisterApi.GetPageAsync(page, limit); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageWhereQueryCodeOrNameAsync(int page, int limit, string code, string name)
        {
            code = System.Web.HttpUtility.UrlEncode(code);
            name = System.Web.HttpUtility.UrlEncode(name);
            var result = await ExecuteApiResultModelAsync(() => { return _posRegisterApi.GetPageWhereQueryAsync(page, limit, code, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _posRegisterApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _posRegisterApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "没有选择要删除的POS登记", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
