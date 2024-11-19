using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapgeminiAsssessment
{
    public class Constants
    {
        // Email-related response codes
        public const string STATUS_EMAIL_OK = "STATUS_EMAIL_OK";
        public const string STATUS_EMAIL_FAIL = "STATUS_EMAIL_FAIL";
        public const string STATUS_EMAIL_INVALID = "STATUS_EMAIL_INVALID";

        // OTP-related response codes
        public const string STATUS_OTP_OK = "STATUS_OTP_OK";
        public const string STATUS_OTP_FAIL = "STATUS_OTP_FAIL";
        public const string STATUS_OTP_TIMEOUT = "STATUS_OTP_TIMEOUT";

        public const string EMAIL_SUBJECT = "Your OTP Code";
        public const string EMAIL_BODY = "Your OTP Code is {0}. The code is valid for 1 minute.";

    }

}
