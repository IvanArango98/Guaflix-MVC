using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_.Interfaces
{
    internal interface INodo<TKey, TValor, TNodo, TArbolBPlus> where TKey : IComparable<TKey>
    {
        //Nodo Padre
        TNodo NodoPadre { get; }

        //Arbol Padre
        TArbolBPlus ArbolPadre { get; }

        //Nodos internos (Enlaces)
        ArbolBPlusEnlaces<TKey, TValor> Enlaces { get; }
        //Nodos hoja (Valores)
        ArbolBPlusValores<TKey, TValor> Valores { get; }

        //Maximo y minimo grado del arbol B+
        int GradoMaximo { get; }
        int GradoMinimo { get; }

        bool EsHoja { get; }
        bool EsRaiz { get; }
    }
}