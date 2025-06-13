using Microsoft.Extensions.Logging;
using Quote.Application.Core.Abstractions.Services;
using Quote.Domain.Core.Abstractions;

namespace Quote.Infrastructure.Services;

public class QuoteEmailService(
    IEmailService emailSerive, 
    ILogger<QuoteEmailService> logger)
    : IQuoteEmailService
{

    private readonly IEmailService _emailSerive = emailSerive;
    private readonly ILogger<QuoteEmailService> _logger = logger;


    public async Task SendDailyQuoteEmailAsync(string email, string author, string text , CancellationToken cancellationToken = default)
    {
        var subject = "Your daily Quotes";
        var body = GenerateQuoteEmailBody(author,text);

        _logger.LogInformation("Daily Quote sent to {Email}", email);
        await _emailSerive.SendEmailAsync(email, subject, body, cancellationToken);
    }
    

    public async Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var subject = "Welcome to Daily Quotes";
        var body = GenerateWelcomeEmailBody();

        await _emailSerive.SendEmailAsync(email, subject, body, cancellationToken);
        _logger.LogInformation("Welcome email sent to {Email}", email);
    }

    private static string GenerateQuoteEmailBody(string author, string text)
    {
        return $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='utf-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Your Daily Quote</title>
            <style>
                body {{ font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f4f4f4; }}
                .container {{ max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                .quote {{ font-size: 18px; font-style: italic; color: #333; line-height: 1.6; margin: 20px 0; text-align: center; }}
                .author {{ font-size: 16px; color: #666; text-align: right; margin-top: 15px; }}
                .category {{ display: inline-block; background-color: #007bff; color: white; padding: 5px 10px; border-radius: 15px; font-size: 12px; margin-top: 20px; }}
                .footer {{ margin-top: 30px; font-size: 12px; color: #888; text-align: center; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1 style='color: #333; text-align: center;'>Your Daily Quote</h1>
                
                <div class='quote'>
                    ""{text}""
                </div>
                
                <div class='author'>
                    — {author}
                </div>
                
        
                    <div class='footer'>
                        <p>Thank you for subscribing to Daily Quotes!</p>
                        <p>If you no longer wish to receive these emails, you can unsubscribe at any time.</p>
                    </div>
                </div>
            </body>
            </html>";
    }
    private static string GenerateWelcomeEmailBody()
    {
        return @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Welcome to Daily Quotes</title>
                <style>
                    body { font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f4f4f4; }
                    .container { max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
                    .welcome-text { font-size: 16px; color: #333; line-height: 1.6; }
                    .footer { margin-top: 30px; font-size: 12px; color: #888; text-align: center; }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1 style='color: #333; text-align: center;'>Welcome to Daily Quotes!</h1>
                    
                    <div class='welcome-text'>
                        <p>Thank you for subscribing to our daily quote service!</p>
                        <p>You'll receive inspiring quotes delivered to your inbox every day. Each quote is carefully selected to brighten your day and provide motivation.</p>
                        <p>We're excited to have you on board!</p>
                    </div>
                    
                    <div class='footer'>
                        <p>Best regards,<br>The Daily Quotes Team</p>
                    </div>
                </div>
            </body>
            </html>";
         }
    
    
    
    
}