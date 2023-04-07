using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : Controller
    {
        private Proyecto_Progra_Web.Models.ProgramacionWebContext _context;

        [Route("GetAllContacts")]
        [HttpGet]
        public async Task<IEnumerable<Proyecto_Progra_Web.Models.Contact>> GetContactsList()
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
            var contacts = await _context.Contacts.Select(s =>
            new Proyecto_Progra_Web.Models.Contact
            {
                Id = s.Id,
                PrimerUserId = s.PrimerUserId,
                SegundoUserId = s.SegundoUserId,
            }
            ).ToListAsync();
            return contacts;
        }

        [Route("SetContact")]
        [HttpPost]
        public async Task<Proyecto_Progra_Web.Models.generalResult> SetContact([FromBody] Proyecto_Progra_Web.Models.Contact contact)
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
            var generalResult = new Proyecto_Progra_Web.Models.generalResult
            {
                Result = false
            };
            try
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                generalResult.Result = true;
            }
            catch (Exception ex)
            {
                generalResult.Result = false;
                generalResult.ErrorMessage = ex.Message;
            }
            return generalResult;
        }

    }
}
