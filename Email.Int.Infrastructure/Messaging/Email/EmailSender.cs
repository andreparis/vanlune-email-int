using Email.Int.Domain.Messaging.Email;
using Email.Int.Infraestructure.Logging;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Email.Int.Infrastructure.Messaging.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IConfiguration configuration,
            ILogger logger)
        {
            _logger = logger;
        }

        public async Task Send(string subject, string htmlBody, string from, string to, List<string> bcs)
        {
            _logger.Info($"Sending email To {to}; subject {subject}");

            try
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress(from, from));
                mailMessage.To.Add(new MailboxAddress(to, to));
                if (bcs != null && bcs.Any())
                    mailMessage.Bcc.AddRange(bcs.Select(x => new MailboxAddress(x, x)));
                mailMessage.Subject = subject;
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody,
                    TextBody = ""
                };
                mailMessage.Body = bodyBuilder.ToMessageBody();
                using var smtpClient = new SmtpClient();
                smtpClient.Connect("smtp.yandex.com", 465, true);
                smtpClient.Authenticate(from, "@FbDM$ud3u4m)ZL");
                await smtpClient.SendAsync(mailMessage).ConfigureAwait(false);
                smtpClient.Disconnect(true);

                _logger.Info($"Sent To {to}; subject {subject}; Body {htmlBody}");
            }
            catch(Exception e)
            {
                _logger.Error($"Error {e.Message} at {e.StackTrace}");
            }
        }
    }
}
