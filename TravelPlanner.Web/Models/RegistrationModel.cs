using System;

namespace TravelPlanner.Web.Models
{
    public class RegistrationModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string CarName { get; set; }
        public string CarPetrolUsage { get; set; }
    }
}