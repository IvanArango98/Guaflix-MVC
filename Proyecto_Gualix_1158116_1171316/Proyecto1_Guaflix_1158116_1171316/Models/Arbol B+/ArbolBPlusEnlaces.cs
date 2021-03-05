using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_
{
    [Serializable]
    public class ArbolBPlusEnlaces<TKey, TValor> : SortedList<TKey, Nodo<TKey, TValor>> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// metodo que agregar Nodo con su Key respectivo y se va a metodo "AgregarConPadreConVecinos" 
        /// siendo los nodos izquierdo y derecho null
        /// </summary>
        /// <param name="padre">Nodo Padre</param>
        /// <param name="key">Key que contiene el Nodo a agregar</param>
        /// <param name="nodo">Nodo a agregar</param>
        public void AgregarConPadre(Nodo<TKey, TValor> padre, TKey key, Nodo<TKey, TValor> nodo)
        {
            AgregarConPadreConVecinos(padre, null, null, key, nodo);
        }

        /// <summary>
        /// metodo que agrega el Nodo ingresado con su Key siendo el Nodo ingresado como Nodo Padre
        /// </summary>
        /// <param name="padre">Nodo Padre</param>
        /// <param name="NodoIzquierdo">Nodo Izquierdo</param>
        /// <param name="NodoDerecho">Nodo Derecho</param>
        /// <param name="key">Key del Nodo</param>
        /// <param name="nodo">Nodo a ingresar</param>
        public void AgregarConPadreConVecinos(Nodo<TKey, TValor> padre, Nodo<TKey, TValor> NodoIzquierdo, Nodo<TKey, TValor> NodoDerecho, TKey key, Nodo<TKey, TValor> nodo)
        {
            Add(key, nodo);

            nodo.NodoPadre = padre;
            nodo.ArbolPadre = padre.ArbolPadre;

            nodo.NodoDerecho = NodoDerecho;
            nodo.NodoIzquierdo = NodoIzquierdo;
        }

        //Agregar Rango
        public void AgregarRango(Nodo<TKey, TValor> padre, IEnumerable<KeyValuePair<TKey, Nodo<TKey, TValor>>> coleccion)
        {
            foreach (KeyValuePair<TKey, Nodo<TKey, TValor>> c in coleccion)
            {
                this.Add(c.Key, c.Value);
                c.Value.NodoPadre = padre;
            }
        }

        //Agregar Rango hasta el inicio
        public void AgregarRangoInicial(Nodo<TKey, TValor> padre, IEnumerable<KeyValuePair<TKey, Nodo<TKey, TValor>>> coleccion)
        {
            if (coleccion != null)
            {
                int contadorPreInsertar = this.Count;

                foreach (KeyValuePair<TKey, Nodo<TKey, TValor>> c in coleccion)
                {
                    this.Add(c.Key, c.Value);
                }

                for (int i = 0; i < this.Count - contadorPreInsertar; i++)
                {
                    this.Values[i].NodoPadre = padre;
                    this.Values[i].ArbolPadre = padre.ArbolPadre;

                    if (i != this.Count - 1)
                    {
                        this.Values[i].NodoDerecho = this.Values[i + 1];
                        this.Values[i + 1].NodoIzquierdo = this.Values[i];
                    }
                }
                this.Values[0].NodoIzquierdo = null;
            }
        }

        //Agregar Rango hasta el final
        public void AgregarRangoFinal(Nodo<TKey, TValor> padre, IEnumerable<KeyValuePair<TKey, Nodo<TKey, TValor>>> coleccion)
        {
            if (coleccion != null)
            {
                int contadorPreInsertar = this.Count;

                foreach (KeyValuePair<TKey, Nodo<TKey, TValor>> c in coleccion)
                {
                    this.Add(c.Key, c.Value);
                }

                for (int i = contadorPreInsertar; i < this.Count; i++)
                {
                    this.Values[i].NodoPadre = padre;
                    this.Values[i].ArbolPadre = padre.ArbolPadre;

                    if (i != 0)
                    {
                        this.Values[i].NodoIzquierdo = this.Values[i - 1];
                        this.Values[i - 1].NodoDerecho = this.Values[i];
                    }
                }
                this.Values[this.Values.Count - 1].NodoDerecho = null;
            }
        }

        /// <summary>
        /// metodo que agrega a la lista de enlaces los "Keys" y los "Values" 
        /// </summary>
        /// <param name="enlaces">lista de los enlaces</param>
        /// <param name="copiarDeIndex">variable con la cual empieza el ciclo for para la agregacion de Keys y Values</param>
        /// <param name="copiarAIndex">variable con la cual es el limite del ciclo para agregar a la lista de enlaces</param>
        public void Copiar(ArbolBPlusEnlaces<TKey, TValor> enlaces, int copiarDeIndex, int copiarAIndex)
        {
            try
            {
                for (int i = copiarDeIndex; i < copiarAIndex; i++)
                {
                    enlaces.Add(this.Keys[i], this.Values[i]);
                }
            }
            catch (Exception)
            {
                //try catch si el Index esta fuera de rango al copiar el ArbolBPlusEnlaces
                throw new IndexOutOfRangeException();
            }
        }
    }
}