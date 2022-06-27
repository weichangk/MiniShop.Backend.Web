using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Model.Enums;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Web.HttpApis;
using MiniShop.Backend.Web.Models;
using System.Threading.Tasks;

namespace MiniShop.Backend.Web.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IShopApi _shopApi;
        private readonly IStoreApi _storeApi;
        private readonly ICategorieApi _categorieApi;
        private readonly IUnitApi _unitApi;
        private readonly ISupplierApi _supplierApi;
        private readonly IItemApi _itemApi;
        private readonly IPaymentApi _paymentApi;
        public HomeController(ILogger<HomeController> logger, IMapper mapper, IUserInfo userInfo,
            IShopApi shopApi,
            IStoreApi storeApi,
            ICategorieApi categorieApi,
            IUnitApi unitApi,
            ISupplierApi supplierApi,
            IItemApi itemApi,
            IPaymentApi paymentApi) : base(logger, mapper, userInfo)
        {
            _shopApi = shopApi;
            _storeApi = storeApi;
            _categorieApi = categorieApi;
            _unitApi = unitApi;
            _supplierApi = supplierApi;
            _itemApi = itemApi;
            _paymentApi = paymentApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await SetShopDefaultInfo();
            return View();
        }

        [HttpGet]
        public IActionResult Console()
        {
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Privacy()
        {
            ViewBag.IdToken = _userInfo.IdToken;
            ViewBag.AccessToken = _userInfo.AccessToken;
            ViewBag.RefreshToken = _userInfo.RefreshToken;
            ViewBag.UserName = _userInfo.UserName;
            ViewBag.Rank = _userInfo.Rank;
            ViewBag.ShopId = _userInfo.ShopId;
            ViewBag.StoreId = _userInfo.StoreId;
            ViewBag.PhoneNumber = _userInfo.PhoneNumber;
            ViewBag.Email = _userInfo.Email;
            ViewBag.IsFreeze = _userInfo.IsFreeze;
            ViewBag.CreatedTime = _userInfo.CreatedTime;
            ViewBag.RefreshToken = HttpContext.GetTokenAsync(Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectParameterNames.RefreshToken).Result;

            return View();
        }

        private async Task<IActionResult> SetShopDefaultInfo()
        {
            if (_userInfo.Rank == Model.Enums.EnumRole.ShopManager)
            {
                var queryShop = await ExecuteApiResultModelAsync(() => { return _shopApi.GetByShopIdAsync(_userInfo.ShopId); });
                if (!queryShop.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = queryShop.Status, errorMsg = queryShop.Msg });
                }
                if (queryShop.Data == null)
                {
                    ShopCreateDto shopCreateDto = new ShopCreateDto
                    {
                        ShopId = _userInfo.ShopId,
                        Name = $"{_userInfo.UserName}的商店",
                        Contacts = _userInfo.UserName,
                        Phone = _userInfo.PhoneNumber,
                        Email = _userInfo.Email,
                        ValidDate = System.DateTime.Now.AddDays(7),
                    };
                    var addShop = await ExecuteApiResultModelAsync(() => { return _shopApi.InsertAsync(shopCreateDto); });
                    if (!addShop.Success)
                    {
                        return RedirectToAction("Error", "Error", new { statusCode = addShop.Status, errorMsg = addShop.Msg });
                    }
                }

                var queryStore = await ExecuteApiResultModelAsync(() => { return _storeApi.GetByStoreIdAsync(_userInfo.StoreId); });
                if (!queryStore.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = queryShop.Status, errorMsg = queryShop.Msg });
                }
                if (queryStore.Data == null)
                {
                    StoreCreateDto storeCreateDto = new StoreCreateDto
                    {
                        StoreId = _userInfo.StoreId,
                        ShopId = _userInfo.ShopId,
                        Name = $"{_userInfo.UserName}的门店",
                        Contacts = _userInfo.UserName,
                        Phone = _userInfo.PhoneNumber,
                    };
                    var addStore = await ExecuteApiResultModelAsync(() => { return _storeApi.InsertAsync(storeCreateDto); });
                    if (!addStore.Success)
                    {
                        return RedirectToAction("Error", "Error", new { statusCode = addStore.Status, errorMsg = addStore.Msg });
                    }
                }

                #region 系统内置支付方式
                var queryPayment = await ExecuteApiResultModelAsync(() => { return _paymentApi.GetByShopIdCodeAsync(_userInfo.ShopId, "CAS"); });
                if (!queryPayment.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = queryShop.Status, errorMsg = queryShop.Msg });
                }
                if (queryPayment.Data == null)
                {
                    PaymentCreateDto paymentCreateDto = new PaymentCreateDto
                    {
                        ShopId = _userInfo.ShopId,
                        Code = "CAS",
                        Name = "现金",
                        Enable = EnumEnableStatus.Enable,
                        SystemPayment = EnumYesOrNoStatus.Yes,
                    };
                    var addPayment = await ExecuteApiResultModelAsync(() => { return _paymentApi.InsertAsync(paymentCreateDto); });
                    if (!addPayment.Success)
                    {
                        return RedirectToAction("Error", "Error", new { statusCode = addPayment.Status, errorMsg = addPayment.Msg });
                    }
                }
                queryPayment = await ExecuteApiResultModelAsync(() => { return _paymentApi.GetByShopIdCodeAsync(_userInfo.ShopId, "ALP"); });
                if (!queryPayment.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = queryPayment.Status, errorMsg = queryPayment.Msg });
                }
                if (queryPayment.Data == null)
                {
                    PaymentCreateDto paymentCreateDto = new PaymentCreateDto
                    {
                        ShopId = _userInfo.ShopId,
                        Code = "ALP",
                        Name = "支付宝",
                        Enable = EnumEnableStatus.Enable,
                        SystemPayment = EnumYesOrNoStatus.Yes,
                    };
                    var addPayment = await ExecuteApiResultModelAsync(() => { return _paymentApi.InsertAsync(paymentCreateDto); });
                    if (!addPayment.Success)
                    {
                        return RedirectToAction("Error", "Error", new { statusCode = addPayment.Status, errorMsg = addPayment.Msg });
                    }
                }
                queryPayment = await ExecuteApiResultModelAsync(() => { return _paymentApi.GetByShopIdCodeAsync(_userInfo.ShopId, "WEC"); });
                if (!queryPayment.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = queryPayment.Status, errorMsg = queryPayment.Msg });
                }
                if (queryPayment.Data == null)
                {
                    PaymentCreateDto paymentCreateDto = new PaymentCreateDto
                    {
                        ShopId = _userInfo.ShopId,
                        Code = "WEC",
                        Name = "微信",
                        Enable = EnumEnableStatus.Enable,
                        SystemPayment = EnumYesOrNoStatus.Yes,
                    };
                    var addPayment = await ExecuteApiResultModelAsync(() => { return _paymentApi.InsertAsync(paymentCreateDto); });
                    if (!addPayment.Success)
                    {
                        return RedirectToAction("Error", "Error", new { statusCode = addPayment.Status, errorMsg = addPayment.Msg });
                    }
                }
                #endregion
            }

            return Json(new Result() { Success = true });
        }

    }
}
