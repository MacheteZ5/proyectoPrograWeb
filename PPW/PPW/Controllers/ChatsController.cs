using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPW.Models;
using NToastNotify;

namespace PPW.Controllers
{
    public class ChatsController : Controller
    {
        private readonly ProgramacionWebContext _context;
        private readonly IToastNotification _toastNotification;

        public ChatsController(ProgramacionWebContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [Authorize]
        public async Task<IActionResult> Index(int ID)
        {
            var token = User.Claims.FirstOrDefault(s=> s.Type == "TokenAPI")?.Value;
            ViewBag.ID = ID;
            var listaNombreContactos = new List<User>();
            var lista = await Functions.APIService.GetContactsList(token);
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
            return View(listaNombreContactos.ToList());
        }
        [Authorize]
        public async Task<IActionResult> Create(int Id, int Contact)
        {
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            var contact = new Contact { PuserId = Id, SuserId = Contact };
            var contactInfo = await Functions.APIService.GetContact(contact, token);
            if (contactInfo != null)
            {
                ViewBag.ContactID = contactInfo.Id;
                ViewBag.Contact = Contact;
                var user = await Functions.APIService.GetUserbyID(Id);
                var sUser = await Functions.APIService.GetUserbyID(Contact);
                ViewBag.Username = user.Username;
                ViewBag.Contactname = sUser.Username;
                ViewBag.ID = Id;
                var chats = await Functions.APIService.GetChat(contactInfo.Id, token);
                return View(chats.ToList());
            }
            return RedirectToAction("Index", "Chats", Id);
        }
        [Authorize]
        public async Task<IActionResult> Menu(int Id, int Contact)
        {
            _toastNotification.AddInfoToastMessage("Seleccione un mensaje a modificar o eliminar.");
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            var contact = new Contact { PuserId = Id, SuserId = Contact };
            var contactInfo = await Functions.APIService.GetContact(contact, token);
            if (contactInfo != null)
            {
                ViewBag.ContactID = Contact;
                var user = await Functions.APIService.GetUserbyID(Id);
                var sUser = await Functions.APIService.GetUserbyID(Contact);
                ViewBag.Username = user.Username;
                ViewBag.Contactname = sUser.Username;
                ViewBag.ID = Id;
                var chats = await Functions.APIService.GetChat(contactInfo.Id, token);
                return View(chats.ToList());
            }
            return RedirectToAction("Index", "Chats", Id);
        }
        public IActionResult Return(int id)
        {
            return RedirectToAction("Index", "Chats", new { @ID = id });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id, int userID, int contact)
        {
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            var Chat = await Functions.APIService.GetChatbyID(id, token);
            if(Chat != null)
            {
                ViewBag.ID = userID;
                ViewBag.ContactID = contact;
                return View(Chat);
            }
            else
            {
                return RedirectToAction("Create", "Chats", new { @Id = userID, @Contact = contact });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int userID, int contact, [Bind("Id,ContactId,UserId,Mensaje,Archivos,FecTransac")] Chat chat)
        {
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            var oldChat = await Functions.APIService.GetChatbyID(chat.Id, token);
            if (oldChat != null)
            {
                oldChat.Mensaje = chat.Mensaje;
                if (await Functions.APIService.UpdateChat(oldChat, token))
                {
                    _toastNotification.AddSuccessToastMessage("Mensaje actualizado exitosamente.");
                    return RedirectToAction("Create", "Chats", new { @Id = userID, @Contact = contact });
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Error al realizar actualización de mensaje.");
                }
            }
            else
            {
                _toastNotification.AddWarningToastMessage("No se pudo obtener la información correspondiente del mensaje seleccionado.");
            }
            return View(chat);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id, int userID, int contact)
        {
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            var chat = await Functions.APIService.GetChatbyID(id, token);
            if(chat != null)
            {
                var user = await Functions.APIService.GetUserbyID(Convert.ToInt32(chat.UserId));
                ViewBag.UserName = user.Username;
                ViewBag.ID = userID;
                ViewBag.ContactID = contact;
                return View(chat);
            }
            else
            {
                return RedirectToAction("Create", "Chats", new { @Id = userID, @Contact = contact });
            }
        }        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int userID, int contact)
        {
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            var Chat = new Chat { Id = id };
            if (await Functions.APIService.DeleteChat(Chat, token))
            {
                _toastNotification.AddSuccessToastMessage("Mensaje eliminado exitosamente.");
                return RedirectToAction("Create", "Chats", new { @Id = userID, @Contact=contact});
            }
            _toastNotification.AddErrorToastMessage("Error al eliminar mensaje.");
            return RedirectToAction("Index", "Chats", new { @ID = userID});
        }
    }
}
