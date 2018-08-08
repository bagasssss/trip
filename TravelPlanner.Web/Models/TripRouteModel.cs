using System.Collections.Generic;

namespace TravelPlanner.Web.Models
{
    public class TripRouteModel
    {
        public int Id { get; set; }
        public string Distance { get; set; }
        public string Time { get; set; }
        public List<TripWaypointModel> TripWaypoints { get; set; }
    }
}