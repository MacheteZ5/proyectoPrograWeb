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
    public class UsersController : Controller
    {
        private ProgramacionWebContext _context;

        public UsersController(ProgramacionWebContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> main([Bind("Username,Password")] User user)
        {
            var username = user.Username;
            var password = user.Password;
            if (username != "" && password != "")
            {
                if (await Functions.APIService.GetValidationUser(username,0))
                {
                    var userInformation = await Functions.APIService.GetUser(username);
                    if (userInformation.Password == password)
                    {
                        return RedirectToAction("Index", "Chats", new { @ID = userInformation.Id });
                    }
                }

            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            ViewBag.Genero = new List<string>() { "Masculino", "Femenino" };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,StatusId,FirstName,LastName,Phone,Birthdate,Email,Genero,FecTransac")] User user)
        {
            var genero = (Request.Form["Genero"]=="Masculino")?true:false;
            var username = user.Username;
            if (!await Functions.APIService.GetValidationUser(username,1))
            {
                user.Genero = genero;
                user.Username = username.ToUpper();
                user.StatusId = 1;
                if (await Functions.APIService.SetUser(user))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Genero = new List<string>() { "Masculino", "Femenino" };
            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Username,Password")] User user)
        {
            var username = user.Username;
            var password = user.Password;
            if (await Functions.APIService.GetValidationUser(username, 0))
            {
                var oldUser = await Functions.APIService.GetUser(username);
                if(oldUser != null)
                {
                    oldUser.Password = password;
                    if (await Functions.APIService.updateUser(oldUser))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(user);
        }




        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View();
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();
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
    }
}
