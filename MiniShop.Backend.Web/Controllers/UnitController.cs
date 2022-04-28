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
    public class UnitController : BaseController
    {
        private readonly IUnitApi _unitApi;
        public UnitController(ILogger<UnitController> logger, IMapper mapper, IUserInfo userInfo,
            IUnitApi unitApi) : base(logger, mapper, userInfo)
        {
            _unitApi = unitApi;
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
        public async Task<IActionResult> Add()
        {
            var maxCodeResult = await _unitApi.GetMaxCodeByShopIdAsync(_userInfo.ShopId);
            if (!maxCodeResult.Success)
            {
                return Json(new Result() { Success = maxCodeResult.Success, Status = maxCodeResult.Status, Msg = maxCodeResult.Msg });
            }
            int maxCode = maxCodeResult.Data == 0 ? 100 : maxCodeResult.Data + 1;
            if (maxCode == 999)
            {
                return Json(new Result() { Success = false, Msg = "该商店拥有的单位数量已达上限", Status = (int)HttpStatusCode.BadRequest });
            }
            UnitCreateDto model = new UnitCreateDto
            {
                ShopId = _userInfo.ShopId,
                Code = maxCode,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UnitCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _unitApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _unitApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(UnitDto model)
        {
            var dto = _mapper.Map<UnitUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _unitApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _unitApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopWhereQueryCodeOrNameAsync(int page, int limit, string code, string name)
        {
            code = System.Web.HttpUtility.UrlEncode(code);
            name = System.Web.HttpUtility.UrlEncode(name);
            var result = await ExecuteApiResultModelAsync(() => { return _unitApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, code, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _unitApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _unitApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "没有选择要删除的单位", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
