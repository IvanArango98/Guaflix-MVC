using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto1_Guaflix_1158116_1171316.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Directory.CreateDirectory(@"C:\Proyecto1");        
            return View();
        }

        public ActionResult About()
        {
            //Se redirecciona a esta vista si ocurre un error para los usuarios.
            ViewBag.Message = "Aplicacion en mantenimiento para usuarios.";

            return View();
        }

        public ActionResult Contact()
        {
            //Se redirecciona a esta vista si ocurre un error para el administrador.
            ViewBag.Message = "Parece que ha ocurrido un error.";

            return View();
        }
    }
}