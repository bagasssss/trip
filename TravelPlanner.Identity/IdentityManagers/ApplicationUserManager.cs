using System;
using Microsoft.AspNet.Identity;
using TravelPlanner.DomainModel;

namespace TravelPlanner.Identity.IdentityManagers
{
    public class ApplicationUserManager : UserManager<User, Guid>
    {
        public ApplicationUserManager(IUserStore<User, Guid> store) : base(store)
        {
        }
    }
}