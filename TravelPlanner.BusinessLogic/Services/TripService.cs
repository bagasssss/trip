using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.DataAccess;
using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Services
{
    public class TripService : ITripService
    {
        private readonly IGenericRepository _repository;

        public TripService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Create(Trip trip)
        {
            var currentUser = await _repository.Find<User>(u => u.Id == trip.CreatorId);

            _repository.Add(trip);
            trip.Users.Add(currentUser);
            await _repository.SaveChanges();

            _repository.Add(new Chat()
            {
                Id = trip.Id
            });
            await _repository.SaveChanges();

            return trip.Id;
        }

        public async Task Remove(int id, Guid userId)
        {
            var trip = await GetTrip(id, userId);
            _repository.Remove(trip);
            await _repository.SaveChanges();
        }

        public async Task Update(Trip trip)
        {
            var dbTrip = await GetTrip(trip.Id, trip.CreatorId);

            dbTrip.Title = trip.Title;
            dbTrip.Description = trip.Description;

            if (trip.TripRoute != null)
            {
                dbTrip.TripRoute.Distance = trip.TripRoute.Distance;
                dbTrip.TripRoute.TripWaypoints = new List<TripWaypoint>();
                trip.TripRoute.TripWaypoints.ForEach(w => dbTrip.TripRoute.TripWaypoints.Add(w));
            }

            await _repository.SaveChanges();
        }

        public async Task<Trip> Get(int id, Guid userId)
        {
            return await GetTrip(id, userId);
        }

        public async Task<IEnumerable<Trip>> GetInvited(Guid userId)
        {
            var user = await _repository.Find<User>(u => u.Id == userId);
            return user.Trips.Where(t => t.CreatorId != userId).ToList();
        }

        public async Task<IEnumerable<Trip>> GetOwn(Guid userId)
        {
            return await _repository.GetList<Trip>(t => t.CreatorId == userId);
        }

        private async Task<Trip> GetTrip(int id, Guid userId)
        {
            var trip = await _repository.Find<Trip>(t => t.Id == id
                                                         && (t.CreatorId == userId ||
                                                             t.Users.Any(u => u.Id == userId)));
            return trip;
        }
    }
}