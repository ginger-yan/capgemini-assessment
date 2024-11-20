using CapgeminiAsssessment;
using CapgeminiAsssessment.Services;
using CapgeminiAsssessment.Models;
using static System.Net.WebRequestMethods;

namespace CapgeminiAssessment.Test
{
    public class EmailOTPServiceTest
    {
        private readonly EmailService _emailService;
        private readonly EmailOTPService _emailOTPService;

        public EmailOTPServiceTest()
        {
            // Initialize the EmailService with a predefined list of whitelisted emails
            _emailService = new EmailService(["user@test1.dso.org.sg", "def@test2.dso.org.sg", "ghi@test3.dso.org.sg,user@dso.org.sg"]);

            // Initialize the EmailOTPService with the actual EmailService instance
            _emailOTPService = new EmailOTPService(_emailService);

            // Clear OTP store to avoid test contamination
            ClearOtpStore();
        }

        // Method to access and clear the static _otpStore from EmailOTPService using reflection
        private void ClearOtpStore()
        {
            var otpStoreField = typeof(EmailOTPService).GetField("_otpStore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (otpStoreField != null)
            {
                otpStoreField.SetValue(null, new Dictionary<string, EmailOTP>());
            }
        }

        // Test for valid email generation
        [Fact]
        public void generate_OTP_email_ValidEmail_ShouldReturnStatusEmailOk()
        {
            // Arrange
            string validEmail = "user@test1.dso.org.sg";

            // Act
            string result = _emailOTPService.generate_OTP_email(validEmail, out string otp);

            // Assert
            Assert.Equal(Constants.STATUS_EMAIL_OK, result);
        }

        // Test for invalid email domain
        [Fact]
        public void generate_OTP_email_InvalidEmail_ShouldReturnStatusEmailInvalid()
        {
            // Arrange
            string invalidEmail = "user@example.com"; // Invalid domain

            // Act
            string result = _emailOTPService.generate_OTP_email(invalidEmail, out string otp);

            // Assert
            Assert.Equal(Constants.STATUS_EMAIL_INVALID, result);
        }

        // Test for failure to send email
        [Fact]
        public void generate_OTP_email_EmailSendingFails_ShouldReturnStatusEmailFail()
        {
            // Arrange
            string validEmail = "user@abc.dso.org.sg";

            // Act
            string result = _emailOTPService.generate_OTP_email(validEmail, out string otp);

            // Assert
            Assert.Equal(Constants.STATUS_EMAIL_FAIL, result);
        }

        // Test for OTP verification with correct OTP
        [Fact]
        public void check_OTP_CorrectOTP_ShouldReturnStatusOtpOk()
        {
            // Arrange
            string email = "user@test1.dso.org.sg";
            string result = string.Empty;

            // Generate OTP
            string otpGenerated = _emailOTPService.generate_OTP_email(email, out string otp);

            // Act
            bool isOTPMatched = _emailOTPService.check_OTP(email, otp, out result);

            // Assert
            Assert.Equal(Constants.STATUS_OTP_OK, result);  // Check if the result matches STATUS_OTP_OK
            Assert.True(isOTPMatched);  // Check if the OTP matches
        }

        // Test for OTP verification with incorrect OTP
        [Fact]
        public void check_OTP_IncorrectOTP_ShouldReturnStatusOtpFail()
        {
            // Arrange
            string email = "user@test1.dso.org.sg";
            string result = string.Empty;

            // Generate OTP
            string otpGenerated = _emailOTPService.generate_OTP_email(email, out string otp);

            // Ensure the incorrect OTP is definitely different by modifying the generated OTP
            string incorrectOtp = otp.Substring(0, otp.Length - 1) + "X";

            // Act
            bool isOTPMatched = _emailOTPService.check_OTP(email, incorrectOtp, out result);

            // Assert
            Assert.Equal(Constants.STATUS_OTP_FAIL, result);  // Check that the result is STATUS_OTP_FAIL
            Assert.False(isOTPMatched);
        }

        // Test for OTP verification exceeding 10 attempts
        [Fact]
        public void check_OTP_MaxAttemptsExceeded_ShouldReturnStatusOtpFail()
        {
            // Arrange
            string email = "user@test1.dso.org.sg";
            string result = string.Empty;
            bool isOTPMatched = false;

            // Generate OTP
            _emailOTPService.generate_OTP_email(email, out string otp);

            // Ensure the incorrect OTP is definitely different by modifying the generated OTP
            string incorrectOtp = otp.Substring(0, otp.Length - 1) + "X";

            // Act & Assert
            for (int i = 0; i < 10; i++)
            {
                isOTPMatched = _emailOTPService.check_OTP(email, incorrectOtp, out result);
                Assert.Equal(Constants.STATUS_OTP_FAIL, result);
                Assert.False(isOTPMatched);
            }

            // On the 11th attempt, the result should still be STATUS_OTP_FAIL
            isOTPMatched = _emailOTPService.check_OTP(email, incorrectOtp, out result);
            Assert.Equal(Constants.STATUS_OTP_FAIL, result);
            Assert.False(isOTPMatched);
        }

        // Test for OTP expiration
        [Fact]
        public void check_OTP_ExpiredOtp_ShouldReturnStatusOtpTimeout()
        {
            // Arrange
            string email = "user@test1.dso.org.sg";
            string result = string.Empty;

            // Generate OTP
            string otpGenerated = _emailOTPService.generate_OTP_email(email, out string otp);

            // Simulate expiration by advancing time
            System.Threading.Thread.Sleep(60000); // Sleep for 1 minute to expire OTP

            // Act
            bool isOTPMatched = _emailOTPService.check_OTP(email, otp, out result);

            // Assert
            Assert.Equal(Constants.STATUS_OTP_TIMEOUT, result);
            Assert.False(isOTPMatched);
        }
    }
}