using Microsoft.Extensions.Logging;
using Quote.Application.Core.Abstractions.Services;
using Quote.Domain.Core.Abstractions;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;

namespace Quote.Infrastructure.Services;

public class QuoteEmailService(
    IEmailService emailSerive, 
    ILogger<QuoteEmailService> logger,
    IWebHostEnvironment webHostEnvironment)
    : IQuoteEmailService
{

    private readonly IEmailService _emailSerive = emailSerive;
    private readonly ILogger<QuoteEmailService> _logger = logger;


    public async Task SendDailyQuoteEmailAsync(string email, string author, string text, CancellationToken cancellationToken = default)
    {
        var subject = "Your Daily Quote";
        var body = await GenerateQuoteEmailBodyAsync(author, text);

        _logger.LogInformation("Daily Quote sent to {Email}", email);
        await emailSerive.SendEmailAsync(email, subject, body, cancellationToken);
    }

    public async Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var subject = "Welcome to Daily Quotes";
        var body = await GenerateWelcomeEmailBodyAsync();

        await emailSerive.SendEmailAsync(email, subject, body, cancellationToken);
        _logger.LogInformation("Welcome email sent to {Email}", email);
    }

    private async Task<string> GenerateQuoteEmailBodyAsync(string author, string text)
    {
        var templatePath = Path.Combine(webHostEnvironment.WebRootPath, "templates", "quote-email.html");
        var template = await File.ReadAllTextAsync(templatePath);
        

        template = template.Replace("{author}", author);
        template = template.Replace("{text}", text);
        
        return template;
    }

    private async Task<string> GenerateWelcomeEmailBodyAsync()
    {
        var templatePath = Path.Combine(webHostEnvironment.WebRootPath, "templates", "welcome-email.html");
        return await File.ReadAllTextAsync(templatePath);
    }
}