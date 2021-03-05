using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_
{
    [Serializable]
    public class ArbolBPlusHoja<TKey, TValor> where TKey : IComparable<TKey>
    {

        //Enlaces
        //getters and setter (private set para que solo sea accesible dentro del cuerpo de la clase o el struct)
        public ArbolBPlusEnlaces<TKey, TValor> Enlaces
        {
            get;
            private set;
        }

        //Valores
        //getters and setter (private set para que solo sea accesible dentro del cuerpo de la clase o el struct)
        public SortedDictionary<TKey, TValor> Valores
        {
            get;
            private set;
        }

        //NodoPadre
        public ArbolBPlusHoja<TKey, TValor> NodoPadre { get; private set; }
        //ArbolPadre
        public ArbolBPlus<TKey, TValor> ArbolPadre { get; private set; }

        //Maximo y minimo grado del arbol B+
        public int GradoMaximo
        {
            get;
            private set;
        }
        public int GradoMinimo
        {
            get;
            private set;
        }

        /// <summary>
        /// Booleano que retorna true si el Nodo es Hoja y false si no es Nodo Hoja
        /// </summary>
        public bool EsHoja
        {
            get;
            private set;
        }

        /// <summary>
        /// Booleano que retorna true si el Nodo es Raiz y false si no es Nodo Raiz
        /// </summary>
        public bool EsRaiz
        {
            get;
            private set;
        }
    }
}