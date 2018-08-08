using System;
using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.DomainModel
{
    public class TripInvite
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid InvitorId { get; set; }

        [Required]
        public int TripId { get; set; }

        [Required]
        public string Phone { get; set; }

        public virtual Trip Trip { get; set; }
    }
}