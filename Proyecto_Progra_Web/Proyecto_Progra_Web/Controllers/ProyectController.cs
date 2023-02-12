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
    }
}
