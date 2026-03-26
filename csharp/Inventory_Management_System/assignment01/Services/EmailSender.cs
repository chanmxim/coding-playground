using assignment01.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace assignment01.Services;

public class EmailSender : IEmailSender
{
    private readonly string _sendGridApiKey;
    private readonly ErrorLog _errorLog;

    public EmailSender(IConfiguration configuration, ErrorLog errorLog)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"]
                          ?? throw new ArgumentException("SendGrid Key is missing");
        _errorLog = errorLog;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("kalllak17@gmail.com",
                "PM Tool Default Sender"); // enter domain email from SendGrid website
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "Welcome to PM Tool Inc", htmlMessage);

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Body.ReadAsStringAsync();
                _errorLog.LogError(new Exception(errorMessage));
            }
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw;
        }
    }
}