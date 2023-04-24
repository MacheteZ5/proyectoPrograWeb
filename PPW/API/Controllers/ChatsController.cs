using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPW.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatsController : Controller
    {
        private PPW.Models.ProgramacionWebContext _context;

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetChat")]
        [HttpPost]
        public IEnumerable<PPW.Models.Chat> GetChat([FromBody] int contadtId)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var chatMessages = new List<PPW.Models.Chat>();
            try
            {
                chatMessages = (from m in _context.Chats where m.ContactId == contadtId
                                select new PPW.Models.Chat
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
        public async Task<bool> SetChat([FromBody] PPW.Models.Chat chat)
        {
            _context = new PPW.Models.ProgramacionWebContext();
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
