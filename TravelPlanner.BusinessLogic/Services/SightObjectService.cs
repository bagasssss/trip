using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.DataAccess;
using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Services
{
    public class SightObjectService : ISightObjectService
    {
        private readonly IGenericRepository _repository;

        public SightObjectService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SightObject>> Get()
        {
            var sights = await _repository.GetAll<SightObject>();
            if (!sights.Any())
            {
                var json = File.ReadAllText("sights.json");
                sights = new JavaScriptSerializer().Deserialize<IEnumerable<SightObject>>(json);
                foreach (var sightObject in sights)
                {
                    _repository.Add(sightObject);
                }

                await _repository.SaveChanges();
            }

            return sights.ToList();
        }
    }
}