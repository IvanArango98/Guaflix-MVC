using Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_
{
    [Serializable]
    public class Nodo<TKey, TValor> : INodo<TKey, TValor, Nodo<TKey, TValor>, ArbolBPlus<TKey, TValor>> where TKey : IComparable<TKey>
    {
        //Constructor para Nodos internos
        public Nodo(ArbolBPlusEnlaces<TKey, TValor> enlaces, Nodo<TKey, TValor> nodoPadre, Nodo<TKey, TValor> nodoDerecho, Nodo<TKey, TValor> nodoIzquierdo, ArbolBPlus<TKey, TValor> arbolPadre, bool esRaiz)
        {
            Enlaces = enlaces;
            NodoPadre = nodoPadre;
            NodoDerecho = nodoDerecho;
            NodoIzquierdo = nodoIzquierdo;
            ArbolPadre = arbolPadre;
            EsRaiz = esRaiz;
            EsHoja = false;
        }

        //Constructor para Nodos Hoja
        public Nodo(ArbolBPlusValores<TKey, TValor> valores, Nodo<TKey, TValor> nodoPadre, Nodo<TKey, TValor> nodoHojaDerecho, Nodo<TKey, TValor> nodoHojaIzquierdo, ArbolBPlus<TKey, TValor> arbolPadre, bool esRaiz)
        {
            Valores = valores;
            NodoPadre = nodoPadre;
            NodoHojaDerecho = nodoHojaDerecho;
            NodoHojaIzquierdo = nodoHojaIzquierdo;
            ArbolPadre = arbolPadre;
            EsRaiz = esRaiz;
            EsHoja = true;
        }

        //Propiedades
        //miembros con internal set para que cooperen de manera privada sin estar expuestos al codigo
        //solo los del mismo ensamblado pueden tener acceso a ellas

        //miembro protegido (solo accesible dentro de la clase)
        protected ArbolBPlusEnlaces<TKey, TValor> enlaces;
        //Nodo Padre
        public Nodo<TKey, TValor> NodoPadre { get; internal set; }
        //Nodo Derecho
        public Nodo<TKey, TValor> NodoDerecho { get; internal set; }
        //Nodo Izquierdo
        public Nodo<TKey, TValor> NodoIzquierdo { get; internal set; }
        //Nodo Hoja Derecho
        public Nodo<TKey, TValor> NodoHojaDerecho { get; internal set; }
        //Nodo Hoja Izquierdo
        public Nodo<TKey, TValor> NodoHojaIzquierdo { get; internal set; }
        //Arbol Padre
        public ArbolBPlus<TKey, TValor> ArbolPadre { get; internal set; }

        //Enlaces
        //getters and setters con internal set (para que otro codigo que no sea de esta clase pueda leerlo pero no modificarlo)
        public ArbolBPlusEnlaces<TKey, TValor> Enlaces
        {
            get
            {
                return enlaces;
            }
            internal set
            {
                enlaces = value;
                if (enlaces != null)
                {
                    foreach (KeyValuePair<TKey, Nodo<TKey, TValor>> e in enlaces)
                    {
                        e.Value.NodoPadre = this;
                    }
                }
            }
        }

        //Valores
        //getters and setters con internal set (para que otro codigo que no sea de esta clase pueda leerlo pero no modificarlo)
        public ArbolBPlusValores<TKey, TValor> Valores
        {
            get;
            internal set;
        }

        //KeysCount 
        //getters and setters 
        public int KeysCount
        {
            get
            {
                if (this.EsHoja)
                {
                    return this.Valores.Keys.Count;
                }
                return this.Enlaces.Keys.Count;
            }
        }

        //GradoMaximo
        //getters
        public int GradoMaximo
        {
            get { return ArbolPadre.GradoMaximo; }
        }

        //GradoMinimo
        //getters
        public int GradoMinimo
        {
            get
            {
                return ArbolPadre.GradoMinimo;
            }
        }

        /// <summary>
        /// Booleano que returna true si el Nodo es hoja y false si no es Nodo hoja
        /// </summary>
        public bool EsHoja
        {
            get;
            protected set;
        }

        /// <summary>
        /// Booleano que returna true si el Nodo es raiz y false si no es Nodo raiz
        /// </summary>
        public bool EsRaiz
        {
            get;
            internal set;
        }
    }
}