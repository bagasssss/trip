using System;

namespace TravelPlanner.Web.Models
{
    public class AcceptInviteModel
    {
        public int InviteId { get; set; }
        public Guid UserId { get; set; }
    }
}