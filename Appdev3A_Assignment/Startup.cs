using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Appdev3A_Assignment.Startup))]
namespace Appdev3A_Assignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
