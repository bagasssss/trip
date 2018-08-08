using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.DomainModel
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
        }

        [ForeignKey("Trip")]
        public int Id { get; set; }
        public Trip Trip { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}