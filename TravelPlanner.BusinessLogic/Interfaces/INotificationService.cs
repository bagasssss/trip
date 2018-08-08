using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlanner.BusinessLogic.Models;

namespace TravelPlanner.BusinessLogic.Interfaces
{
    public interface INotificationService
    {
        Task SendNotification(NotificationModel notification);
        Task SendNotifications(IEnumerable<NotificationModel> notifications);
    }
}