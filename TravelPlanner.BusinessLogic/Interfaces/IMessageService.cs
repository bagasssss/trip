using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetAll(int chatId);
        Task<Message> Send(Message message);
    }
}