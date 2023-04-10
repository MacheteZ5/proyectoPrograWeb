using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PPW.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PPW.Controllers
{
    public class UsersController : Controller
    {
        private readonly IToastNotification _toastNotification;
        public UsersController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> main([Bind("Username,Password")] User user)
        {
            var username = user.Username;
            var password = user.Password;
            if ((username != "" && password != "") && (username != null && password != null))
            {
                if (await Functions.APIService.GetValidationUser(username, 0))
                {
                    var userInformation = await Functions.APIService.GetUser(username);
                    if (userInformation.Password == password)
                    {
                        _toastNotification.AddSuccessToastMessage("Login realizado exitosamente.");
                        return RedirectToAction("Index", "Chats", new { @ID = userInformation.Id });
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Contraseña incorrecta.");
                    }
                }
                else
                {
                    _toastNotification.AddWarningToastMessage("El usuario ingresado no existe.");
                }
            }
            else
            {
                _toastNotification.AddWarningToastMessage("Completar los campos correspondientes.");
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            _toastNotification.AddInfoToastMessage("Llenar cada campo con la información solicitada.");
            ViewBag.Genero = new List<string>() { "Masculino", "Femenino" };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,StatusId,FirstName,LastName,Phone,Birthdate,Email,Genero,FecTransac")] User user)
        {
            var username = user.Username;
            if (!await Functions.APIService.GetValidationUser(username, 1))
            {
                var genero = (Request.Form["Genero"] == "Masculino") ? true : false;
                user.Genero = genero;
                user.Username = username.ToUpper();
                user.StatusId = 1;
                if (await Functions.APIService.SetUser(user))
                {
                    _toastNotification.AddSuccessToastMessage("Creación de usuario exitosa.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Error al crear usuario.");
                }
            }
            else
            {
                _toastNotification.AddWarningToastMessage("El usuario ingresado ya existe.");
            }
            ViewBag.Genero = new List<string>() { "Masculino", "Femenino" };
            return View(user);
        }
        public async Task<IActionResult> Details(int id)
        {
            var user = await Functions.APIService.GetUserbyID(id);
            ViewBag.Genero = (user.Genero == true) ? "Masculino" : "Femenino";
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
                if (oldUser != null)
                {
                    oldUser.Password = password;
                    if (await Functions.APIService.updateUser(oldUser))
                    {
                        _toastNotification.AddSuccessToastMessage("Cambio de contraseña exitoso.");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Error al realizar el cambio de contraseña.");
                    }
                }
                else
                {
                    _toastNotification.AddWarningToastMessage("No se pudo obtener la información correspondiente del usuario.");
                }
            }
            else
            {
                _toastNotification.AddWarningToastMessage("El usuario ingresado no existe.");
            }
            return View(user);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = await Functions.APIService.GetUserbyID(id);
            ViewBag.Genero = (user.Genero == true) ? "Masculino" : "Femenino";
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await Functions.APIService.GetUserbyID(id);
            user.StatusId = 2;
            if (await Functions.APIService.DisableUser(user))
            {
                _toastNotification.AddSuccessToastMessage("Usuario deshabilitado exitosamente.");
                return RedirectToAction(nameof(Index));
            }
            _toastNotification.AddErrorToastMessage("Error al deshabilitar usuario.");
            return RedirectToAction("Delete", "Users", new { @ID = id });
        }

        public async Task<JsonResult> GetUserJson()
        {
            var userId = Convert.ToInt32(HttpContext.Request.Form["UserId"].FirstOrDefault().ToString());
            var user = await Functions.APIService.GetUserbyID(userId);
            var jsonresult = new { user = user };
            return Json(jsonresult);
        }
    }
}
