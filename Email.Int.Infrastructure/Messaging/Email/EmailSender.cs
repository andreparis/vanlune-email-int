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
        private readonly string _sender;
        private readonly List<string> _to;

        public EmailSender(IConfiguration configuration,
            ILogger logger)
        {
            if (Debugger.IsAttached)
            {
                _sender = configuration["Email:sender"];
                _to = configuration.GetSection("Email:to").Get<List<string>>();
            }
            else
            {
                _sender = configuration["EMAIL_SEND"];
                _to = new List<string>() { configuration["EMAIL_TO"] };
            }

            _logger = logger;
        }

        public async Task Send(string subject, string htmlBody, string to)
        {
            _logger.Info($"Sending email To {to}; subject {subject}");

            try
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress(_sender, _sender));
                mailMessage.To.Add(new MailboxAddress(to, to));
                mailMessage.Bcc.AddRange(_to.Select(x => new MailboxAddress(x, x)));
                mailMessage.Subject = subject;
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody,
                    TextBody = ""
                };
                mailMessage.Body = bodyBuilder.ToMessageBody();
                using var smtpClient = new SmtpClient();
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.Authenticate("andreparis.comp@gmail.com", "m3uVent1l4d0r@&b=om");
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
