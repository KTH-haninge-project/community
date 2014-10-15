using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Community.Startup))]
namespace Community
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
