using MiniShop.Backend.Web.Code;
using MiniShop.Platform.Model.Dtos;
using Weick.Orm.Core.Result;
using System.Collections.Generic;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace MiniShop.Backend.Web.HttpApis
{
    [MiniShopAdminApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IRenewPackageApi : IHttpApi
    {
        [HttpGet("/api/Admin/RenewPackage")]
        ITask<ResultModel<List<RenewPackageDto>>> GetRenewPackagesAsync();

        [HttpGet("/api/Admin/RenewPackage/GetById")]
        ITask<ResultModel<RenewPackageDto>> GetRenewPackageByIdAsync(int id);
    }
}
