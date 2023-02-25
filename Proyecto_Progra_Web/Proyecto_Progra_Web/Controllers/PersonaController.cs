using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Progra_Web.Models;
using System.Net;
using System.Net.Http;

namespace Proyecto_Progra_Web.Controllers
{
    public class PersonaController : Controller
    {
        static ProgramacionWebContext context = new ProgramacionWebContext();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult verificarUsuario(string user)
        {
            //var userName = user;
            //var password = Request.Form["Password"].ToString();
            //List<Person> personas = context.People.ToList();
            //userName = userName.Trim();
            return View("mantenimiento-create");
        }

        [HttpPost]
        public IActionResult crearUsuario(string user, string password)
        {
            var newUser = new User()
            {
                Username = user,
                Password = password
            };
            context.Users.Add(newUser);
            context.SaveChanges();
            return RedirectToAction("~/Views/Usuario/login");
        }
        [HttpPut]
        public async Task<HttpStatusCode> actualizarInformacion(User usuario)
        {

            var entity = await context.Users.FirstOrDefaultAsync(s => s.Id == usuario.Id);

            entity.Username = usuario.Username;
            entity.Password = usuario.Password;

            await context.SaveChangesAsync();
            return HttpStatusCode.OK;

            //eliminacion
            /* var entity = new User()
    {
        Id = Id
    };
    DBContext.User.Attach(entity);
    DBContext.User.Remove(entity);
    await DBContext.SaveChangesAsync();
    return HttpStatusCode.OK;*/
        }

    }
}
