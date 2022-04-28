﻿using Microsoft.AspNetCore.JsonPatch;
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
    public interface IPurchaseOderApi : IHttpApi
    {
        [HttpGet("/api/PurchaseOder/GetByIdAsync")]
        ITask<ResultModel<PurchaseOderDto>> GetByIdAsync(int id);

        [HttpGet("/api/PurchaseOder/GetByShopIdOderNoAsync")]
        ITask<ResultModel<PurchaseOderDto>> GetByShopIdOderNoAsync(Guid shopId, string oderNo);
        
        [HttpGet("/api/PurchaseOder/GetPageByShopIdAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/PurchaseOder/GetPageByShopIdWhereQueryAsync")]
        ITask<ResultModel<PagedList<PurchaseOderDto>>> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, int storeId, string oderNo);

        [HttpDelete("/api/PurchaseOder/DeleteAsync")]
        ITask<ResultModel<PurchaseOderDto>> DeleteAsync(int id);

        [HttpDelete("/api/PurchaseOder/BatchDeleteAsync")]
        ITask<ResultModel<PurchaseOderDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PurchaseOder/InsertAsync")]
        ITask<ResultModel<PurchaseOderCreateDto>> InsertAsync([JsonContent] PurchaseOderCreateDto model);

        [HttpPut("/api/PurchaseOder/UpdateAsync")]
        ITask<ResultModel<PurchaseOderUpdateDto>> UpdateAsync([JsonContent] PurchaseOderUpdateDto model);

        [HttpPatch("/api/PurchaseOder/PatchAsync")]
        ITask<ResultModel<PurchaseOderDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PurchaseOderUpdateDto> doc);
    }
}
