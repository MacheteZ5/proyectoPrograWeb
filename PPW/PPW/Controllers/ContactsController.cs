using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PPW.Models;

namespace PPW.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IToastNotification _toastNotification;
        public ContactsController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        
        [Authorize]
        public async Task<IActionResult> Create(int ID)
        {
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            _toastNotification.AddInfoToastMessage("Seleccione el usuario que desea guardar como contacto.");
            //obtener información del usuario principal
            ViewBag.ID = ID;
            var mainList = new List<User>() { await Functions.APIService.GetUserbyID(ID) };
            ViewBag.PrimerUserId = new SelectList(mainList, "Id", "Username");
            //obtener información de cada usuario activo que puedan ser asociados a una nueva conversación
            var allActiveUsers = await Functions.APIService.GetAllUsers(ID);
            var allAvailableUsers = new List<User>();
            foreach(User user in allActiveUsers)
            {
                var contact = new Contact()
                {
                    PuserId = user.Id,
                    SuserId = ID
                };
                var contactInfo = await Functions.APIService.GetContact(contact,token);
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
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            if (await Functions.APIService.SetContact(contact, token))
            {
                _toastNotification.AddSuccessToastMessage("Creación de contacto realizado exitosamente.");
                return RedirectToAction("Index", "Chats", new { @ID = contact.PuserId });
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Creación de contacto incorrecta.");
            }
            ViewBag.ID = contact.PuserId;
            var alternateList = new List<User>() { await Functions.APIService.GetUserbyID(contact.PuserId) };
            ViewData["PrimerUserId"] = new SelectList(alternateList, "Id", "Username");
            var allActiveUsers = await Functions.APIService.GetAllUsers(contact.PuserId);
            var allAvailableUsers = new List<User>();
            foreach (User user in allActiveUsers)
            {
                var aContact = new Contact()
                {
                    PuserId = user.Id,
                    SuserId = contact.PuserId
                };
                var contactInfo = await Functions.APIService.GetContact(aContact, token);
                if (contactInfo == null)
                {
                    allAvailableUsers.Add(user);
                }
            }
            ViewData["SegundoUserId"] = new SelectList(allAvailableUsers, "Id", "Username");
            return View(contact);
        }
        [Authorize]
        public async Task<IActionResult> Delete(int ID)
        {
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            _toastNotification.AddInfoToastMessage("Seleccione el usuario que desea eliminar de sus contactos.");
            ViewBag.ID = ID;
            var alternateList = new List<User>() { await Functions.APIService.GetUserbyID(ID) };
            ViewBag.PrimerUserId = new SelectList(alternateList, "Id", "Username");
            var lista = await Functions.APIService.GetContactList(ID, token);
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
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            var id = contact.PuserId;
            var contactInfo = await Functions.APIService.GetContact(contact,token);
            if (await Functions.APIService.DeleteContact(contactInfo, token))
            {
                return RedirectToAction("Index", "Chats", new { @ID = contact.PuserId });
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Eliminación de contacto incorrecta.");
            }
            ViewBag.ID = contact.PuserId;
            var alternateList = new List<User>() { await Functions.APIService.GetUserbyID(contact.PuserId) };
            ViewData["PrimerUserId"] = new SelectList(alternateList, "Id", "Username");
            var lista = await Functions.APIService.GetContactList(contact.PuserId, token);
            var listaNombreContactos = new List<User>();
            foreach (var contacts in lista)
            {
                User user;
                if (contacts.PuserId == id)
                {
                    user = await Functions.APIService.GetUserbyID(contacts.SuserId);
                    if (user != null)
                    {
                        listaNombreContactos.Add(user);
                    }
                }
                else if (contacts.SuserId == id)
                {
                    user = await Functions.APIService.GetUserbyID(contacts.PuserId);
                    if (user != null)
                    {
                        listaNombreContactos.Add(user);
                    }
                }
            }
            ViewData["SegundoUserId"] = new SelectList(listaNombreContactos, "Id", "Username");
            return View(contactInfo);
        }
    }
}
