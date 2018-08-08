using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Interfaces
{
    public interface ISightObjectService
    {
        Task<IEnumerable<SightObject>> Get();
    }
}