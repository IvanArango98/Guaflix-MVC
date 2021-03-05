using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B
{
    class ArbolBHoja : Nodo
    {
        //Constructor
        public ArbolBHoja(int TamañoNodo) : base(TamañoNodo)
        {
        }

        /// <summary>
        /// Metodo de insercion, mete el valor "valor" a la hoja 
        /// </summary>
        /// <param name="valor">El valor para insertar</param>
        /// <returns>returna un enum Agregar dependiendo de su estado
        /// Exito: fue agregado exitosamente
        /// HacerDivision: fue agregado extiosamente pero se tiene que hacer una division
        /// Duplicar: valor duplicado por lo que no se inserta</returns>
        public Agregar Insertar(string valor)
        {
            //el estado del enum Agregar
            Agregar estado;

            //si encuentra un duplicado
            if (Valores.Contains(valor))
                estado = Agregar.Duplicar;
            //si no esta duplicado pero el espacio del Nodo esta lleno
            else if (Valores.Count == TamañoNodo)
            {
                estado = Agregar.HacerDivision;
                Valores.Add(valor);
                Valores.Sort();
            }
            else
            {
                //sino ingresar exitosamente el valor 
                estado = Agregar.Exito;
                Valores.Add(valor);
                Valores.Sort();
            }
            return estado;
        }
    }
}