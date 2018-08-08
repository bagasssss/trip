using System;

namespace TravelPlanner.Web.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string PetrolUsage { get; set; }
    }
}