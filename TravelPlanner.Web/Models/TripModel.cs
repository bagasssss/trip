using System;

namespace TravelPlanner.Web.Models
{
    public class TripModel
    {
        public string Id { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
    }
}