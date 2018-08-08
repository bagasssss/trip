using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.BusinessLogic.Interfaces;
using TravelPlanner.DomainModel;
using TravelPlanner.Web.Infrastructure.Helpers;
using TravelPlanner.Web.Infrastructure.WebSocket;
using TravelPlanner.Web.Models;

namespace TravelPlanner.Web.Controllers
{
    [Authorize]
    public class MessageApiController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly WebSocketMessageHandler _chat;

        public MessageApiController(IMessageService messageService, WebSocketMessageHandler chat)
        {
            _messageService = messageService;
            _chat = chat;
        }

        [HttpGet]
        [Route("api/message/getall/{chatId}")]
        public async Task<IActionResult> GetAll(int chatId)
        {
            var dbMessages = await _messageService.GetAll(chatId);
            return Ok(dbMessages.Select(Mapper.Map<Message, MessageModel>));
        }

        [HttpPost]
        [Route("api/message/send")]
        public async Task<IActionResult> Send([FromBody] MessageModel model)
        {
            var sentMessage = await _messageService.Send(Mapper.Map<Message>(model));
            model.Id = sentMessage.Id;
            await _chat.SendMessageToAllAsync(JsonCamelSerializer.Serialize(model));
            return Ok();
        }
    }
}