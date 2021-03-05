using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models
{
    //Clase creada para extraer si existen la pelicula en el arbolB+, y poder crear una vista en base a este clase.
    public class IniciarSesion
    {  
        public string usuario { get; set; }
        public string contraseña { get; set; }
    }
}