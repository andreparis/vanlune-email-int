using MediatR;
using Email.Int.Domain.Messaging.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Email.Int.Application.Application.MediatR.Events.SendEmail
{
    public class SendEmailEventHandler : INotificationHandler<SendEmailEvent>
    {
        private readonly IEmailSender _emailSender;

        public SendEmailEventHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Handle(SendEmailEvent notification, CancellationToken cancellationToken)
        {
            await _emailSender.Send(notification.Message.Subject, 
                notification.Message.Body, 
                notification.Message.From, 
                notification.Message.To, 
                notification.Message.Bcs).ConfigureAwait(false);
        }
    }
}
