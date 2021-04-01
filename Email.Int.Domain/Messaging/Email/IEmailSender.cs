using Email.Int.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Email.Int.Domain.Messaging.Email
{
    public interface IEmailSender
    {
        Task Send(string subject, string htmlBody, string from, string to, List<string> bcs);
    }
}
