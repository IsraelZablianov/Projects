using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MailSender
{
   public class MailSender
    {
        private static readonly MailAddress FromAddress = new MailAddress("ngauctionssite@gmail.com", "ngAuctions site");
        private const string Host = "smtp.gmail.com";
        private const string FromPassword = "orellidorisrael";

        public bool SendMail(MailInfo mailInfo)
        {
            try
            {
                var toAddress = new MailAddress(mailInfo.ReceiverAddress, mailInfo.ReceiverName);
                var smtp = CreateSmtpClient();
                SendMailMessage(mailInfo, toAddress, smtp);

                return true;
            }
            catch (SmtpException smtpException)
            {
                return false;
            }
        }

        private void SendMailMessage(MailInfo mailInfo, MailAddress toAddress, SmtpClient smtp)
        {
            using (var message = new MailMessage(FromAddress, toAddress))
            {
                message.Subject = mailInfo.Subject;
                message.Body = mailInfo.Body;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient
            {
                Host = Host,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(FromAddress.Address, FromPassword),
                Timeout = 20000
            };
        }
    }
}

