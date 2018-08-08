using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.DomainModel;
using TravelPlanner.Web.Models;

namespace TravelPlanner.Web.Controllers
{
    [Authorize]
    public class TripApiController : Controller
    {
        private readonly ITripService _tripService;

        public TripApiController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [Route("api/trip/create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TripDetailModel model)
        {
            var id = await _tripService.Create(Mapper.Map<Trip>(model));
            return Ok(id);
        }

        [Route("api/trip/get/{id}/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetTrip(int id, Guid userId)
        {
            var trip = await _tripService.Get(id, userId);
            var tripModel = Mapper.Map<TripDetailModel>(trip);
            return Ok(tripModel);
        }

        [Route("api/trip/getown/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetOwnTrips(Guid userId)
        {
            var trips = (await _tripService.GetOwn(userId)).Select(Mapper.Map<TripModel>);
            return Ok(trips);
        }

        [Route("api/trip/getinvited/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetInvitedTrips(Guid userId)
        {
            var trips = (await _tripService.GetInvited(userId)).Select(Mapper.Map<TripModel>);
            return Ok(trips);
        }

        [Route("api/trip/update")]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] TripDetailModel model)
        {
            await _tripService.Update(Mapper.Map<Trip>(model));
            return Ok();
        }

        [Route("api/trip/remove")]
        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] int id, Guid userId)
        {
            await _tripService.Remove(id, userId);
            return Ok();
        }
    }
}