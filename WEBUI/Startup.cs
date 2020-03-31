using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WEBUI.Startup))]
namespace WEBUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
