using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;
using PPW.Models;
using System.Net;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace PPW.Controllers
{
    public class UsersController : Controller
    {
        private readonly IToastNotification _toastNotification;
        public UsersController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
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
                        var jwttoken = await Functions.APIService.GetToken(userInformation);
                        if (jwttoken != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim("username", username),
                                new Claim("TokenAPI", jwttoken.token),
                                new Claim("DateTimeExpirationTokenAPI", jwttoken.expirationTime.ToString())
                            };
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                            await HttpContext.SignInAsync(claimsPrincipal);

                            /*#region coockiepropia
                            var key = "coockie";
                            var value = jwttoken.token;
                            var myCookie = new CookieOptions();
                            myCookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Append(key, value, myCookie);
                            #endregion coockiepropia*/
                            _toastNotification.AddSuccessToastMessage("Login realizado exitosamente.");
                            return RedirectToAction("Index", "Chats", new { @ID = userInformation.Id });
                        }
                        else
                        {
                            _toastNotification.AddErrorToastMessage("Error durante la generación del Token.");
                        }
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
        [Authorize]
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
            var token = User.Claims.FirstOrDefault(s => s.Type == "TokenAPI")?.Value;
            var user = await Functions.APIService.GetUserbyID(id);
            user.StatusId = 2;
            if (await Functions.APIService.DisableUser(user,token))
            {
                _toastNotification.AddSuccessToastMessage("Usuario deshabilitado exitosamente.");
                await HttpContext.SignOutAsync();
                return RedirectToAction(nameof(Index));
            }
            _toastNotification.AddErrorToastMessage("Error al deshabilitar usuario.");
            return RedirectToAction("Delete", "Users", new { @ID = id });
        }
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
