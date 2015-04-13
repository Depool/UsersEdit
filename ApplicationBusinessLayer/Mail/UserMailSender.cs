using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using ApplicationRepository.Interface;
using Infrastructure.Mail;

namespace ApplicationBusinessLayer.Mail
{
    public class UserMailSender
    {
        public static void SendMessagesFromQueue(IMailMessageRepository mailRep)
        {
            IEnumerable<ApplicationRepository.Models.MailMessage> messages = mailRep.GetAll();

            foreach (ApplicationRepository.Models.MailMessage messageForAdmin in messages)
            {
                MailSender.SendMail(messageForAdmin);
            }

            for (int i = messages.Count() - 1; i >= 0; --i)
                mailRep.Delete(messages.ElementAt(i));

            mailRep.SaveChanges();
        }
    }
}
