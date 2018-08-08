using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace TravelPlanner.DomainModel
{
    public class User: IUser<Guid>
    {
        public User()
        {
            Trips = new List<Trip>();
            Cars = new List<Car>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}