using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DiaSymReader;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PPW.Models;

namespace PPW.Controllers
{
    public class UsersController : Controller
    {
        private readonly ProgramacionWebContext _context;
        private IToastNotification _toastNotification;

        public UsersController(ProgramacionWebContext context)
        {
            _context = context;
        }

       
        //login view
        public async Task<IActionResult> Index()
        {
              return View();
        }
        //Validate user"
        public async Task<IActionResult> main([Bind("Username,Password")] User user)
        {
            var username = user.Username;
            var password = user.Password;
            if(username != ""&&password !="")
            {
                if (await Functions.APIService.GetValidationUser(username))
                {
                    var userInformation = await Functions.APIService.GetUser(username);
                    if (userInformation.Password == password)
                    {
                        return RedirectToAction("Index", "Chats");
                    }
                }

            }
            //_toastNotification.AddSuccessToastMessage("prueba");
            return RedirectToAction(nameof(Index));
        }



        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

       


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,FirstName,LastName,Phone,Birthdate,Email,Genero,FecTransac")] User user)
        {
            var username = user.Username;
            if (!await Functions.APIService.GetValidationUser(username))
            {
                if (ModelState.IsValid)
                {
                    user.Username = username.ToUpper();
                    user.StatusId = 1;
                    await Functions.APIService.SetUser(user);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,StatusId,FirstName,LastName,Phone,Birthdate,Email,Genero,FecTransac")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ProgramacionWebContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
