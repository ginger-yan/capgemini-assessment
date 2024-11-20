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

5. Limited OTP Attempts:
Users have a maximum of 10 attempts to enter the correct OTP. After 10 failed attempts, the OTP will no longer be valid, and the system will return an OTP fail error.
The application will track the number of failed attempts for each user and stop after 10 incorrect entries.



Test Framework:
xUnit is used to structure and run the test. 
The EmailOTPServiceTest class is created to test the functionality of the EmailOTPService module.

Test 1: generate_OTP_email_ValidEmail_ShouldReturnStatusEmailOk
- Objective: Verify that the system correctly generates and sends an OTP when a valid email is provided.
- Input: A valid email address
- Expected Outcome: The OTP is generated.The service returns STATUS_EMAIL_OK.

Test 2: generate_OTP_email_InvalidEmail_ShouldReturnStatusEmailInvalid
- Objective: Ensure that emails with invalid domains, the service returns STATUS_EMAIL_INVALID.
- Input: An email address with invalid domain
- Expected Outcome: The service return STATUS_EMAIL_INVALID.

Test 3: generate_OTP_email_EmailSendingFails_ShouldReturnStatusEmailFail
- Objective: Ensure that when the email is invalid (not within the predefined list), the service return STATUS_EMAIL_FAIL.
- Input: An invalid email
- Expected Outcome: The service return STATUS_EMAIL_FAIL.

Test 4: check_OTP_CorrectOTP_ShouldReturnStatusOtpOk
- Objective: Ensure OTP verification for a correct OTP.
- Input: The correct OTP
- Expected Outcome: The OTP matches, and the service returns STATUS_OTP_OK.

Test 5: check_OTP_IncorrectOTP_ShouldReturnStatusOtpFail
- Objective: Verify that when an incorrect OTP is entered, the system returns a failure status STATUS_OTP_FAIL.
- Input: An incorrect OTP
- Expected Outcome: The service should return STATUS_OTP_FAIL if the OTP doesn't match.

Test 6: check_OTP_MaxAttemptsExceeded_ShouldReturnStatusOtpFail
- Objective: Verify that the system enforces a maximum retry limit and returns STATUS_OTP_FAIL after 10 failed attempts.
- Input: 10 failed attempts of incorrect OTP
- Expected Outcome: After 10 failed attempts, the service returns STATUS_OTP_FAIL on the 11th attempt

Test 7: check_OTP_ExpiredOtp_ShouldReturnStatusOtpTimeout
- Objective: Verify that OTP expiration is handled correctly, returning STATUS_OTP_TIMEOUT.
- Input: An OTP that has expired.
- Expected Outcome: Verify that after expiration (1 min), the OTP validation returns STATUS_OTP_TIMEOUT.