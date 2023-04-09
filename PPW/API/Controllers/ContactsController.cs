using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                PuserId = s.PuserId,
                SuserId = s.SuserId,
            }
            ).ToListAsync();
            return contacts;
        }

        [Route("GetContact")]
        [HttpPost]
        public async Task<PPW.Models.Contact> GetContact([FromBody] PPW.Models.Contact contact)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var contactInfo = await _context.Contacts.Select(s =>
            new PPW.Models.Contact
            {
                Id = s.Id,
                PuserId = s.PuserId,
                SuserId = s.SuserId,
                FecTransac = s.FecTransac
            }
            ).FirstOrDefaultAsync(s => (s.PuserId == contact.PuserId || s.PuserId == contact.SuserId) && (s.SuserId == contact.PuserId || s.SuserId == contact.SuserId));
            return contactInfo;
        }

        [Route("GetContactById")]
        [HttpPost]
        public async Task<PPW.Models.Contact> GetContactById([FromBody] int contactId)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var contact = await _context.Contacts.Select(s =>
            new PPW.Models.Contact
            {
                Id = s.Id,
                PuserId = s.PuserId,
                SuserId = s.SuserId,
                FecTransac = s.FecTransac
            }
            ).FirstOrDefaultAsync(s => s.Id == contactId);
            return contact;
        }

        [Route("SetContact")]
        [HttpPost]
        public async Task<bool> SetContact([FromBody] PPW.Models.Contact contact)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            bool result;
            try
            {
                _context.Add(contact);
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

        [Route("DeleteContact")]
        [HttpPost]
        public async Task<bool> DeleteContact([FromBody] PPW.Models.Contact contact)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            bool result;
            try
            {
                _context.Contacts.Remove(contact);
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
