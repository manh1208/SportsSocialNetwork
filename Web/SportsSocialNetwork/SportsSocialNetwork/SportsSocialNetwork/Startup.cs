using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using SportsSocialNetwork.Models.Hubs.IdProvider;

[assembly: OwinStartupAttribute(typeof(SportsSocialNetwork.Startup))]
namespace SportsSocialNetwork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var idProvider = new CustomUserIdProvider();

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
