using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.DataAccess;
using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Services
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository _repository;

        public MessageService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Message>> GetAll(int chatId)
        {
            return await _repository.GetList<Message>(m => m.ChatId == chatId);
        }

        public async Task<Message> Send(Message message)
        {
            message.SentDt = DateTime.UtcNow;
            _repository.Add(message);
            await _repository.SaveChanges();

            return message;
        }
    }
}