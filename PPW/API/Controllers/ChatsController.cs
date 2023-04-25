using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatsController : Controller
    {
        private ProgramacionWebContext _context;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetChat")]
        [HttpPost]
        public IEnumerable<Modelos.Chat> GetChat([FromBody] int contadtId)
        {
            _context = new ProgramacionWebContext();
            var chatMessages = new List<Modelos.Chat>();
            try
            {
                chatMessages = (from m in _context.Chats where m.ContactId == contadtId
                                select new Modelos.Chat
                                {
                                    Id = m.ContactId,
                                    ContactId = m.ContactId,
                                    Mensaje = m.Mensaje,
                                    UserId = m.UserId,
                                    FecTransac  = m.FecTransac
                                }).ToList();
            }
            catch (Exception ex)
            {
                chatMessages = null;
            }
            return chatMessages;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("SetChat")]
        [HttpPost]
        public async Task<bool> SetChat([FromBody] Modelos.Chat chat)
        {
            _context = new ProgramacionWebContext();
            bool result;
            try
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                var Errormessage = ex.Message;
            }
            return result;
        }
    }
}
