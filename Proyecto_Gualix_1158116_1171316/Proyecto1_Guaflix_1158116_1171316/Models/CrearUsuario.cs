using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models
{
    //Clase creada para desealizar y seralizar a los usuarios. Y recolectar informacion en la vista.
    public class CrearUsuario
    {      
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Edad { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}