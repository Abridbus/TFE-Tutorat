using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tutorat.Startup))]
namespace Tutorat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
