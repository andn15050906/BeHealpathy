using Contract.Helpers;
using Contract.Helpers.AppExploration;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Web;

namespace Infrastructure.Helpers.Email;

public sealed class EmailService
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly string _senderMail;
    private readonly string _senderPassword;
    private readonly string _senderName;

    private readonly IAppLogger _logger;
    private const short PORT = 587;



    private const string REGISTRATION_SUBJECT = "Healpathy - Registration Confirmation";
    private const string RELATIVE_REGISTRATION_TEMPLATE = @"\emailTemplates\confirmation.html";

    private const string PASSWORD_RESET_SUBJECT = "Healpathy - Password Reset";
    private const string RELATIVE_PASSWORD_RESET_TEMPLATE = @"\emailTemplates\passwordreset.html";

    private const string HABIT_EXPIRIED_SUBJECT = "Healpathy - Hãy cập nhật thói quen của bạn nào!!!";

    private const string TEMPLATE_NOTFOUND = "Email template not found";

    public EmailService(IWebHostEnvironment hostEnvironment, IOptions<EmailOptions> emailOptions, IOptions<AppInfoOptions> appInfo, IAppLogger logger)
    {
        _hostEnvironment = hostEnvironment;
        _senderMail = emailOptions.Value.SenderMail;
        _senderPassword = emailOptions.Value.SenderPassword;
        _senderName = appInfo.Value.AppName ?? "-App-";
        _logger = logger;
    }



    public async Task SendRegistrationEmail(string toAddress, string username, string link)
    {
        string path = _hostEnvironment.WebRootPath + RELATIVE_REGISTRATION_TEMPLATE;
        if (!File.Exists(path))
        {
            _logger.Warn("Email Template Not found");
            throw new Exception(TEMPLATE_NOTFOUND);
        }

        string template = File.ReadAllText(path);
        template = template.Replace("{username}", username).Replace("{app}", _senderName).Replace("{link}", link);

        await SendEmailAsync(toAddress, REGISTRATION_SUBJECT, template);
    }

    public async Task SendPasswordResetEmail(string toAddress, string link)
    {
        string path = _hostEnvironment.WebRootPath + RELATIVE_PASSWORD_RESET_TEMPLATE;
        if (!File.Exists(path))
            throw new Exception(TEMPLATE_NOTFOUND);

        string template = File.ReadAllText(path);
        template = template.Replace("{link}", link).Replace("{app}", _senderName);

        await SendEmailAsync(toAddress, PASSWORD_RESET_SUBJECT, template);
    }

    public async Task SendHabitExpiredWarning(string toAddress, string habitTitle, string expiryDate, string tagColor)
    {
        string htmlTemplate = $@"
        <!DOCTYPE html>
        <html lang=""vi"">
        <head><meta charset=""UTF-8""><title>Cảnh báo thói quen sắp hết hạn</title></head>
        <body style=""margin:0;padding:0;font-family:Arial,sans-serif;background:#f4f4f4;"">
          <table width=""100%"" bgcolor=""#f4f4f4"" cellpadding=""0"" cellspacing=""0"">
            <tr><td align=""center"" style=""padding:20px 0;"">
              <table width=""600"" bgcolor=""#ffffff"" cellpadding=""0"" cellspacing=""0"" style=""border-radius:8px;overflow:hidden;"">
                <tr>
                  <td style=""background:{tagColor};padding:20px;text-align:center;"">
                    <h1 style=""margin:0;color:#ffffff;font-size:24px;"">
                      ⏰ Nhắc nhở thói quen: {habitTitle}
                    </h1>
                  </td>
                </tr>
                <tr>
                  <td style=""padding:20px;color:#333333;font-size:16px;line-height:1.5;"">
                    <p style=""margin:0 0 10px;"">
                      Chào bạn! Thói quen <strong>{habitTitle}</strong> sẽ hết hạn vào 
                      <strong>{expiryDate}</strong>
                    </p>
                    <p style=""margin:0 0 20px;"">
                      Hãy tiếp tục duy trì bằng cách kiểm tra ngay trong ứng dụng 📱.
                    </p>
                    <table align=""center"" cellpadding=""0"" cellspacing=""0"">
                      <tr>
                        <td bgcolor=""{tagColor}"" style=""border-radius:4px;"">
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td></tr>
          </table>
        </body>
        </html>";

        await SendEmailAsync(toAddress, HABIT_EXPIRIED_SUBJECT, htmlTemplate);
    }

    private async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        _logger.Inform("Sending to " + toAddress);

        var mail = new MimeMessage
        {
            Sender = new MailboxAddress(_senderName, _senderMail),
            Subject = subject,
            Body = new BodyBuilder { HtmlBody = HttpUtility.HtmlDecode(body) }.ToMessageBody()
        };
        mail.From.Add(mail.Sender);
        mail.To.Add(MailboxAddress.Parse(toAddress));

        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        try
        {
            smtp.Connect("smtp.gmail.com", PORT, SecureSocketOptions.StartTls);
            smtp.Authenticate(_senderMail, _senderPassword);
            await smtp.SendAsync(mail);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
        }
        smtp.Disconnect(true);
    }
}
