using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using Microsoft.EntityFrameworkCore;

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
                                    Id = m.Id,
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
        [Route("GetChatbyID")]
        [HttpPost]
        public async Task<Modelos.Chat> GetChatbyID([FromBody] int id)
        {
            _context = new ProgramacionWebContext();
            var Chat = await _context.Chats.Select(s =>
            new Modelos.Chat
            {
                Id = s.Id,
                ContactId = s.ContactId,
                Mensaje = s.Mensaje,
                UserId = s.UserId,
                FecTransac = s.FecTransac
            }
            ).FirstOrDefaultAsync(s => s.Id == id );
            return Chat;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("UpdateChat")]
        [HttpPut]
        public async Task<bool> UpdateChat([FromBody] Modelos.Chat Chat)
        {
            _context = new ProgramacionWebContext();
            bool result;
            try
            {
                _context.Chats.Update(Chat);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("DeleteChat")]
        [HttpPost]
        public async Task<bool> DeleteChat([FromBody] Modelos.Chat Chat)
        {
            _context = new ProgramacionWebContext();
            bool result;
            try
            {
                _context.Chats.Remove(Chat);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
