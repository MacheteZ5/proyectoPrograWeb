using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Progra_Web.Controllers;
using Proyecto_Progra_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Progra_Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ProgramacionWebContext context = new ProgramacionWebContext();
        public IActionResult login()
        {
            return View();
        }
        public IActionResult main()
        {
            return View("main");
        }
        [HttpGet]
        public async Task<ActionResult<User>> verificarUsuario(string user, string password)
        {
            var userEncontrado = await context.Users.Select(u=> new User
            {
                Id = u.Id,
                Username = u.Username,
                Password = u.Password,
                StatusId = u.StatusId,  
                IdPersona = u.IdPersona, 
                Status = u.Status
            }).FirstOrDefaultAsync(s=> s.Username == user);
            return (userEncontrado == null) ? NotFound() : View("main");//esto hace falta por corregir
        }


        [HttpPost]
        public IActionResult create()
        {
            return View("mantenimiento-create");
        }
        public IActionResult delete()
        {
            return View("mantenimiento-delete");
        }
        public IActionResult update()
        {
            return View("mantenimiento-update");
        }
        public IActionResult about()
        {
            return View("about");
        }

    }
}
