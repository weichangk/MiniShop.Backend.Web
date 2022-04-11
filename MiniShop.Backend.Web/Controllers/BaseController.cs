using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Weick.Orm.Core;
using Weick.Orm.Core.Result;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Exceptions;
using MiniShop.Backend.Web.Code;
using MiniShop.Backend.Web.Models;

namespace MiniShop.Backend.Web.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(RefreshAccessTokenAttribute))]
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly IMapper _mapper;
        protected readonly IUserInfo _userInfo;
        public BaseController(ILogger<BaseController> logger, IMapper mapper, IUserInfo userInfo)
        {
            _logger = logger;
            _mapper = mapper;
            _userInfo = userInfo;
        }

        public async Task<IActionResult> ExecuteApiAsync<T>(Func<ITask<ResultModel<T>>> action)
        {
            int statusCode;
            string msg;
            try
            {
                var result = await action();
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiInvalidConfigException configException)
            {
                // 请求配置异常
                msg = configException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiResponseStatusException statusException)
            {
                // 响应状态码异常
                msg = statusException.Message;
                statusCode = (int)statusException.StatusCode;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiException apiException)
            {
                // 抽象的api异常
                msg = apiException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                // socket连接层异常
                msg = socketException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ValidationException validationException)
            {
                // 数据验证异常
                msg = validationException.Message;
                statusCode = 400;
            }
            catch (HttpRequestException ex)
            {
                // 请求异常
                msg = ex.Message;
                statusCode = 500;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                statusCode = 500;
            }
            return Json(new Result() { Success = false, Msg = msg, Status = statusCode });
        }

        public async Task<ResultModel<T>> ExecuteApiResultModelAsync<T>(Func<ITask<ResultModel<T>>> action)
        {
            int statusCode;
            string msg;
            try
            {
                var result = await action();
                return result;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiInvalidConfigException configException)
            {
                // 请求配置异常
                msg = configException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiResponseStatusException statusException)
            {
                // 响应状态码异常
                msg = statusException.Message;
                statusCode = (int)statusException.StatusCode;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiException apiException)
            {
                // 抽象的api异常
                msg = apiException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                // socket连接层异常
                msg = socketException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ValidationException validationException)
            {
                // 数据验证异常
                msg = validationException.Message;
                statusCode = 400;
            }
            catch (HttpRequestException ex)
            {
                // 请求异常
                msg = ex.Message;
                statusCode = 500;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                statusCode = 500;
            }
            return new ResultModel<T> { Success = false, Msg = msg, Status = statusCode };
        }

        public async Task<ResultModel<PagedList<T>>> ExecuteApiResultModelAsync<T>(Func<ITask<ResultModel<PagedList<T>>>> action)
        {
            int statusCode;
            string msg;
            try
            {
                var result = await action();
                return result;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiInvalidConfigException configException)
            {
                // 请求配置异常
                msg = configException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiResponseStatusException statusException)
            {
                // 响应状态码异常
                msg = statusException.Message;
                statusCode = (int)statusException.StatusCode;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiException apiException)
            {
                // 抽象的api异常
                msg = apiException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                // socket连接层异常
                msg = socketException.Message;
                statusCode = 500;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ValidationException validationException)
            {
                // 数据验证异常
                msg = validationException.Message;
                statusCode = 400;
            }
            catch (HttpRequestException ex)
            {
                // 请求异常
                msg = ex.Message;
                statusCode = 500;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                statusCode = 500;
            }
            return new ResultModel<PagedList<T>> { Success = false, Msg = msg, Status = statusCode };
        }
    }
}
