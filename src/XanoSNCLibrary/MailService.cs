using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    internal class MailService
    {
        #region Singleton
        private static MailService instance = new MailService();
        private MailService() { }

        internal static MailService Instance { get { return instance; } }
        #endregion

        internal Task SendEmailAsync(string toAddress, string subject, string message, MailPriority priority = MailPriority.Normal)
        {
            var mail = new MailMessage(ConfigurationManager.AppSettings["EmailFromAddress"], toAddress);
            var client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = ConfigurationManager.AppSettings["EmailHost"];
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = message;
            mail.Priority = priority;
            client.SendAsync(mail, null);
            return Task.FromResult(0);
        }
    }
}
