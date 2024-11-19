using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CapgeminiAsssessment.Services
{
    public class EmailService
    {
        private List<string> _whitelistedEmails = [];
        private readonly string smtpHost = "192.168.1.1";
        private readonly int smtpPort = 80;
        private readonly string smtpUsername = "hello";
        private readonly string smtpPassword = "world";

        public EmailService(List<string> whitelistedEmails)
        {
            _whitelistedEmails = whitelistedEmails;
        }

        public bool SendEmail(string toEmail, string otp)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpHost))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Port = smtpPort;

                    // Mock email send
                    // If the email recipients does not in predefined email list, return false; otherwise return true;
                    // MailMessage mailMessage = new MailMessage(smtpUsername, toEmail, Constants.EMAIL_SUBJECT, string.Format(Constants.EMAIL_BODY, otp));
                    // smtpClient.Send(mailMessage);
                    return _whitelistedEmails.Any(email => email.Equals(toEmail));
                }
            }
            catch (Exception ex)
            {
                // Handle logging of the exception as needed
                return false;
            }
        }
    }
}
