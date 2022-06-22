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
    public interface IPosRegisterApi : IHttpApi
    {
        [HttpGet("/api/PosRegister/GetByIdAsync")]
        ITask<ResultModel<PosRegisterDto>> GetByIdAsync(int id);

        [HttpGet("/api/PosRegister/GetByCodeAsync")]
        ITask<ResultModel<PosRegisterDto>> GetByCodeAsync(string code);
   
        [HttpGet("/api/PosRegister/GetPageAsync")]
        ITask<ResultModel<PagedList<PosRegisterDto>>> GetPageAsync(int pageIndex, int pageSize);

        [HttpGet("/api/PosRegister/GetPageWhereQueryAsync")]
        ITask<ResultModel<PagedList<PosRegisterDto>>> GetPageWhereQueryAsync(int pageIndex, int pageSize, string code, string name);

        [HttpDelete("/api/PosRegister/DeleteAsync")]
        ITask<ResultModel<PosRegisterDto>> DeleteAsync(int id);

        [HttpDelete("/api/PosRegister/BatchDeleteAsync")]
        ITask<ResultModel<PosRegisterDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/PosRegister/InsertAsync")]
        ITask<ResultModel<PosRegisterCreateDto>> InsertAsync([JsonContent] PosRegisterCreateDto model);

        [HttpPut("/api/PosRegister/UpdateAsync")]
        ITask<ResultModel<PosRegisterUpdateDto>> UpdateAsync([JsonContent] PosRegisterUpdateDto model);

        [HttpPatch("/api/PosRegister/PatchAsync")]
        ITask<ResultModel<PosRegisterDto>> PatchAsync(int id, [JsonContent] JsonPatchDocument<PosRegisterUpdateDto> doc);
    }
}
