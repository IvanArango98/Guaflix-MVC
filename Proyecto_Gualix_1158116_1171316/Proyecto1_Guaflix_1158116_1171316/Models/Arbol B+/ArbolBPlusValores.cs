using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_
{
    [Serializable]
    public class ArbolBPlusValores<TKey, TValor> : SortedList<TKey, TValor> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// metodo que agrega a la lista de valores los "Keys" y los "Values" 
        /// </summary>
        /// <param name="valores">lista de los valores</param>
        /// <param name="copiarDeIndex">variable con la cual empieza el ciclo for para la agregacion de Keys y Values</param>
        /// <param name="copiarAIndex">variable con la cual es el limite del ciclo para agregar a la lista de valores</param>
        public void Copiar(ArbolBPlusValores<TKey, TValor> valores, int CopiarDeIndex, int copiarAIndex)
        {
            try
            {
                for (int i = CopiarDeIndex; i < copiarAIndex; i++)
                {
                    valores.Add(this.Keys[i], this.Values[i]);
                }
            }
            catch (Exception)
            {
                //try catch si el Index esta fuera de rango al copiar el ArbolBPlusValores
                throw new IndexOutOfRangeException();
            }
        }

        //Agregar Rango
        public void AgregarRango(IEnumerable<KeyValuePair<TKey, TValor>> valores)
        {
            foreach (KeyValuePair<TKey, TValor> v in valores)
            {
                this.Add(v.Key, v.Value);
            }
        }
    }
}