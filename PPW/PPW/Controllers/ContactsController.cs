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
            ViewBag.PrimerUserId = new SelectList(alternateList, "Id", "Username");
            //obtener información del cada usuario activo
            var allUsers = await Functions.APIService.GetAllUsers();
            var allActiveUsers = new List<User>();
            var allAvailableUsers = new List<User>();
            foreach (User user in allUsers)
            {
                if (user.StatusId == 1)
                {
                    if(user.Id != ID)
                    {
                        allActiveUsers.Add(user);
                    }
                }
            }
            foreach(User user in allActiveUsers)
            {
                var contact = new Contact()
                {
                    PuserId = user.Id,
                    SuserId = ID
                };
                var contactInfo = await Functions.APIService.GetContact(contact);
                if(contactInfo == null)
                {
                    allAvailableUsers.Add(user);
                }
            }
            ViewData["SegundoUserId"] = new SelectList(allAvailableUsers, "Id", "Username");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PuserId,SuserId,FecTransac")] Contact contact)
        {
            if (await Functions.APIService.SetContact(contact))
            {
                return RedirectToAction("Index", "Chats", new { @ID = contact.PuserId });
            }
            ViewBag.ID = contact.PuserId;
            var alternateList = new List<User>() { await Functions.APIService.GetUserbyID(contact.PuserId) };
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
        public async Task<IActionResult> Edit()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PuserId,SuserId,FecTransac")] Contact contact)
        {
            return View(contact);
        }
        public async Task<IActionResult> Delete(int ID)
        {
            ViewBag.ID = ID;
            //obtener información del usuario principal
            var alternateList = new List<User>() { await Functions.APIService.GetUserbyID(ID) };
            ViewBag.PrimerUserId = new SelectList(alternateList, "Id", "Username");
            //obtener información del cada usuario activo relacionados al contacto
            var lista = await Functions.APIService.GetContactsList();
            var listaNombreContactos = new List<User>();
            foreach (var contact in lista)
            {
                User user;
                if (contact.PuserId == ID)
                {
                    user = await Functions.APIService.GetUserbyID(contact.SuserId);
                    if(user != null)
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
            ViewData["SegundoUserId"] = new SelectList(listaNombreContactos, "Id", "Username");
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("PuserId,SuserId,FecTransac")] Contact contact)
        {
            var contactInfo = await Functions.APIService.GetContact(contact);
            if (await Functions.APIService.DeleteContact(contactInfo))
            {
                return RedirectToAction("Index", "Chats", new { @ID = contact.PuserId });
            }
            ViewBag.ID = contact.PuserId;
            var alternateList = new List<User>() { await Functions.APIService.GetUserbyID(contact.PuserId) };
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
            return View(contactInfo);
        }
    }
}
