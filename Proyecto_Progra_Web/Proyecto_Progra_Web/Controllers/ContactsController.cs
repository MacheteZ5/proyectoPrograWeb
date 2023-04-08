using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Progra_Web.Models;

namespace Proyecto_Progra_Web.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ProgramacionWebContext _context;

        public ContactsController(ProgramacionWebContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            
            return View();
        }


        public async Task<IActionResult> Create(int ID)
        {
            ViewBag.ID = ID;
            //obtener información del usuario principal
            var alternateList = new List<User>() { await Functions.APIService.GetUserbyID(ID) };
            ViewData["PrimerUserId"] = new SelectList(alternateList, "Id","Username");
            //obtener información del cada usuario activo
            var allUsers = await Functions.APIService.GetAllUsers();
            var allActiveUsers = new List<User>();
            foreach (User user in allUsers)
            {
                if(user.StatusId == 1)
                {
                    allActiveUsers.Add(user);
                }
            }
            ViewData["SegundoUserId"] = new SelectList(allActiveUsers, "Id", "Username");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrimerUserId,SegundoUserId,FecTransac")] Contact contact)
        {
            contact.Id = 0;
            if(await Functions.APIService.SetContact(contact))
            {
                return RedirectToAction("Index", "Chats", new { @ID = contact.PrimerUserId });
            }
            ViewBag.ID = contact.PrimerUserId;
            var alternateList = new List<User>() { await Functions.APIService.GetUserbyID(contact.PrimerUserId) };
            ViewData["PrimerUserId"] = new SelectList(alternateList, "Id", "Username");
            var allUsers = await Functions.APIService.GetAllUsers();
            var allActiveUsers = new List<User>();
            foreach (User user in allUsers)
            {
                if (user.StatusId == 1)
                {
                    allActiveUsers.Add(user);
                }
            }
            ViewData["SegundoUserId"] = new SelectList(allActiveUsers, "Id", "Username");
            return View(contact);
        }








        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["PrimerUserId"] = new SelectList(_context.Users, "Id", "Id", contact.PrimerUserId);
            ViewData["SegundoUserId"] = new SelectList(_context.Users, "Id", "Id", contact.SegundoUserId);
            return View(contact);
        }
        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrimerUserId,SegundoUserId,FecTransac")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PrimerUserId"] = new SelectList(_context.Users, "Id", "Id", contact.PrimerUserId);
            ViewData["SegundoUserId"] = new SelectList(_context.Users, "Id", "Id", contact.SegundoUserId);
            return View(contact);
        }
        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();
        }
        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'ProgramacionWebContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ContactExists(int id)
        {
          return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
