using System;
using System.Collections.Generic;

namespace TravelPlanner.Web.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public IEnumerable<CarModel> Cars { get; set; }
    }
}