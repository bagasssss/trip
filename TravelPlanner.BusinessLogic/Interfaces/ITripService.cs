using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Interfaces
{
    public interface ITripService
    {
        Task<int> Create(Trip model);
        Task Remove(int id, Guid userId);
        Task Update(Trip model);
        Task<Trip> Get(int id, Guid userId);
        Task<IEnumerable<Trip>> GetInvited(Guid userId);
        Task<IEnumerable<Trip>> GetOwn(Guid userId);
    }
}
