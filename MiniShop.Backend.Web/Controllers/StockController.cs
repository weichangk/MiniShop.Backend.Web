using System;
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
using Microsoft.AspNetCore.JsonPatch;

namespace MiniShop.Backend.Web.Controllers
{
    public class StockController : BaseController
    {
        private readonly IStockApi _stockApi;
        public StockController(ILogger<StockController> logger, IMapper mapper, IUserInfo userInfo,
            IStockApi stockApi) : base(logger, mapper, userInfo)
        {
            _stockApi = stockApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(StockCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _stockApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
 
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(StockDto model)
        {
            var dto = _mapper.Map<StockUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _stockApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _stockApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopWhereQueryCodeOrNameAsync(int page, int limit, string name)
        {
            name = System.Web.HttpUtility.UrlEncode(name);
            var result = await ExecuteApiResultModelAsync(() => { return _stockApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _stockApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _stockApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "没有选择要删除的库存", Status = (int)HttpStatusCode.NotFound });
            }

        }

        [HttpPatch]
        public async Task<IActionResult> UpdateStockNumberAsync(Guid shopId, int itemId, decimal number)
        {
            int stockId = 0;
            var getStock = await _stockApi.GetByShopIdAndItemIdAsync(shopId, itemId);
            if(getStock.Success)
            {
                stockId = getStock.Data.Id;
                if(getStock.Data == null)
                {
                    StockCreateDto stockCreateDto = new StockCreateDto
                    {
                        ShopId = shopId,
                        ItemId = itemId,
                        Number = 0,
                    };
                    var inserStock =  await _stockApi.InsertAsync(stockCreateDto);
                    stockId = inserStock.Data.Id;
                }
                var doc = new JsonPatchDocument<StockUpdateDto>();
                doc.Replace(item => item.Number, number);
                var result = await ExecuteApiResultModelAsync(() => { return _stockApi.PatchAsync(stockId, doc); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            return Json(new Result() { Success = getStock.Success, Msg = getStock.Msg, Status = getStock.Status });
        }
    }
}
