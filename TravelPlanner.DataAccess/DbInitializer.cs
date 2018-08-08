using TravelPlanner.DomainModel;

namespace TravelPlanner.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(TravelPlannerDbContext context)
        {
            context.Users.Add(new User() { UserName = "test", Email = "test", PasswordHash = "pass", Phone = "phone"});
        }
    }
}
