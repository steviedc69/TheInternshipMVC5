using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Internship.Startup))]
namespace Internship
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
