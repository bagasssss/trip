using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.DomainModel
{
    public class TripWaypoint
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int TripRouteId { get; set; }
        public Guid LatLngId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual LatLng LatLng { get; set; }
        public virtual TripRoute TripRoute { get; set; }
    }
}