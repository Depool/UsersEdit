using ApplicationBusinessLayer.Mail;
using ApplicationRepository.Concrete.Entity;
using Infrastructure.Authentication.Interface;
using Infrastructure.Logging.Concrete.Nlog;
using Infrastructure.Logging.Interface;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using UsersEdit.App_Start;
using UsersEdit.Authentication;

namespace UsersEdit
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILogger logger;


        private Thread mailThread { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            mailThread = new Thread(new ThreadStart(threadFunc));
            mailThread.Start();
        }

        private static void threadFunc()
        {
            while (true)
            {
               var goMails = new Thread(new ThreadStart(runSendingMails));
               goMails.Start();
               goMails.Join();

               Thread.Sleep(60000);
            }
        }

        private static void runSendingMails()
        {
            IKernel kernel = new StandardKernel(new RepositoriesModule());
            UserMailSender.SendMessagesFromQueue(kernel.Get<IRepositoryFactory>().CreateMailMessageRepository());
        }

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    IKernel kernel = new StandardKernel(new RepositoriesModule());

                    IPrincipalService principalService = new UsersEditPrincipalService(kernel.Get<IRepositoryFactory>().CreateUserRepository(), 
                                                                                       new HttpContextWrapper(Context));
                    var user = principalService.GetCurrent();
                    Context.User = principalService.GetCurrent();
                }
            }
        }

        public void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            IKernel kernel = new StandardKernel(new LoggerModule());
            logger = kernel.Get<ILoggerFactory>().CreateDefaultLogger();

            logger.Error("HttpError (out of controller)");

            if (exception != null)
                Response.Redirect("~/Error/HttpError");
        }

    }
}
