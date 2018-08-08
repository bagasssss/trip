using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.DomainModel
{
    public class SightObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid LatLngId { get; set; }

        [Required]
        public string Label { get; set; }
        public string Description { get; set; }

        public virtual LatLng LatLng { get; set; }
    }
}