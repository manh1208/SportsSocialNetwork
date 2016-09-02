using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SportsSocialNetwork.Startup))]
namespace SportsSocialNetwork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
