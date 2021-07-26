using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MLP.Web.UI.Startup))]
namespace MLP.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
