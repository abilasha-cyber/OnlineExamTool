using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssessmentTool.Startup))]
namespace AssessmentTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
