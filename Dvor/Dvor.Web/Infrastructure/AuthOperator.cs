using Dvor.Web.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dvor.Web.Infrastructure
{
    public class AuthOperator
    {
        public static ApiAuthSettings AddApiAuthSettings(IConfiguration configuration, IServiceCollection services)
        {
            var authSettingsSection = configuration.GetSection(nameof(ApiAuthSettings));
            services.Configure<ApiAuthSettings>(authSettingsSection);

            return authSettingsSection.Get<ApiAuthSettings>();
        }

        public static CookieAuthSettings AddCookieAuthSettings(IConfiguration configuration,
            IServiceCollection services)
        {
            var settingsSection = configuration.GetSection(nameof(CookieAuthSettings));
            services.Configure<CookieAuthSettings>(settingsSection);

            return settingsSection.Get<CookieAuthSettings>();
        }
    }
}