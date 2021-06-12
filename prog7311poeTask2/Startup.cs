using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(prog7311poeTask2.Startup))]
namespace prog7311poeTask2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
