using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MiniShop.Backend.Model.Enums;
using AutoMapper;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Model.Code;
using Weick.Orm.Core.Result;
using Weick.Orm.Core;
using System;
using WebApiClientCore.Parameters;

namespace MiniShop.Backend.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApi _userApi;
        private readonly IStoreApi _storeApi;

        public UserController(ILogger<UserController> logger, IMapper mapper, IUserInfo userInfo,
            IUserApi userApi, IStoreApi storeApi) : base(logger, mapper, userInfo)
        {
            _userApi = userApi;
            _storeApi = storeApi;
        }

        public IActionResult Index()
        {
            ViewBag.CurrentRank = "";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetStoresByCurrentShopAsync()
        {
            var result = await ExecuteApiResultModelAsync(() => { return _storeApi.GetByShopIdAsync(_userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            if (result.Data != null)
            {
                List<dynamic> rankSelect = new List<dynamic>();
                foreach (var item in result.Data)
                {
                    var op = new { opValue = item.StoreId, opName = item.Name };
                    rankSelect.Add(op);
                }
                return Json(new Result() { Success = true, Data = rankSelect });
            }
            return Json(new Result() { Success = false });
        }

        [HttpGet]
        public IActionResult GetRankScopeByCurrentRankForAddUser()
        {
            List<dynamic> rankSelect = new List<dynamic>();
            var shopAssistantOp = new { opValue = EnumRole.ShopAssistant.ToString(), opName = EnumRole.ShopAssistant.ToDescription() };
            var storeManagerOp = new { opValue = EnumRole.StoreManager.ToString(), opName = EnumRole.StoreManager.ToDescription() };
            var storeAssistantOp = new { opValue = EnumRole.StoreAssistant.ToString(), opName = EnumRole.StoreAssistant.ToDescription() };
            var cashierOp = new { opValue = EnumRole.Cashier.ToString(), opName = EnumRole.Cashier.ToDescription() };
            switch (_userInfo.Rank)
            {          
                case EnumRole.ShopManager:
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.ShopAssistant:
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreManager:
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreAssistant:
                    rankSelect.Add(cashierOp);
                    break;
                //case EnumRole.Cashier:
                //    break;
                default:
                    break;
            }
            return Json(new Result() { Success = true, Data = rankSelect });
        }

        [HttpGet]
        public IActionResult GetRankScopeByCurrentRankForUpdateUser()
        {
            List<dynamic> rankSelect = new List<dynamic>();
            var shopManagerOp = new { opValue = EnumRole.ShopManager.ToString(), opName = EnumRole.ShopManager.ToDescription() };
            var shopAssistantOp = new { opValue = EnumRole.ShopAssistant.ToString(), opName = EnumRole.ShopAssistant.ToDescription() };
            var storeManagerOp = new { opValue = EnumRole.StoreManager.ToString(), opName = EnumRole.StoreManager.ToDescription() };
            var storeAssistantOp = new { opValue = EnumRole.StoreAssistant.ToString(), opName = EnumRole.StoreAssistant.ToDescription() };
            var cashierOp = new { opValue = EnumRole.Cashier.ToString(), opName = EnumRole.Cashier.ToDescription() };
            switch (_userInfo.Rank)
            {
                case EnumRole.ShopManager:
                    rankSelect.Add(shopManagerOp);
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.ShopAssistant:
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreManager:
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreAssistant:
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                //case EnumRole.Cashier:
                //    break;
                default:
                    break;
            }
            return Json(new Result() { Success = true, Data = rankSelect });
        }

        [HttpGet]
        public IActionResult GetRankScopeByCurrentRankForQueryUser()
        {
            List<dynamic> rankSelect = new List<dynamic>();
            var shopManagerOp = new { opValue = EnumRole.ShopManager.ToString(), opName = EnumRole.ShopManager.ToDescription() };
            var shopAssistantOp = new { opValue = EnumRole.ShopAssistant.ToString(), opName = EnumRole.ShopAssistant.ToDescription() };
            var storeManagerOp = new { opValue = EnumRole.StoreManager.ToString(), opName = EnumRole.StoreManager.ToDescription() };
            var storeAssistantOp = new { opValue = EnumRole.StoreAssistant.ToString(), opName = EnumRole.StoreAssistant.ToDescription() };
            var cashierOp = new { opValue = EnumRole.Cashier.ToString(), opName = EnumRole.Cashier.ToDescription() };
            switch (_userInfo.Rank)
            {
                case EnumRole.ShopManager:
                    rankSelect.Add(shopManagerOp);
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.ShopAssistant:
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreManager:
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreAssistant:
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                //case EnumRole.Cashier:
                //    break;
                default:
                    break;
            }
            return Json(new Result() { Success = true, Data = rankSelect });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageAsync(int page, int limit)
        {
            ResultModel<PagedList<UserDto>> result = new ResultModel<PagedList<UserDto>>();
            switch (_userInfo.Rank)
            {
                case EnumRole.ShopManager:
                case EnumRole.ShopAssistant:
                    result = await ExecuteApiResultModelAsync(() => { return _userApi.GetPageByShopIdRankAsync(page, limit, _userInfo.ShopId, _userInfo.Rank); });
                    break;
                case EnumRole.StoreManager:
                case EnumRole.StoreAssistant:
                case EnumRole.Cashier:
                    result = await ExecuteApiResultModelAsync(() => { return _userApi.GetPageByShopIdStoreIdRankAsync(page, limit, _userInfo.ShopId, _userInfo.StoreId, _userInfo.Rank); });
                    break;
            }
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageWhereQueryAsync(int page, int limit, Guid? store, EnumRole? rank, string name, string phone)
        {
            name = System.Web.HttpUtility.UrlEncode(name);
            phone = System.Web.HttpUtility.UrlEncode(phone);

            ResultModel<PagedList<UserDto>> result = new ResultModel<PagedList<UserDto>>();
            switch (_userInfo.Rank)
            {
                case EnumRole.ShopManager:
                case EnumRole.ShopAssistant:
                    result = await ExecuteApiResultModelAsync(() => { return _userApi.GetPageByShopIdStoreIdRankWhereQueryWithStoreIdAsync(page, limit, _userInfo.ShopId, _userInfo.Rank, store, rank, name, phone); });
                    break;
                case EnumRole.StoreManager:
                case EnumRole.StoreAssistant:
                case EnumRole.Cashier:
                     result = await ExecuteApiResultModelAsync(() => { return _userApi.GetPageByShopIdStoreIdRankWhereQueryWithStoreIdAsync(page, limit, _userInfo.ShopId, _userInfo.Rank, _userInfo.StoreId, rank, name, phone); });
                    break;
                default:
                    break;
            }
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpGet]
        public IActionResult Add()
        {
            UserCreateDto model = new UserCreateDto 
            { 
                ShopId = _userInfo.ShopId,
                StoreId = _userInfo.StoreId,
                //Rank = EnumRole.Cashier, 
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _userApi.InsertByRankAsync(_userInfo.Rank, model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAsync(string name)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _userApi.GetByNameAsync(name); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(UserDto model)
        {
            var dto = _mapper.Map<UserUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _userApi.UpdateByRankAsync(_userInfo.Rank, dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeFreezeStateAsync(string name, bool freeze)
        {
            var doc = new JsonPatchDocument<UserUpdateDto>();
            doc.Replace(item => item.IsFreeze, freeze);
            var result = await ExecuteApiResultModelAsync(() => { return _userApi.PatchUpdateByRankNameAsync(_userInfo.Rank, name, doc); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _userApi.DeleteByRankNameAsync(_userInfo.Rank, name); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string names)
        {
            List<string> namesList = names.Split(",").ToList();
            var result = await ExecuteApiResultModelAsync(() => { return _userApi.BatchDeleteByRankNamesAsync(_userInfo.Rank, namesList); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

    }

}
