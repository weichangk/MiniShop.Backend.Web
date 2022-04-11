using System.Threading.Tasks;
using MiniShop.Backend.Web.Code;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace MiniShop.Backend.Web
{
    public class SetAccessTokenAttribute : ApiActionAttribute
    {
        public override Task OnRequestAsync(ApiRequestContext context)
        {
            var userInfo = (IUserInfo)context.HttpContext.ServiceProvider.GetService(typeof(IUserInfo));
            context.HttpContext.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userInfo.AccessToken);
            return Task.CompletedTask;
        }
    }
}
