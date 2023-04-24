using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ContactsController : Controller
    {
        private PPW.Models.ProgramacionWebContext _context;
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetContactList")]
        [HttpPost]
        public IEnumerable<PPW.Models.Contact> GetContactList([FromBody] int contact)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var contactList = new List<PPW.Models.Contact>();
            try
            {
                contactList = (from c in _context.Contacts
                               where (c.PuserId == contact || c.SuserId == contact)
                               select new PPW.Models.Contact
                               {
                                   Id = c.Id,
                                   PuserId = c.PuserId,
                                   SuserId = c.SuserId
                               }).ToList();
            }
            catch (Exception ex)
            {
                contactList = null;
            }
            return contactList;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
