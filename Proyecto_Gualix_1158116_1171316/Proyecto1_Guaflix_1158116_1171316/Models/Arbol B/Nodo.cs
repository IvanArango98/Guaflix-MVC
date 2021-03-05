using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B
{
    class Nodo : IComparable
    {
        //tamaño del nodo
        public int TamañoNodo { get; set; }
        //lista de valores
        public List<string> Valores { get; set; }

        //constructor
        public Nodo(int size)
        {
            TamañoNodo = size;
            Valores = new List<string>(TamañoNodo);
        }

        public int CompareTo(object objeto)
        {
            int compareValue;

            // si el objeto no es un Nodo
            if (!(objeto is Nodo))
                throw new ArgumentException("Nodo debe ser comparado con un Nodo");
            else
            {
                Nodo OtroNodo = objeto as Nodo;
                if (this.Valores.Count == 0)
                {
                    //si el Nodo esta vacio returnar 0
                    if (OtroNodo.Valores.Count == 0)
                        compareValue = 0;
                    //sino returna -1
                    else
                        compareValue = -1;
                }
                else
                {
                    //Si el otro nodo esta vacia retornar 1
                    if (OtroNodo.Valores.Count == 0)
                        compareValue = 1;
                    //sino returnar la comparacion del primer valor en cada Nodo
                    else
                        compareValue = this.Valores[0].CompareTo(OtroNodo.Valores[0]);
                }
            }
            return compareValue;
        }
    }
}