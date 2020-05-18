using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dvor.DAL.EF;
using Dvor.Web.Infrastructure;
using Dvor.Web.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace Dvor.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connection = _configuration.GetConnectionString("Connection");
            services.AddDbContext<DvorContext>(options => options.UseSqlServer(connection));

            var apiAuthSettings = AuthOperator.AddApiAuthSettings(_configuration, services);
            var cookieAuthSettings = AuthOperator.AddCookieAuthSettings(_configuration, services);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.SlidingExpiration = true;
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(cookieAuthSettings.ExpirationTimeInSeconds);
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(apiAuthSettings.Secret)),
                        ValidIssuer = apiAuthSettings.Issuer,
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization();

            MapperModule.Configure(services);

            services.AddLocalization(options => options.ResourcesPath = "App_LocalResources");

            services.AddMvc()
                .AddViewLocalization()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var builder = new ContainerBuilder();
            builder.RegisterModule<DIModule>();
            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "category",
                    "Category/{CategoryId}", new {controller = "Category", action = "Get"});

                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
