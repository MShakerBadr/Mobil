using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MLP.Web.Evaluation.Startup))]
namespace MLP.Web.Evaluation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
