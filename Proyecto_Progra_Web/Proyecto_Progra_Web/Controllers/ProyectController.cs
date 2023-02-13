using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_Progra_Web.Models;
using Proyecto_Progra_Web.Controllers;

namespace Proyecto_Progra_Web.Controllers
{
    public class ProyectController : Controller
    {
        public IActionResult login()
        {
            return View();
        }
        public IActionResult main()
        {
            return View("main");
        }
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
