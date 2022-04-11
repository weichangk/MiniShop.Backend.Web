using MiniShop.Backend.Model.Enums;
using System;

namespace MiniShop.Backend.Web.Code
{
    public interface IUserInfo
    {
        string IdToken { get; }
        string AccessToken { get; }
        string RefreshToken { get; }
        string UserName { get; }
        EnumRole Rank { get; }
        Guid ShopId { get; }
        Guid StoreId { get; }
        string PhoneNumber { get; }
        string Email { get; }
        bool IsFreeze { get; }
        DateTime CreatedTime { get; }
    }
}
