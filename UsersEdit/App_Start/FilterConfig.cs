using Infrastructure.Logging.Concrete.Nlog;
using Ninject;
using NLog.Config;
using System.Web;
using System.Web.Mvc;
using UsersEdit.App_Start;
using UsersEdit.CustomAttributes.Error;

namespace UsersEdit
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("utc_date", typeof(UtcDateRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("web_variables", typeof(WebVariablesRenderer));

            IKernel kernel = new StandardKernel(new LoggerModule()); 
            filters.Add(new CustomHandleErrorWithElmahAttribute(kernel.Get<ILoggerFactory>().CreateDefaultLogger()));
        }
    }
}
