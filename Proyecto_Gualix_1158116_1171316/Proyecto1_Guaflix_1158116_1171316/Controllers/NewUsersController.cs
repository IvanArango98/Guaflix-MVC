using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Proyecto1_Guaflix_1158116_1171316.Models;
using Proyecto1_Guaflix_1158116_1171316.Controllers;
using Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B;
using Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_;

namespace Proyecto1_Guaflix_1158116_1171316.Controllers
{
    public class NewUsersController : Controller
    {
        // GET: NewUsers
        [HttpGet]
        public ActionResult Register()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Register(CrearUsuario NuevoUsuario)
        {
            //Se valida que todos los textbox contegan informacion para agregar.
            if (NuevoUsuario.Nombre != null && NuevoUsuario.Apellido != null && NuevoUsuario.Edad != null && NuevoUsuario.UserName != null && NuevoUsuario.Password != null && NuevoUsuario.ConfirmPassword != null)
            {
                //Se crea una variable de tipo CrearUsuario en la cual se alacenara todo los valores que el usuario ingresara para crear su usuario.
                NuevoUsuario = new CrearUsuario
                {
                    Nombre = NuevoUsuario.Nombre,
                    Apellido = NuevoUsuario.Apellido,
                    Edad = NuevoUsuario.Edad,
                    UserName = NuevoUsuario.UserName,
                    Password = NuevoUsuario.Password,
                    ConfirmPassword = NuevoUsuario.ConfirmPassword
                };

                //Definir variables
                ArbolBPlus<string, string> arbolBPlusUsuarios = new ArbolBPlus<string, string>(4);
                List<string> listaNombreUsuario = new List<string>();
                List<string> JsonUsers = new List<string>();
                bool condicion = false;
                
                //Validar que el archivo de usuarios existan en el sistema.
                if (System.IO.File.Exists(@"C:\Proyecto1\Users.tree"))
                {                    
                    //Leer archivos que contienen los usuarios
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
                    //Desearializar json de usuarios y almacenar la informacion del usuario
                    for (int i = 0; i < JsonUsers.Count; i++)
                    {
                        CrearUsuario usuarios = JsonConvert.DeserializeObject<CrearUsuario>(JsonUsers.ElementAt(i));
                        listaNombreUsuario.Add(usuarios.UserName);                
                    }

                }

                //Validar que el usuario que desee crear no exista un usuario ya creado con el mismo nombre antes.
                for (int i = 0; i < listaNombreUsuario.Count; i++)
                {
                    if (NuevoUsuario.UserName == listaNombreUsuario.ElementAt(i))
                    {
                        condicion = true;
                        break;
                    }
                }

                //Si ya existe un usuario con el mismo nombre no permitira avanzar.
                if (condicion == true)
                {
                    ViewBag.Message = "Usuario ya existente, pruebe uno diferente.";
                }

                //Si no existe un usuario con el mismo nombre y que la contraseñas condicidan se creara un nuevo nodo con el usuario.

                if (NuevoUsuario.Password == NuevoUsuario.ConfirmPassword && condicion == false)
                {

                    Directory.CreateDirectory(@" C:\Proyecto1");                 
                    string location = @"C:\Proyecto1\Users.tree";
                    string location2 = @"C:\Proyecto1\Users.txt";
                                

                        string convertir = JsonConvert.SerializeObject(NuevoUsuario);
                        StreamWriter writer = new StreamWriter(location, true);                         
                        writer.WriteLine(convertir);                     
                        writer.Dispose();                     
                        ViewBag.Message = "Usuario creado exitosamente.";

                    //Almacenar arbol de usuarios en disco local c:
                    #region

                    //deserealizar 
                    List<string> Json = new List<string>();
                    List<string> ListNombre = new List<string>();
                    using (StreamReader leer = new StreamReader(location))
                    {
                        int i = 0;
                        while (!leer.EndOfStream)
                        {
                            string x = leer.ReadLine();
                            Json.Add(x);
                            i++;
                        }
                    }


                    //Desearializar json
                    for (int i = 0; i < Json.Count; i++)
                    {
                        CrearUsuario usuario = JsonConvert.DeserializeObject <CrearUsuario>(Json.ElementAt(i));
                        ListNombre.Add(usuario.UserName);
                        arbolBPlusUsuarios.Insertar(ListNombre.ElementAt(i),Json.ElementAt(i));                     
                    }

                      #region
                            var list = new List<KeyValuePair<string, string>>();
                            list.Add(new KeyValuePair<string, string>("B", "A"));
                            List<string> MostrarArbol = new List<string>();

                            Nodo<string, string> pivote = arbolBPlusUsuarios.EncontrarHojaPorKey(list.ElementAt(0).Key);
                            if (pivote.Valores != null)
                            {
                                for (int i = 0; i < pivote.Valores.Count; i++)
                                {
                                    MostrarArbol.Add(pivote.Valores.ElementAt(i).ToString());
                                }
                            }
                            if (pivote.NodoHojaDerecho != null)
                            {
                                for (int i = 0; i < pivote.NodoHojaDerecho.Valores.Count; i++)
                                {
                                    MostrarArbol.Add(pivote.NodoHojaDerecho.Valores.ElementAt(i).ToString());
                                }
                                if (pivote.NodoHojaDerecho.NodoHojaDerecho != null)
                                {
                                    for (int i = 0; i < pivote.NodoHojaDerecho.NodoHojaDerecho.Valores.Count; i++)
                                    {

                                        MostrarArbol.Add(pivote.NodoHojaDerecho.NodoHojaDerecho.Valores.ElementAt(i).ToString());
                                    }
                                    if (pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho != null)
                                    {
                                        for (int i = 0; i < pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.Count; i++)
                                        {

                                            MostrarArbol.Add(pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.ElementAt(i).ToString());
                                        }
                                        if (pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho != null)
                                        {
                                            for (int i = 0; i < pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.Count; i++)
                                            {

                                                MostrarArbol.Add(pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.ElementAt(i).ToString());
                                            }
                                            if (pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho != null)
                                            {
                                                for (int i = 0; i < pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.Count; i++)
                                                {

                                                    MostrarArbol.Add(pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.ElementAt(i).ToString());
                                                }
                                                if (pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho != null)
                                                {
                                                    for (int i = 0; i < pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.Count; i++)
                                                    {

                                                        MostrarArbol.Add(pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.ElementAt(i).ToString());
                                                    }
                                                    if (pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho != null)
                                                    {
                                                        for (int i = 0; i < pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.Count; i++)
                                                        {

                                                            MostrarArbol.Add(pivote.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.NodoHojaDerecho.Valores.ElementAt(i).ToString());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (pivote.NodoHojaIzquierdo != null)
                            {
                                for (int i = 0; i < pivote.NodoHojaIzquierdo.Valores.Count; i++)
                                {

                                    MostrarArbol.Add(pivote.NodoHojaIzquierdo.Valores.ElementAt(i).ToString());
                                }
                                if (pivote.NodoHojaIzquierdo.NodoHojaIzquierdo != null)
                                {
                                    for (int i = 0; i < pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.Count; i++)
                                    {
                                        MostrarArbol.Add(pivote.NodoHojaIzquierdo.Valores.ElementAt(i).ToString().ToString());
                                    }
                                    if (pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo != null)
                                    {
                                        for (int i = 0; i < pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.Count; i++)
                                        {

                                            MostrarArbol.Add(pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.ElementAt(i).ToString());
                                        }
                                        if (pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo != null)
                                        {
                                            for (int i = 0; i < pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.Count; i++)
                                            {

                                                MostrarArbol.Add(pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.ElementAt(i).ToString());
                                            }
                                            if (pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo != null)
                                            {
                                                for (int i = 0; i < pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.Count; i++)
                                                {

                                                    MostrarArbol.Add(pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.ElementAt(i).ToString());
                                                }
                                                if (pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo != null)
                                                {
                                                    for (int i = 0; i < pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.Count; i++)
                                                    {

                                                        MostrarArbol.Add(pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.ElementAt(i).ToString());
                                                    }
                                                    if (pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo != null)
                                                    {
                                                        for (int i = 0; i < pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.Count; i++)
                                                        {

                                                            MostrarArbol.Add(pivote.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.NodoHojaIzquierdo.Valores.ElementAt(i).ToString());
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            #endregion

                            using (StreamWriter writer2 = new StreamWriter(location2))
                            {
                                for (int i = 0; i < MostrarArbol.Count; i++)
                                {
                                    writer2.WriteLine(MostrarArbol.ElementAt(i));
                                }
                            }

                    #endregion

                    return RedirectToAction("IniciarSesionGuaflix", "LogIn");
                    }
                    if (NuevoUsuario.Password != NuevoUsuario.ConfirmPassword)
                    {
                        ViewBag.Message = "Contraseñas no coiciden, intente de nuevo.";
                    }
                
            }
            else
            {
                ViewBag.Message = "Debe de llenar todos los campos que se le piden.";
            }
            return View();
        }
    }
}