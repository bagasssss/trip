using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.BusinessLogic.Models;
using TravelPlanner.DomainModel;
using TravelPlanner.Identity.IdentityManagers;
using TravelPlanner.Web.Models;

namespace TravelPlanner.Web.Controllers
{
    [Authorize]
    public class InviteApiController : Controller
    {
        private readonly ITripInviteService _inviteService;
        private readonly INotificationService _notificationService;
        private readonly ApplicationUserManager _userManager;

        public InviteApiController(ITripInviteService inviteService, INotificationService notificationService, ApplicationUserManager userManager)
        {
            _inviteService = inviteService;
            _notificationService = notificationService;
            _userManager = userManager;
        }

        [Route("api/invite/send")]
        [HttpPost]
        public async Task<IActionResult> SendInvites([FromBody] InvitesModel model)
        {
            var tripInvites = new List<TripInvite>();
            model.Phones.ToList().ForEach(p => tripInvites.Add(new TripInvite()
            {
                InvitorId = model.InvitorUserId,
                Phone = p,
                TripId = model.TripId
            }));

            var invites = await _inviteService.AddInvites(tripInvites);

            var userInfo = await _userManager.FindByIdAsync(model.InvitorUserId);
            await SendInvites(userInfo.UserName, invites.ToList());

            return Ok();
        }

        [Route("api/invite/accept")]
        [HttpPost]
        public async Task<IActionResult> AcceptInvite([FromBody] AcceptInviteModel model)
        {
            var tripInvite = await _inviteService.AcceptInvite(model.InviteId, model.UserId);
            if (tripInvite == null) return BadRequest(ValidationResultCodes.InviteNotFound);
            return Ok(tripInvite.TripId);
        }

        private async Task SendInvites(string invitorUserName, IEnumerable<TripInvite> invites)
        {
            var inviteLink = $"http://{Request.Host}/acceptinvite/";
            var notifications = invites.ToList().Select(invite => new NotificationModel()
            {
                Text = $"Привет, {invitorUserName} приглашает вас в путешествие: {inviteLink + invite.Id}.",
                To = invite.Phone
            });

            await _notificationService.SendNotifications(notifications);
        }
    }
}