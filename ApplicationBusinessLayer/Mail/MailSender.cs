using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using ApplicationRepository.Interface;

namespace ApplicationBusinessLayer.Mail
{
    public class MailSender
    {
        private static AppSettingsReader config = null;

        public static AppSettingsReader Config
        {
            get
            {
                if (config == null)
                    config = new AppSettingsReader();
                return config;
            }
        }

        public static void SendMail(ApplicationRepository.Models.MailMessage email)
        {
            System.Net.Mail.MailMessage mail = new MailMessage();
            mail.To.Add(email.To == string.Empty ? (string)Config.GetValue("EmailName", typeof(string)) : email.To);

            mail.From = new MailAddress(email.From == string.Empty ? (string)Config.GetValue("EmailName", typeof(string)) : email.From);
            mail.Body = email.Body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = (int)Config.GetValue("EmailPort", typeof(int)),
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential
                    ((string)Config.GetValue("EmailName", typeof(string)), 
                     (string)Config.GetValue("EmailPassword", typeof(string))),
               EnableSsl = true
            };

            smtp.Send(mail);
        }

        public static void SendMessagesFromQueue(IMailMessageRepository mailRep)
        {
            IEnumerable<ApplicationRepository.Models.MailMessage> messages = mailRep.GetAll();

            foreach (ApplicationRepository.Models.MailMessage messageForAdmin in messages)
            {
                SendMail(messageForAdmin);
            }

            for (int i = messages.Count() - 1; i >= 0; --i)
                mailRep.Delete(messages.ElementAt(i));

            mailRep.SaveChanges();
        }
    }
}
