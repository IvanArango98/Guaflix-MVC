using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto1_Guaflix_1158116_1171316.Models;
using System.IO;
using Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B;

namespace Proyecto1_Guaflix_1158116_1171316.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        [HttpGet]
        public ActionResult IniciarSesionGuaflix()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IniciarSesionGuaflix(IniciarSesion iniciarSesion)
        {
            //Se valida que al ingresar sea el usuario y redireccionar al menu de admin
            if (iniciarSesion.usuario == "admin" && iniciarSesion.contraseña == "admin")
            {
                //Se crea una carpeta en la cual llevera el contenido de toda la pagina web.
                Directory.CreateDirectory(@"C:\Proyecto1");        
                ViewBag.Message = "Inicio de sesion exitoso.";
                return RedirectToAction("VisualizarCatalogo", "Peliculas");

            }

            //Si la carpeta existe con el nombre de users.tree se leera los archivos de usuarios previamente agregados.
            if (System.IO.File.Exists(@"C:\Proyecto1\Users.tree"))
            {              
                List<string> JsonUsers = new List<string>();
                    var location = @" C:\Proyecto1\Users.tree";
                    using (StreamReader leer = new StreamReader(location))
                    {
                        int i = 0;
                        while (!leer.EndOfStream)
                        {
                            string x = leer.ReadLine();
                            JsonUsers.Add(x);
                            i++;
                        }
                    }
                    //Desearializar json
                    List<string> listaNombreUsuario = new List<string>();
                    List<string> listaContraseña = new List<string>();
                    ArbolB ArbolUsuarios = new ArbolB(3);

                   //Agregar informacion del archivo users.tree al arbol B de usuarios
                    for (int i = 0; i < JsonUsers.Count; i++)
                    {
                        CrearUsuario usuarios = JsonConvert.DeserializeObject<CrearUsuario>(JsonUsers.ElementAt(i));
                        listaNombreUsuario.Add(usuarios.UserName);
                        listaContraseña.Add(usuarios.Password);
                        ArbolUsuarios.Insertar(listaNombreUsuario.ElementAt(i) + " " + listaContraseña.ElementAt(i));
                    }
                                      
                    bool condicionUsuario = false;
                    bool condicionContraseña = false;

                    //Si el usuario existe y su usuario y contraseña son validas se entrara la sesion.
                    for (int i = 0; i < listaNombreUsuario.Count; i++)
                    {
                        if (iniciarSesion.usuario == listaNombreUsuario.ElementAt(i) && iniciarSesion.contraseña == listaContraseña.ElementAt(i))
                        {
                            condicionUsuario = true;
                            condicionContraseña = true;
                        }                      
                    }
                    if (condicionUsuario == true && condicionContraseña == true)
                    {
                    //Se crea un archivo con el usuario actual para poder interactuar con el nombre del usuario para crear el archivo de su watchlist.
                    ViewBag.Message = "Inicio de sesion exitoso.";
                    string nombreusuarioactual = iniciarSesion.usuario;
                    string rutaUsuario = @" C:\Proyecto1\UsuarioActual.txt";
                    StreamWriter swNombre = new StreamWriter(rutaUsuario, true);
                    swNombre.WriteLine(nombreusuarioactual);
                    swNombre.Close();
                    return RedirectToAction("VisualizarCatalogoUsuario", "Peliculas");
                    }
                    if(condicionUsuario == false && condicionContraseña == false)
                {
                    ViewBag.Message = "Usuario invalido.";
                }                
            }                    
            return View();
        }
    }
}