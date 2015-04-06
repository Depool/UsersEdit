using ActionMailer.Net.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UsersEdit.CustomAttributes.Authorization;

namespace UsersEdit.Controllers
{
    public class EmailController : MailerBase
    {
        //
        // GET: /Email/
        public EmailResult Registration(string message, string email)
        {
            To.Add(email);
            Subject = "Пользователь зарегистрирован";
            MessageEncoding = Encoding.UTF8;
            return Email("Registration", message);
        }

        public EmailResult Block(string message, string cause, string email)
        {
            To.Add(email);
            Subject = "Пользователь забанен";
            MessageEncoding = Encoding.UTF8;
            ViewBag.Cause = cause;
            return Email("Block", message);
        }

        public ContentResult EmailToText(EmailResult someMail)
        {
            using (var reader = new StreamReader(someMail.Mail.AlternateViews[0].ContentStream))
            {
                var content = reader.ReadToEnd();
                return Content(content);
            }
        }
	}
}