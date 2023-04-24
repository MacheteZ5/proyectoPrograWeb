using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPW.Models;

namespace PPW.Controllers
{
    public class ChatsController : Controller
    {
        private readonly ProgramacionWebContext _context;

        public ChatsController(ProgramacionWebContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int ID)
        {
            ViewBag.ID = ID;
            var listaNombreContactos = new List<User>();
            var lista = await Functions.APIService.GetContactsList("");
            foreach (var contact in lista)
            {
                User user;
                if (contact.PuserId == ID)
                {
                    user = await Functions.APIService.GetUserbyID(contact.SuserId);
                    if (user != null)
                    {
                        listaNombreContactos.Add(user);
                    }
                }
                else if (contact.SuserId == ID)
                {
                    user = await Functions.APIService.GetUserbyID(contact.PuserId);
                    if (user != null)
                    {
                        listaNombreContactos.Add(user);
                    }
                }
            }
            //ViewBag.SID = new SelectList(listaNombreContactos, "Id", "Username");
            return View(listaNombreContactos.ToList());
        }
        public async Task<IActionResult> Create(int Id, int Contact)
        {
            var contact = new Contact { PuserId = Id, SuserId = Contact };
            var contactInfo = await Functions.APIService.GetContact(contact,"");
            if (contactInfo != null)
            {
                ViewBag.ContactID = contactInfo.Id;
                var user = await Functions.APIService.GetUserbyID(Id);
                var sUser = await Functions.APIService.GetUserbyID(Contact);
                ViewBag.Username = user.Username;
                ViewBag.Contactname = sUser.Username;
                ViewBag.ID = Id;
                var chats = await Functions.APIService.GetChat(contactInfo.Id, "");
                return View(chats.ToList());
            }
            return RedirectToAction("Index", "Chats", Id);
        }
        public IActionResult Return(int id)
        {
            return RedirectToAction("Index", "Chats", new { @ID = id });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContactId,UserId,Mensaje,Archivos,FecTransac")] Chat chat)
        {
            return View(chat);
        }
        // GET: Chats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chats == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }
        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chats == null)
            {
                return Problem("Entity set 'ProgramacionWebContext.Chats'  is null.");
            }
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ChatExists(int id)
        {
            return (_context.Chats?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
