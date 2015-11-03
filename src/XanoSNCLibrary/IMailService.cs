using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace XanoSNCLibrary
{
    public interface IMailService
    {
        Task SendEmailAsync(string toAddress, string subject, string message, MailPriority priority = MailPriority.Normal);
    }
}
