using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UsersEdit.Startup))]
namespace UsersEdit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
