using System.Reflection.Metadata;
using CapgeminiAsssessment;
using CapgeminiAsssessment.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        List<string> whitelistedEmails = ["abc@test1.dso.org.sg", "def@test2.dso.org.sg", "ghi@test3.dso.org.sg"];

        EmailService emailService = new(whitelistedEmails);

        // Initialize EmailOTPModule
        EmailOTPService emailOTPService = new(emailService);

        // Step 1: Accept email address input from user
        string emailAddress = string.Empty;
        string response = string.Empty;
        bool emailValid = false;

        // Keep prompting the user for a valid email address
        while (!emailValid)
        {
            Console.WriteLine(@$"Predefined Email Address: {string.Join(",", whitelistedEmails)}");
            Console.WriteLine("Enter your email address (only .dso.org.sg domain allowed):");
            emailAddress = Console.ReadLine() ?? string.Empty;

            // Check email validity using EmailOTPModule
            response = emailOTPService.generate_OTP_email(emailAddress, out string otp);

            if (response == Constants.STATUS_EMAIL_OK)
                emailValid = true; // Email is valid, proceed with OTP generation

            Console.WriteLine(response);
        }

        // Step 2: Wait for user input to enter OTP (with timeout)
        string enteredOTP;
        bool otpVerified = false;
        DateTime startTime = DateTime.Now;

        // Loop to keep asking for OTP until success or timeout
        int attemptCount = 0;  // Track the number of attempts

        while (!otpVerified && attemptCount < 10)
        {
            Console.WriteLine("Please enter the OTP sent to your email:");
            enteredOTP = Console.ReadLine() ?? string.Empty;

            // Step 3: Verify OTP entered by the user
            otpVerified = emailOTPService.check_OTP(emailAddress, enteredOTP, out response);

            if (response == Constants.STATUS_OTP_OK)
            {
                otpVerified = true;  // OTP matched successfully
                Console.WriteLine(response);  // Output success message
            }
            else if (response == Constants.STATUS_OTP_FAIL)
            {
                attemptCount++;  // Increment the number of attempts
                Console.WriteLine(response);  // Output failure message
                if (attemptCount < 10)
                {
                    Console.WriteLine($"Incorrect OTP. You have {10 - attemptCount} attempt(s) left.");
                }
                else
                {
                    Console.WriteLine("Max attempts reached. Please request to send OTP again.");
                    break;  // Exit if max attempts reached
                }
            }
            else if (response == Constants.STATUS_OTP_TIMEOUT)
            {
                Console.WriteLine(response);  // Output timeout message
                break;  // Exit if OTP expired
            }
        }

        if (attemptCount >= 10)
        {
            Console.WriteLine("You have exceeded the maximum number of attempts.");
        }      
    }
}