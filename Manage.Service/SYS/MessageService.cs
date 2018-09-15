using Manage.Core.Infrastructure;
using Manage.Data;
using Manage.Data.Data;
using Manage.Service.SYS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.SYS
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> _messageRepository;
        public MessageService(IRepository<Message> _messageRepository)
        {
            this._messageRepository = _messageRepository;
        }

        public Message GetMessage()
        {
            Message message = this._messageRepository.Entity(ContextDB.smsDBContext, t => 1 == 1);
            return message;
        }
    }
}
