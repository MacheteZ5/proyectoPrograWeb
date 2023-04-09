using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatsController : Controller
    {
        private PPW.Models.ProgramacionWebContext _context;

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
