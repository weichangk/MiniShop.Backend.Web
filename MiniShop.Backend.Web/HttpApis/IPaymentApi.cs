using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Backend.Model.Dto;
using MiniShop.Backend.Web.Code;
using Weick.Orm.Core;
using Weick.Orm.Core.Result;
using System;
using System.Collections.Generic;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IPaymentApi : IHttpApi
    {
        [HttpGet("/api/Payment/GetByIdAsync")]
        ITask<ResultModel<PaymentDto>> GetByIdAsync(int id);

        [HttpGet("/api/Payment/GetByShopIdCodeAsync")]
        ITask<ResultModel<PaymentDto>> GetByShopIdCodeAsync(Guid shopId, string code);
   
        [HttpGet("/api/Payment/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PaymentDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/Payment/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PaymentDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);

        [HttpDelete("/api/Payment/DeleteAsync")]
        ITask<ResultModel<PaymentDto>> DeleteAsync(int id);

        [HttpDelete("/api/Payment/BatchDeleteAsync")]
        ITask<ResultModel<PaymentDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/Payment/InsertAsync")]
        ITask<ResultModel<PaymentCreateDto>> InsertAsync([JsonContent] PaymentCreateDto model);

        [HttpPut("/api/Payment/UpdateAsync")]
        ITask<ResultModel<PaymentUpdateDto>> UpdateAsync([JsonContent] PaymentUpdateDto model);

        [HttpPatch("/api/Payment/PatchAsync")]
        ITask<ResultModel<PaymentDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PaymentUpdateDto> doc);
    }
}
