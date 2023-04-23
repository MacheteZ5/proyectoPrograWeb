using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatsController : Controller
    {
        private Proyecto_Progra_Web.Models.ProgramacionWebContext _context;

        [Route("SetChat")]
        [HttpPost]
        public async Task<bool> SetChat([FromBody] Proyecto_Progra_Web.Models.Chat chat)
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
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
