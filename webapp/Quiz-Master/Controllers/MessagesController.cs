using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quiz_Master.Middleware;

namespace Quiz_Master.Controllers
{
    public class MessagesController : Controller
    {
        private NotificationsMessageHandler _notificationsMessageHandler { get; set; }

        public MessagesController(NotificationsMessageHandler notificationsMessageHandler)
        {
            _notificationsMessageHandler = notificationsMessageHandler;
        }

        [HttpGet]
        public async Task SendMessage([FromQueryAttribute]string message)
        {
            await _notificationsMessageHandler.SendMessageToAllAsync(message);
        }
    }
}
