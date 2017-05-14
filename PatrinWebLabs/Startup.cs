using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PatrinWebLabs.Startup))]
namespace PatrinWebLabs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
