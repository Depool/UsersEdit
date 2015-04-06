using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging
{
    public static class LogUtility
    {
        public static string BuildExceptionMessage(Exception ex)
        {
            Exception logException = ex;
            if (ex.InnerException != null)
            logException = ex.InnerException;

            string strErrorMsg = String.Empty;

            //strErrorMsg += Environment.NewLine + "Error in Path :" + System.Web.HttpContext.Current.Request.Path;

            //strErrorMsg += Environment.NewLine + "Raw Url :" + System.Web.HttpContext.Current.Request.RawUrl;
            
            strErrorMsg += Environment.NewLine + "Message :" + logException.Message;
            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;
            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;
            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;
            return strErrorMsg;
        }
    }
}
