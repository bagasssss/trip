using System;

namespace TravelPlanner.Web.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public int ChatId { get; set; }
        public Guid UserId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime SentDt { get; set; }
    }
}