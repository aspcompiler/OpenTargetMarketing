using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TargetMarketingWeb.Startup))]
namespace TargetMarketingWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
