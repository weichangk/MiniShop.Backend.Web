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
    public class PaymentController : BaseController
    {
        private readonly IPaymentApi _paymentApi;
        public PaymentController(ILogger<PaymentController> logger, IMapper mapper, IUserInfo userInfo,
            IPaymentApi paymentApi) : base(logger, mapper, userInfo)
        {
            _paymentApi = paymentApi;
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

        [HttpGet]
        public IActionResult Add()
        {
            PaymentCreateDto model = new PaymentCreateDto
            {
                ShopId = _userInfo.ShopId,
                Enable = EnumEnableStatus.Enable,
                SystemPayment = EnumYesOrNoStatus.No,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(PaymentCreateDto model)
        {
            var getPayment = await _paymentApi.GetByShopIdCodeAsync(_userInfo.ShopId, model.Code);
            if (!getPayment.Success)
            {
                return Json(new Result() { Success = getPayment.Success, Status = getPayment.Status, Msg = getPayment.Msg });
            }
            if (getPayment.Data != null)
            {
                return Json(new Result() { Success = false, Msg = $"{model.Code}支付方式编码已经存在，请重新写入编码", Status = (int)HttpStatusCode.BadRequest });
            }
            var result = await ExecuteApiResultModelAsync(() => { return _paymentApi.InsertAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _paymentApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(PaymentDto model)
        {
            var getPayment1 = await _paymentApi.GetByIdAsync(model.Id);
            if (!getPayment1.Success)
            {
                return Json(new Result() { Success = getPayment1.Success, Status = getPayment1.Status, Msg = getPayment1.Msg });
            }
            if(getPayment1.Data.Code != model.Code)
            {
                var getPayment2 = await _paymentApi.GetByShopIdCodeAsync(_userInfo.ShopId, model.Code);
                if (!getPayment2.Success)
                {
                    return Json(new Result() { Success = getPayment2.Success, Status = getPayment2.Status, Msg = getPayment2.Msg });
                }
                if (getPayment2.Data != null)
                {
                    return Json(new Result() { Success = false, Msg = $"{model.Code}支付方式编码已经存在，请重新写入编码", Status = (int)HttpStatusCode.BadRequest });
                }
            }
            var dto = _mapper.Map<PaymentUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _paymentApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _paymentApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId); });
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
            var result = await ExecuteApiResultModelAsync(() => { return _paymentApi.GetPageByShopIdWhereQueryAsync(page, limit, _userInfo.ShopId, code, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _paymentApi.DeleteAsync(id); });
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
                var result = await ExecuteApiResultModelAsync(() => { return _paymentApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "没有选择要删除的支付方式", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
