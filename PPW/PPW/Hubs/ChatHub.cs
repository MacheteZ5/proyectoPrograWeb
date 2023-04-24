using Microsoft.AspNetCore.SignalR;
using PPW.Models;
using PPW.Functions;

namespace PPW.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string user, string message, string contactId)
        {
            if(message!="")
            {
                var userInfo = await APIService.GetUser(user);
                var chat = new Chat()
                {
                    ContactId = Convert.ToInt32(contactId),
                    UserId = userInfo.Id,
                    Mensaje = message
                };
                if (await APIService.SetChat(chat, ""))
                {
                    await Clients.Group(contactId).SendAsync("ReceiveMessage", user, message);
                }
            }
        }
        public async Task AddToGroup(string contactId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, contactId);
        }
        public async Task RemoveFromGroup(string contactId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, contactId);
        }
    }
}
