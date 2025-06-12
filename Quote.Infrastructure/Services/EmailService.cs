using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quote.Application.Core.Abstractions.Services;


namespace Quote.Infrastructure.Services;

public class EmailService : IEmailService,IDisposable
{

    private readonly ILogger<EmailService> _logger;
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;
    private readonly string _fromName;


    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _logger = logger;

        var smtpSettings = configuration.GetSection("SmtpSettings");
        _fromEmail = smtpSettings["FromEmail"] ?? throw new InvalidOperationException("FromEmail is required");
        _fromName = smtpSettings["FromName"] ?? "Daily Quotes";

        _smtpClient = new SmtpClient
        {
            Host = smtpSettings["Host"] ?? throw new InvalidOperationException("SMTP host is required"),
            Port = int.Parse(smtpSettings["Port"] ?? "587"),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true"),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                smtpSettings["Username"] ?? throw new InvalidOperationException("SMTP Username is required"),
                smtpSettings["Password"] ?? throw new InvalidOperationException("SMTP Password is required")
            )
        };
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        try
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            message.To.Add(new MailAddress(to));

            _logger.LogInformation("Sending email to {Email} with subjects : {Subject}", to, subject);
            await _smtpClient.SendMailAsync(message, cancellationToken);

            _logger.LogInformation("Email successfully sent to {Email}", to);

        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "SMTP error occured while sending email to {Email}: {Error}", to, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SMTP error occured while sending email to {Email}: {Error}", to, ex.Message);
            throw;
        }

    }

    public void Dispose()
    {
        _smtpClient?.Dispose();
    }
    
}