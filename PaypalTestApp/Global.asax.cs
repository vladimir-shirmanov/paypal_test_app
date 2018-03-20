using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PayPal.Api;
using PaypalTestApp.Helpers;

namespace PaypalTestApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var config = ConfigManager.Instance.GetProperties();

            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            CacheManager.Instance.Add(Constants.TOKEN_KEY, accessToken);
        }
    }
}
