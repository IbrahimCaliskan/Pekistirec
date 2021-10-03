using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pekistirec.Engine.AspNetIdentity.Startup))]
namespace Pekistirec.Engine.AspNetIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
