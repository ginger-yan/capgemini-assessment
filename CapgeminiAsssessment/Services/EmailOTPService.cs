using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapgeminiAsssessment.Models;

namespace CapgeminiAsssessment.Services
{
    public class EmailOTPService
    {
        private static Dictionary<string, EmailOTP> _otpStore = [];
        private readonly int _expiredAfterMin = 1;
        private readonly int _maxRetryCount = 10;
        private readonly EmailService _emailService;

        public EmailOTPService(EmailService emailService)
        {
            _emailService = emailService;
        }

        public string generate_OTP_email(string emailAddress, out string otp)
        {
            otp = string.Empty;

            if (!IsValidEmailDomain(emailAddress))
            {
                return Constants.STATUS_EMAIL_INVALID;
            }

            otp = GenerateRandomOTP();
            DateTime expirationTime = DateTime.Now.AddMinutes(_expiredAfterMin);
            _otpStore[emailAddress] = new EmailOTP(otp, expirationTime);

            string emailBody = string.Format(Constants.EMAIL_BODY, otp);
            bool emailSent = _emailService.SendEmail(emailAddress, emailBody);

            if (emailSent)
                Console.WriteLine(emailBody);

            return emailSent ? Constants.STATUS_EMAIL_OK : Constants.STATUS_EMAIL_FAIL;
        }

        public bool check_OTP(string emailAddress, string enteredOTP, out string responseCode)
        {
            responseCode = string.Empty;

            if (!_otpStore.ContainsKey(emailAddress))
                return false;

            EmailOTP otpDetails = _otpStore[emailAddress];

            if (DateTime.Now > otpDetails.Expiration)
            {
                _otpStore.Remove(emailAddress);
                responseCode = Constants.STATUS_OTP_TIMEOUT;
                return false;
            }

            if (enteredOTP == otpDetails.OTP)
            {
                _otpStore.Remove(emailAddress);
                responseCode = Constants.STATUS_OTP_OK;
                return true;
            }

            if (otpDetails.Attempts >= _maxRetryCount)
            {
                responseCode = Constants.STATUS_OTP_FAIL;
                return false;
            }

            _otpStore[emailAddress] = new EmailOTP(otpDetails.OTP, otpDetails.Expiration, otpDetails.Attempts + 1);

            // If OTP is incorrect, allow retry and return failure
            responseCode = Constants.STATUS_OTP_FAIL;
            return false;
        }

        private bool IsValidEmailDomain(string emailAddress)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(emailAddress);
                return address.Host.EndsWith(".dso.org.sg", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private string GenerateRandomOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
