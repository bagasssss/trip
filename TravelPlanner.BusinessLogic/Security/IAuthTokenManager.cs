using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Security
{
    public interface IAuthTokenManager
    {
        string GetIdToken(User user);
        string GetAccessToken(User user);
    }
}