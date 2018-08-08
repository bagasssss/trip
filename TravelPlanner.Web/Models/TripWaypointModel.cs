using System;
using TravelPlanner.DomainModel;

namespace TravelPlanner.Web.Models
{
    public class TripWaypointModel
    {
        public Guid Id { get; set; }
        public int TripRouteId { get; set; }
        public string Name { get; set; }
        public LatLng LatLng { get; set; }
    }
}