using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P_Progra_Web.Models;

namespace P_Progra_Web.Controllers
{
    public class ContactListsController : Controller
    {
        private readonly ProgramacionWebContext _context;

        public ContactListsController(ProgramacionWebContext context)
        {
            _context = context;
        }

        // GET: ContactLists
        public async Task<IActionResult> Index()
        {
            var programacionWebContext = _context.ContactLists.Include(c => c.PrimerUser).Include(c => c.SegundoUser);
            return View(await programacionWebContext.ToListAsync());
        }

        // GET: ContactLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContactLists == null)
            {
                return NotFound();
            }

            var contactList = await _context.ContactLists
                .Include(c => c.PrimerUser)
                .Include(c => c.SegundoUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactList == null)
            {
                return NotFound();
            }

            return View(contactList);
        }

        // GET: ContactLists/Create
        public IActionResult Create()
        {
            ViewData["PrimerUserId"] = new SelectList(_context.People, "Id", "Id");
            ViewData["SegundoUserId"] = new SelectList(_context.People, "Id", "Id");
            return View();
        }

        // POST: ContactLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrimerUserId,SegundoUserId,StatusId,FecTransac")] ContactList contactList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PrimerUserId"] = new SelectList(_context.People, "Id", "Id", contactList.PrimerUserId);
            ViewData["SegundoUserId"] = new SelectList(_context.People, "Id", "Id", contactList.SegundoUserId);
            return View(contactList);
        }

        // GET: ContactLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContactLists == null)
            {
                return NotFound();
            }

            var contactList = await _context.ContactLists.FindAsync(id);
            if (contactList == null)
            {
                return NotFound();
            }
            ViewData["PrimerUserId"] = new SelectList(_context.People, "Id", "Id", contactList.PrimerUserId);
            ViewData["SegundoUserId"] = new SelectList(_context.People, "Id", "Id", contactList.SegundoUserId);
            return View(contactList);
        }

        // POST: ContactLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrimerUserId,SegundoUserId,StatusId,FecTransac")] ContactList contactList)
        {
            if (id != contactList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactListExists(contactList.Id))
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
            ViewData["PrimerUserId"] = new SelectList(_context.People, "Id", "Id", contactList.PrimerUserId);
            ViewData["SegundoUserId"] = new SelectList(_context.People, "Id", "Id", contactList.SegundoUserId);
            return View(contactList);
        }

        // GET: ContactLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContactLists == null)
            {
                return NotFound();
            }

            var contactList = await _context.ContactLists
                .Include(c => c.PrimerUser)
                .Include(c => c.SegundoUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactList == null)
            {
                return NotFound();
            }

            return View(contactList);
        }

        // POST: ContactLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContactLists == null)
            {
                return Problem("Entity set 'ProgramacionWebContext.ContactLists'  is null.");
            }
            var contactList = await _context.ContactLists.FindAsync(id);
            if (contactList != null)
            {
                _context.ContactLists.Remove(contactList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactListExists(int id)
        {
          return (_context.ContactLists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
