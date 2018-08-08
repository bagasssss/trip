using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.BusinessLogic.Models;

namespace TravelPlanner.BusinessLogic.Services
{
    public class TextFileNotificationService: INotificationService
    {
        public async Task SendNotification(NotificationModel notification)
        {
            await SendNotifications(new[] {notification});
        }

        public async Task SendNotifications(IEnumerable<NotificationModel> notifications)
        {
            await Task.Run(() =>
            {
                var streamWriter = new StreamWriter(File.Open("notifications.txt", FileMode.OpenOrCreate));
                foreach (var notification in notifications)
                {
                    streamWriter.WriteLine(notification.Text);
                }

                streamWriter.Close();
            });
        }
    }
}