using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hackagram.Startup))]
namespace Hackagram
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
