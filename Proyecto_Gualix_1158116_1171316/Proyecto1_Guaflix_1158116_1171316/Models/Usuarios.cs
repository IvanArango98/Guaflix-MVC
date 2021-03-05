using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models
{
   //Clase extrae archivo json de usuarios.
    public class Usuarios
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string edad { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}