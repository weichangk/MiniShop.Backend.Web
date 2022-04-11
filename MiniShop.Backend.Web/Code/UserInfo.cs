using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MiniShop.Backend.Model.Enums;
using System;
using System.Linq;

namespace MiniShop.Backend.Web.Code
{
    public class UserInfo : IUserInfo
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserInfo(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string IdToken
        {
            get
            {
                var value = _contextAccessor?.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken).Result;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("IdToken Not Found");
                }
                return value;
            }
        }

        public string AccessToken
        {
            get
            {
                var value = _contextAccessor?.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("AccessToken Not Found");
                }
                return value;
            }
        }

        public string RefreshToken
        {
            get
            {
                var value = _contextAccessor?.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).Result;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("RefreshToken Not Found");
                }
                return value;
            }
        }

        public string UserName
        {
            get
            {
                var value = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.PreferredUserName)?.Value;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("UserName Not Found");
                }
                return value;
            }
        }

        public EnumRole Rank
        {
            get
            {
                var value = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "rank")?.Value;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Rank Not Found");
                }
                return (EnumRole)Enum.Parse(typeof(EnumRole), value);
            }
        }

        public Guid ShopId
        {
            get
            {
                var value = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "shopid")?.Value;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("ShopId Not Found");
                }
                return Guid.Parse(value);
            }
        }

        public Guid StoreId
        {
            get
            {
                var value = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "storeid")?.Value;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("StoreId Not Found");
                }
                return Guid.Parse(value);
            }
        }

        public string PhoneNumber
        {
            get
            {
                var value = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.PhoneNumber)?.Value;
                //if (value == null || string.IsNullOrWhiteSpace(value))
                //{
                //    throw new Exception("LoginPhone Not Found");
                //}
                return value;
            }
        }

        public string Email
        {
            get
            {
                var value = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value;
                //if (value == null || string.IsNullOrWhiteSpace(value))
                //{
                //    throw new Exception("Email Not Found");
                //}
                return value;
            }
        }

        public bool IsFreeze
        {
            get
            {
                var value = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "isfreeze")?.Value;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("IsFreeze Not Found");
                }
                return bool.Parse(value);
            }
        }

        public DateTime CreatedTime
        {
            get
            {
                var value = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "createdtime")?.Value;
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("CreatedTime Not Found");
                }
                return DateTime.Parse(value);
            }
        }
    }
}
