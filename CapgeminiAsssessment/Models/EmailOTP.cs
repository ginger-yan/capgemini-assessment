using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapgeminiAsssessment.Models
{
    public class EmailOTP
    {
        public EmailOTP(string otp, DateTime expiration, int attempts = 0)
        {
            OTP = otp;
            Expiration = expiration;
            Attempts = attempts;
        }

        public string OTP { get; set; }
        public DateTime Expiration { get; set; }
        public int Attempts { get; set; }
    }
}
