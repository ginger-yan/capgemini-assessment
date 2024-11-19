Assumptions for implementing the Email OTP module:
1. Application Type: 
The application is a console application. This means that the module will run in a terminal or command-line interface and there will be no graphical user interface (GUI). The user interacts with the system via console input/output.

2. Email Service Mocking:
The application does not have an actual email server set up. There is no real emails will be sent. The SendEmail method is mocked to simulate email sending. This allows for testing the OTP generation and checking functionality without an actual email server.

3. Predefined Email List:
The OTP can only be sent to email addresses with specific domain ".dso.org.sg". Otherwise, the email will treat as invalid and will not receive an OTP. The list of whitelisted emails will be predefined, and only those emails can receive OTPs. This list will be passed into the EmailService when the application is initialized.

4. OTP Expiration:
The OTP is valid only for a short duration (1 minute). After this time, the OTP will expire, and any attempts to check the OTP after expiration will result in a timeout error.
Once the OTP has expired, the application will remove the OTP from its store, and any future attempts to verify it should return the OTP timeout error.

5.Limited OTP Attempts:
Users have a maximum of 10 attempts to enter the correct OTP. After 10 failed attempts, the OTP will no longer be valid, and the system will return an OTP fail error.
The application will track the number of failed attempts for each user and stop after 10 incorrect entries.



Describe how you would test your module.
xUnit is used to make unit testing easy and effective.
The EmailOTPServiceTest class is created to test the functionality of the EmailOTPService module.

Test 1: generate_OTP_email_ValidEmail_ShouldReturnStatusEmailOk
Objective: Verify that a valid email address generates a new OTP and returns STATUS_EMAIL_OK.
Test Flow:
A valid email from the predefined list is passed to the generate_OTP_email method.
The test asserts that the OTP generation is successful and that the status returned is STATUS_EMAIL_OK.

Test 2: generate_OTP_email_InvalidEmail_ShouldReturnStatusEmailInvalid
Objective: Ensure that an invalid email domain returns STATUS_EMAIL_INVALID.
Test Flow:
An email address outside of the whitelisted domain is passed to generate_OTP_email.
The test asserts that the result is STATUS_EMAIL_INVALID.

Test 3: generate_OTP_email_EmailSendingFails_ShouldReturnStatusEmailFail
Objective: Verify that when the email fails to be sent, the result is STATUS_EMAIL_FAIL.
Test Flow:
An invalid email address is passed to generate_OTP_email.
The test asserts that the result is STATUS_EMAIL_FAIL.

Test 4: check_OTP_CorrectOTP_ShouldReturnStatusOtpOk
Objective: Test OTP verification for a correct OTP.
Test Flow:
Generate an OTP using a valid email address.
Retrieve the correct OTP from the store.
Verify that entering the correct OTP results in STATUS_OTP_OK.
The test also asserts that the isOTPMatched value is true.

Test 5: check_OTP_IncorrectOTP_ShouldReturnStatusOtpFail
Objective: Ensure incorrect OTP results in STATUS_OTP_FAIL.
Test Flow:
Generate an OTP and retrieve it from the store.
Modify the OTP to make it incorrect.
Verify that the incorrect OTP results in STATUS_OTP_FAIL.

Test 6: check_OTP_MaxAttemptsExceeded_ShouldReturnStatusOtpFail
Objective: Verify that the system enforces a maximum retry limit and returns STATUS_OTP_FAIL after 10 failed attempts.
Test Flow:
Generate an OTP for a valid email.
Simulate 10 incorrect attempts.
Assert that STATUS_OTP_FAIL is returned for each attempt.
On the 11th attempt, ensure the system still returns STATUS_OTP_FAIL.

Test 7: check_OTP_ExpiredOtp_ShouldReturnStatusOtpTimeout
Objective: Verify that OTP expiration is handled correctly, returning STATUS_OTP_TIMEOUT.
Test Flow:
Generate an OTP.
Simulate OTP expiration by adding a delay.
Verify that after expiration, the OTP validation returns STATUS_OTP_TIMEOUT.