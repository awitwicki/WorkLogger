using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using WorkLogger.Domain.ConfigModels;

namespace WorkLogger.Services;

public class EmailSender : IEmailSender
{
    private readonly ConfigModel _config;

    public EmailSender(ConfigModel config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("WorkLogger", _config.EmailFrom));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();
        
        await client.ConnectAsync(_config.SmtpHost, _config.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_config.SmtpUsername, _config.SmtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
