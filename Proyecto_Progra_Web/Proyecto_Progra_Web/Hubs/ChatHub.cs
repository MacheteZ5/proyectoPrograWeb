using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using Proyecto_Progra_Web.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_Progra_Web.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string user, string message, string contactId)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message); 
            await Groups.AddToGroupAsync(Context.ConnectionId, contactId);
            await Clients.Group(contactId).SendAsync("ReceiveMessage", user, message);
            var userInfo = await Functions.APIService.GetUser(user);
            var chat = new Chat()
            {
                ContactId = Convert.ToInt32(contactId),
                UserSend = userInfo.Id,
                Mensaje = message,
                Contact = await Functions.APIService.GetContactById(Convert.ToInt32(contactId))
            };
            if (await Functions.APIService.SetChat(chat))
            {

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
