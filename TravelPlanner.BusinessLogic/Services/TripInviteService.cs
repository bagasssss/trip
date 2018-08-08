using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.DataAccess;
using TravelPlanner.DomainModel;

namespace TravelPlanner.BusinessLogic.Services
{
    public class TripInviteService : ITripInviteService
    {
        private readonly IGenericRepository _repository;

        public TripInviteService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TripInvite>> AddInvites(IEnumerable<TripInvite> invites)
        {
            var tripId = invites.FirstOrDefault().TripId;

            var uniqueInvites = await AddUniqueInvites(invites, tripId);
            return uniqueInvites;
        }

        public async Task<TripInvite> AcceptInvite(int inviteId, Guid userId)
        {
            var tripInvite = await Get(inviteId);
            if (tripInvite != null)
            {
                if (tripInvite.Trip.Users.Any(u => u.Id == userId))
                {
                    return null;
                }

                var user = await _repository.Find<User>(u => u.Id == userId);
                tripInvite.Trip.Users.Add(user);

                _repository.Remove(tripInvite);
                await _repository.SaveChanges();
            }

            return tripInvite;
        }

        public async Task<TripInvite> Get(int inviteId)
        {
            var trip = await _repository.Find<TripInvite>(t => t.Id == inviteId);
            return trip;
        }

        private async Task<List<TripInvite>> AddUniqueInvites(IEnumerable<TripInvite> invites, int tripId)
        {
            var trip = await _repository.Find<Trip>(t => t.Id == tripId);
            var uniqueInvites = invites.Where(invite => trip.TripInvites.All(t => t.Phone != invite.Phone)).ToList();

            uniqueInvites.ForEach(invite => trip.TripInvites.Add(invite));

            await _repository.SaveChanges();
            return uniqueInvites;
        }
    }
}