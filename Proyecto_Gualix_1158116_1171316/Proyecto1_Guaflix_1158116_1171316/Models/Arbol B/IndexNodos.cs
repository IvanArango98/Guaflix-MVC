using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B
{
    class ArbolBIndex : Nodo
    {
        //Indices de Nodos
        public List<Nodo> IndexNodos { get; set; }

        //constructor
        public ArbolBIndex(int TamañoNodo) : base(TamañoNodo)
        {
            IndexNodos = new List<Nodo>(TamañoNodo);
        }

        /// <summary>
        /// Inserta el valor y el Nodo al index y los valores y Nodos se ordenan despues de la insercion
        /// returna el estado del enum Insertar
        /// </summary>
        /// <param name="valor">The value to add.</param>
        /// <param name="nodo">The node to add.</param>
        /// <returns>returna un enum Agregar dependiendo de su estado
        /// Exito: fue agregado exitosamente
        /// HacerDivision: fue agregado extiosamente pero se tiene que hacer una division
        /// Duplicar: valor duplicado por lo que no se inserta</returns>
        public Agregar Insertar(string valor, Nodo nodo)
        {
            //el estado del enum Agregar
            Agregar estado;
            if (Valores.Contains(valor))
            {
                //si se encuentra un duplicado
                estado = Agregar.Duplicar;
            }
            else if (IndexNodos.Count == TamañoNodo)
            {
                //si no esta duplicado pero el espacio del Nodo esta lleno
                estado = Agregar.HacerDivision;
                Valores.Add(valor);
                IndexNodos.Add(nodo);
                Sort();
            }
            else
            {
                //insertado exitosamente
                Valores.Add(valor);
                IndexNodos.Add(nodo);
                Sort();
                estado = Agregar.Exito;
            }
            return estado;
        }

        /// <summary>
        /// ordena la lista de valores y de Nodos
        /// </summary>
        public void Sort()
        {
            Valores.Sort();
            IndexNodos.Sort();
        }
    }
}