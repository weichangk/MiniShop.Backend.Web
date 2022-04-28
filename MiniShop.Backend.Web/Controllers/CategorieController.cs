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

namespace MiniShop.Backend.Web.Controllers
{
    public class CategorieController : BaseController
    {
        private readonly ICategorieApi _categorieApi;
        public CategorieController(ILogger<CategorieController> logger, IMapper mapper, IUserInfo userInfo,
            ICategorieApi categorieApi) : base(logger, mapper, userInfo)
        {
            _categorieApi = categorieApi;
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
            var maxCodeResult = await _categorieApi.GetMaxCodeByShopIdLevelAsync(_userInfo.ShopId, 0);
            if (!maxCodeResult.Success)
            {
                return Json(new Result() { Success = maxCodeResult.Success, Status = maxCodeResult.Status, Msg = maxCodeResult.Msg });
            }
            int maxCode = maxCodeResult.Data == 0 ? 100 : maxCodeResult.Data + 1;
            if (maxCode == 999)
            {
                return Json(new Result() { Success = false, Msg = "该类别等级拥有的类别数量已达上限", Status = (int)HttpStatusCode.BadRequest });
            }
            CategorieCreateDto model = new CategorieCreateDto
            {
                ShopId = _userInfo.ShopId,
                Code = maxCode,
                ParentCode = maxCode,//目前默认只有一级类别，且一级类别的类别编码等于父类别编码
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CategorieCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _categorieApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _categorieApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(CategorieDto model)
        {
            var dto = _mapper.Map<CategorieUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _categorieApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _categorieApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
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
            var result = await ExecuteApiResultModelAsync(() => { return _categorieApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, code, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _categorieApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _categorieApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "没有选择要删除的类别", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
