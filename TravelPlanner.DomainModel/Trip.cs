using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.DomainModel
{
    public class Trip
    {
        public Trip()
        {
            TripInvites = new List<TripInvite>();
            Users = new List<User>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid CreatorId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description  { get; set; }

        public virtual TripRoute TripRoute { get; set; }
        public virtual Chat Chat { get; set; }
        public virtual ICollection<TripInvite> TripInvites { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
