using Infrastructure.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UsersEdit.CustomAttributes.Error
{
    public class CustomHandleErrorWithElmahAttribute : HandleErrorAttribute
    {
        private ILogger logger;

        public CustomHandleErrorWithElmahAttribute()
        {
            logger = null;
        }

        public CustomHandleErrorWithElmahAttribute(ILogger logger)
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult("~/Error/ApplicationError");
            //elmah log
            Elmah.ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);

            if (logger != null)
                logger.Error("Application error (inside the controller)");
        }
    }
}