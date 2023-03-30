using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPW.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : Controller
    {
        private PPW.Models.ProgramacionWebContext _context;

        [Route("GetAllContacts")]
        [HttpGet]
        public async Task<IEnumerable<PPW.Models.Contact>> GetContactsList()
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var contacts = await _context.Contacts.Select(s =>
            new PPW.Models.Contact
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
        public async Task<PPW.Models.generalResult> CreateContact([FromBody] PPW.Models.Contact contact)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var generalResult = new PPW.Models.generalResult
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
