using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models
{
    //Clase creada para extraer archivo json de peliculas.
    public class Movies
    {     
        public string name { get; set; }
        public string year { get; set; }
        public string type { get; set; }
        public string genre { get; set; }
    }
}