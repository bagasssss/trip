using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.DomainModel
{
    public class TripRoute
    {
        public TripRoute()
        {
            TripWaypoints = new List<TripWaypoint>();
        }

        [ForeignKey("Trip")]
        public int Id { get; set; }
        public string Distance { get; set; }

        public virtual List<TripWaypoint> TripWaypoints { get; set; }
        public virtual Trip Trip { get; set; }
    }
}