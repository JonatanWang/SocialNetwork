using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheSocialNetwork.Startup))]
namespace TheSocialNetwork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
