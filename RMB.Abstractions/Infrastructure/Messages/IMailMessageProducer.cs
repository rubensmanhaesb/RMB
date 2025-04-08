using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMB.Abstractions.Infrastructure.Messages
{
    public interface IMailMessageProducer
    {
        Task PublishEmailConfirmationAsync(EmailConfirmationMessage message);
    }

}
