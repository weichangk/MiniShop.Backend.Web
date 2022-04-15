using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniShop.Backend.Web.Code;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using WebApiClientCore;

namespace MiniShop.Backend.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)      
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserInfo, UserInfo>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, config => {
                config.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.Authority = _configuration["IdsConfig:Authority"];
                config.ClientId = _configuration["IdsConfig:ClientId"];
                config.ClientSecret = _configuration["IdsConfig:ClientSecret"];
                config.SaveTokens = true;
                config.ResponseType = _configuration["IdsConfig:ResponseType"];
                config.RequireHttpsMetadata = false;
                //config.SignedOutCallbackPath = "/Home/Index";

                // two trips to load claims in to the cookie
                // but the id token is smaller !
                config.GetClaimsFromUserInfoEndpoint = true;

                // configure scope
                config.Scope.Clear();
                var scopes = _configuration["IdsConfig:Scopes"];
                var scopeArray = scopes.Split(" ");
                for (int i = 0; i < scopeArray.Length; i++)
                {
                    config.Scope.Add(scopeArray[i]);
                }
            });

            services.AddHttpClient();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // 配置cookie策略
            services.Configure<CookiePolicyOptions>(options =>
            {
                //https://docs.microsoft.com/zh-cn/aspnet/core/security/samesite?view=aspnetcore-3.1&viewFallbackFrom=aspnetcore-3
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            });

            //添加HttpClient相关
            var miniShopApiTypes = typeof(Startup).Assembly.GetTypes()
                        .Where(type => type.IsInterface
                        && ((System.Reflection.TypeInfo)type).ImplementedInterfaces != null
                        && type.GetInterfaces().Any(a => a.FullName == typeof(IHttpApi).FullName)
                        && type.IsDefined(typeof(MiniShopApiAttribute), false));
            foreach (var type in miniShopApiTypes)
            {
                services.AddHttpApi(type);
                services.ConfigureHttpApi(type, o =>
                {
                    o.HttpHost = new Uri(_configuration["MiniShopApi:Urls"]);
                    // 符合国情的不标准时间格式，有些接口就是这么要求必须不标准
                    o.JsonSerializeOptions.Converters.Add(new WebApiClientCore.Serialization.JsonConverters.JsonDateTimeConverter("yyyy-MM-dd HH:mm:ss"));
                });
            }

            var miniShopAdminApiTypes = typeof(Startup).Assembly.GetTypes()
                        .Where(type => type.IsInterface
                        && ((System.Reflection.TypeInfo)type).ImplementedInterfaces != null
                        && type.GetInterfaces().Any(a => a.FullName == typeof(IHttpApi).FullName)
                        && type.IsDefined(typeof(MiniShopAdminApiAttribute), false));
            foreach (var type in miniShopAdminApiTypes)
            {
                services.AddHttpApi(type);
                services.ConfigureHttpApi(type, o =>
                {
                    o.HttpHost = new Uri(_configuration["MiniShopAdminApi:Urls"]);
                    // 符合国情的不标准时间格式，有些接口就是这么要求必须不标准
                    o.JsonSerializeOptions.Converters.Add(new WebApiClientCore.Serialization.JsonConverters.JsonDateTimeConverter("yyyy-MM-dd HH:mm:ss"));
                });
            }

            services.AddScoped(typeof(RefreshAccessTokenAttribute));

            //添加AutoMapper
            services.AddAutoMapper(typeof(Model.Dto.Profiles.AutoMapperProfiles).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add this before any other middleware that might write cookies
            app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}
