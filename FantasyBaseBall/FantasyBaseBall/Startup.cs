using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FantasyBaseBall.Startup))]
namespace FantasyBaseBall
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
