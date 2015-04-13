using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infrastructure.Extensions.Html
{
    public static class AExtensions
    {
        public static MvcHtmlString EmailLink(this HtmlHelper helper, string email, string tittle)
        {
            return new MvcHtmlString("<a href='mailto:" + email + "'>" + tittle + "</a>");
        }
    }
}