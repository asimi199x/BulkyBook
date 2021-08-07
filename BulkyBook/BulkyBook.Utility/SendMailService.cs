using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public class SendMailService
    {
        private MailSettings _mailSetting { get; set; }
        public SendMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSetting = mailSettings.Value;
        }
        public void SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail);
            email.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail));
            email.To.Add(new MailboxAddress(mailContent.To,mailContent.To));
            email.Subject = mailContent.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
            smtp.Send(email);

            smtp.Disconnect(true);
        }
    }
}

public class MailContent
{
    public string To { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

}
