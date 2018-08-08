using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.DomainModel
{
    public class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string PetrolUsage { get; set; }

        public virtual User Users { get; set; }
    }
}