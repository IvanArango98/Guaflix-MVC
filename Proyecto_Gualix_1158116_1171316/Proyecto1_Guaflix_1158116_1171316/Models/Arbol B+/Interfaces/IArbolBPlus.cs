using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_.Interfaces
{
    internal interface IArbolBPlus<TKey, TValor, Nodo> : ICollection<KeyValuePair<TKey, TValor>> where TKey : IComparable<TKey>
    {
        Nodo Arbol { get; set; }

        int GradoMaximo { get; }
        int GradoMinimo { get; }
        //Contador de elementos "Key"
        int ContadorKeys { get; }
        //Contador de Nodos
        int ContadorNodos { get; }
    }
}