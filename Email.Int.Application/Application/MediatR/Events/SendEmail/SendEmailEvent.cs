using Email.Int.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Int.Application.Application.MediatR.Events.SendEmail
{
    public class SendEmailEvent : INotification
    {
        public Message Message { get; set; }
    }
}
