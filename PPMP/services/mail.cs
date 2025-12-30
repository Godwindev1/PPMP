using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _port = 587;
    private readonly string _fromEmail;
    private readonly string _password;

    public EmailService(IConfiguration configuration)
    {
        _password = configuration["APP_PASSWORD"];
        _fromEmail = configuration["EMAIL_SERVER"];
    }
    
    private SmtpClient CreateSmtpClient()
    {
        return new SmtpClient(_smtpServer, _port)
        {
            Credentials = new NetworkCredential(_fromEmail, _password),
            EnableSsl = true
        };
    }

    public void SendEmail(string to, string subject, string body, bool isHtml = false)
    {
        var mail = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml,
        };

        mail.To.Add(to);

        using var smtp = CreateSmtpClient();
        smtp.Send(mail);
    }

    public void SendLinkEmail(
        string to,
        string subject,
        string message,
        string buttonText,
        string linkUrl
    )
    {
        string htmlBody = $@"
        <html>
        <body style='font-family: Arial, sans-serif;'>
            <p>{message}</p>

            <a href='{linkUrl}'
               style='
                 display:inline-block;
                 padding:12px 20px;
                 background-color:#3B4CB8;
                 color:white;
                 text-decoration:none;
                 border-radius:5px;
                 font-weight:bold;'>
               {buttonText}
            </a>

            <p style='margin-top:20px; font-size:12px; color:#777;'>
                If the button doesn’t work, copy and paste this link:<br/>
                {linkUrl}
            </p>
        </body>
        </html>";

        SendEmail(to, subject, htmlBody, isHtml: true);
    }
}

