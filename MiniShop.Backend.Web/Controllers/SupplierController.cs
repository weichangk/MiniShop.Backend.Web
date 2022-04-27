using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Model.Code;
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
    public class SupplierController : BaseController
    {
        private readonly ISupplierApi _supplierApi;
        public SupplierController(ILogger<SupplierController> logger, IMapper mapper, IUserInfo userInfo,
            ISupplierApi supplierApi) : base(logger, mapper, userInfo)
        {
            _supplierApi = supplierApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetSupplierStatus()
        {
            List<dynamic> statusSelect = new List<dynamic>();
            var dic = EnumExtensions.ToNameAndDesDictionary<EnumSupplierStatus>();
            foreach (var item in dic)
            {
                var op = new { opValue = item.Key, opName = item.Value };
                statusSelect.Add(op);
            }

            return Json(new Result() { Success = true, Data = statusSelect });
        }

        [HttpGet]
        public async Task<IActionResult> AddAsync()
        {
            var maxCodeResult = await _supplierApi.GetMaxCodeByShopIdAsync(_userInfo.ShopId);
            if (!maxCodeResult.Success)
            {
                return Json(new Result() { Success = maxCodeResult.Success, Status = maxCodeResult.Status, Msg = maxCodeResult.Msg });
            }
            int maxCode = maxCodeResult.Data == 0 ? 100 : maxCodeResult.Data + 1;
            if (maxCode == 999)
            {
                return Json(new Result() { Success = false, Msg = "该商店拥有的供应商数量已达上限", Status = (int)HttpStatusCode.BadRequest });
            }
            SupplierCreateDto model = new SupplierCreateDto
            {
                ShopId = _userInfo.ShopId,
                Code = maxCode,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(SupplierCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _supplierApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _supplierApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(SupplierDto model)
        {
            var dto = _mapper.Map<SupplierUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _supplierApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _supplierApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
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
            var result = await ExecuteApiResultModelAsync(() => { return _supplierApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, code, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _supplierApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _supplierApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的供应商", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
