using Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B_
{
    [Serializable]
    public class ArbolBPlus<TKey, TValor> : IArbolBPlus<TKey, TValor, Nodo<TKey, TValor>> where TKey : IComparable<TKey>
    {
        //Constructores
        public ArbolBPlus() : this(null, 0)
        {
        }
        //ArbolBPlus definiendo su grado 
        public ArbolBPlus(int gradoArbol) : this(null, gradoArbol)
        {
        }
        //Lo que ingresa el usuario sera el grado del arbol y por consecuencia eso se vuelva el GradoMaximo
        public ArbolBPlus(IEnumerable<KeyValuePair<TKey, TValor>> coleccion, int gradoArbol)
        {
            GradoMaximo = gradoArbol;

            if (coleccion != null)
            {
                AgregarRango(coleccion);
            }
        }

        //Propiedades
        //Variable que tendra el gradoMaximo del arbol (grado a trabajar)
        private int gradoMaximo;

        public Nodo<TKey, TValor> Arbol { get; set; }
        //Si el grado ingresado es menor a 2, se tomara como grado 2, si es mayor a 2 se tomara el valor ingresado
        public int GradoMaximo
        {
            get { return gradoMaximo; }
            protected set { gradoMaximo = value < 2 ? 2 : value; }
        }

        //Siempre será grado 2 ya que es el grado minimo a trabajar en un arbol B+
        public int GradoMinimo
        {
            get
            {
                return (GradoMaximo >= 2) ? 2 : 2;
            }
        }

        //Contadores
        public int Count { get; protected set; }
        //Contador de las llaves "Keys"
        public int ContadorKeys
        {
            get
            {
                return Count;
            }
        }
        //Contador de los Nodos
        public int ContadorNodos { get; protected set; }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo que al ingresar una Key se intentará encontrar la hoja donde estará esa Key a encontrar
        /// </summary>
        /// <param name="key">La Key que se ingresara</param>
        /// <returns>Retornar el Nodo Hoja si se encuentra la Key, sino returnal null</returns>
        public Nodo<TKey, TValor> EncontrarHojaPorKey(TKey key)
        {
            if (Arbol != null)
            {
                Nodo<TKey, TValor> pivote = Arbol;

                while (!pivote.EsHoja)
                {
                    pivote = pivote.Enlaces.Values[EncontrarIndexEnlaces(pivote, key)];
                }
                return pivote;
            }
            return null;
        }

        /// <summary>
        /// Metodo que al ingresar el nodo y su respectiva llave Key, se encuentra el indice de la lista de enlaces donde 
        /// se situa el nodo y Key ingresados
        /// </summary>
        /// <param name="nodo">Nodo a ingresar</param>
        /// <param name="key">Key a ingresar</param>
        /// <returns>Returna la posicion donde esta situado el Nodo y Key de la lista de Enlaces</returns>
        protected int EncontrarIndexEnlaces(Nodo<TKey, TValor> nodo, TKey key)
        {
            for (int i = 0; i < nodo.Enlaces.Count; i++)
            {
                if (nodo.Enlaces.Keys[i].CompareTo(key) >= 0)
                {
                    return i;
                }
            }
            return nodo.Enlaces.Count - 1;
        }

        /// <summary>
        /// Encontrar valor en hoja al ingresar una llave "Key"
        /// </summary>
        /// <param name="hoja">Nodo hoja a ingresar</param>
        /// <param name="key">Key a ingresar</param>
        /// <param name="valor">valor a ingresar</param>
        /// <returns>returna true si encuentra el valor, y false si no encuentra el valor por Key</returns>
        protected bool EncontrarValorPorKey(Nodo<TKey, TValor> hoja, TKey key, out TValor valor)
        {
            return hoja.Valores.TryGetValue(key, out valor);
        }

        /// <summary>
        /// Metodo que verifica si es un Nodo o si es hoja overlfow para hacer DividirHoja o DividirNodo 
        /// </summary>
        /// <param name="NodoDividir">Nodo a ingresar para hacer Dividir</param>
        protected void Dividir(Nodo<TKey, TValor> NodoDividir)
        {
            ContadorNodos++;

            if (NodoDividir.EsHoja)
            {
                DividirHoja(NodoDividir);
            }
            else
            {
                DividirNodo(NodoDividir);
            }
        }

        /// <summary>
        /// metodo que hace dividir al nodo Hoja al estar overflow 
        /// </summary>
        /// <param name="HojaDividir">Nodo Hoja a ingresar para hacer dividir</param>
        protected void DividirHoja(Nodo<TKey, TValor> HojaDividir)
        {
            int IndexPivote = HojaDividir.Valores.Count / 2;

            ArbolBPlusValores<TKey, TValor> ValoresIzquierdos = new ArbolBPlusValores<TKey, TValor>();
            ArbolBPlusValores<TKey, TValor> ValoresDerechos = new ArbolBPlusValores<TKey, TValor>();

            HojaDividir.Valores.Copiar(ValoresIzquierdos, 0, IndexPivote + 1);
            HojaDividir.Valores.Copiar(ValoresDerechos, IndexPivote + 1, HojaDividir.Valores.Count);

            Nodo<TKey, TValor> NodoIzquierdo = new Nodo<TKey, TValor>(ValoresIzquierdos, HojaDividir.NodoPadre, null, null, HojaDividir.ArbolPadre, false);
            Nodo<TKey, TValor> NodoDerecho = new Nodo<TKey, TValor>(ValoresDerechos, HojaDividir.NodoPadre, null, null, HojaDividir.ArbolPadre, false);

            NodoIzquierdo.NodoHojaDerecho = NodoDerecho;
            NodoIzquierdo.NodoHojaIzquierdo = HojaDividir.NodoHojaIzquierdo;
            NodoDerecho.NodoHojaIzquierdo = NodoIzquierdo;
            NodoDerecho.NodoHojaDerecho = HojaDividir.NodoHojaDerecho;

            if (HojaDividir.NodoHojaIzquierdo != null)
            {
                HojaDividir.NodoHojaIzquierdo.NodoHojaDerecho = NodoIzquierdo;
            }
            if (HojaDividir.NodoHojaDerecho != null)
            {
                HojaDividir.NodoHojaDerecho.NodoHojaIzquierdo = NodoDerecho;
            }
            if (!HojaDividir.EsRaiz)
            {
                HojaDividir.NodoPadre.Enlaces.Remove(HojaDividir.Valores.Keys.Max());

                HojaDividir.NodoPadre.Enlaces.Add(NodoDerecho.Valores.Keys.Last(), NodoDerecho);
                HojaDividir.NodoPadre.Enlaces.Add(NodoIzquierdo.Valores.Keys.Last(), NodoIzquierdo);
            }
            else
            {
                Nodo<TKey, TValor> nuevaRaiz = new Nodo<TKey, TValor>(new ArbolBPlusEnlaces<TKey, TValor>(), null, null, null, HojaDividir.ArbolPadre, true);

                NodoIzquierdo.NodoPadre = nuevaRaiz;
                NodoDerecho.NodoPadre = nuevaRaiz;

                nuevaRaiz.Enlaces.Add(NodoIzquierdo.Valores.Keys.Last(), NodoIzquierdo);
                nuevaRaiz.Enlaces.Add(NodoDerecho.Valores.Keys.Last(), NodoDerecho);

                Arbol = nuevaRaiz;
                ContadorNodos++;
            }
            if (NodoIzquierdo.NodoPadre.KeysCount > GradoMaximo)
            {
                Dividir(NodoIzquierdo.NodoPadre);
            }
        }

        /// <summary>
        /// Metodo que divide un Nodo al estar en overflow
        /// </summary>
        /// <param name="NodoDividir">Nodo a ingresar para hacer dividir</param>
        protected void DividirNodo(Nodo<TKey, TValor> NodoDividir)
        {
            int IndexPivote = NodoDividir.Enlaces.Count / 2;

            ArbolBPlusEnlaces<TKey, TValor> EnlacesIzquierdos = new ArbolBPlusEnlaces<TKey, TValor>();
            ArbolBPlusEnlaces<TKey, TValor> EnlacesDerechos = new ArbolBPlusEnlaces<TKey, TValor>();

            NodoDividir.Enlaces.Copiar(EnlacesIzquierdos, 0, IndexPivote + 1);
            NodoDividir.Enlaces.Copiar(EnlacesDerechos, IndexPivote + 1, NodoDividir.Enlaces.Count);

            Nodo<TKey, TValor> NodoIzquierdo = new Nodo<TKey, TValor>(EnlacesIzquierdos, NodoDividir.NodoPadre, null, null, NodoDividir.ArbolPadre, false);
            Nodo<TKey, TValor> NodoDerecho = new Nodo<TKey, TValor>(EnlacesDerechos, NodoDividir.NodoPadre, null, null, NodoDividir.ArbolPadre, false);

            NodoIzquierdo.NodoDerecho = NodoDerecho;
            NodoIzquierdo.NodoIzquierdo = NodoDividir.NodoIzquierdo;
            NodoDerecho.NodoIzquierdo = NodoIzquierdo;
            NodoDerecho.NodoDerecho = NodoDividir.NodoDerecho;

            if (NodoDividir.NodoIzquierdo != null)
            {
                NodoDividir.NodoIzquierdo.NodoDerecho = NodoIzquierdo;
            }
            if (NodoDividir.NodoDerecho != null)
            {
                NodoDividir.NodoDerecho.NodoIzquierdo = NodoDerecho;
            }
            if (!NodoDividir.EsRaiz)
            {
                NodoDividir.NodoPadre.Enlaces.Remove(NodoDividir.Enlaces.Keys.Max());

                NodoDividir.NodoPadre.Enlaces.Add(NodoDerecho.Enlaces.Keys.Last(), NodoDerecho);
                NodoDividir.NodoPadre.Enlaces.Add(NodoIzquierdo.Enlaces.Keys.Last(), NodoIzquierdo);
            }
            else
            {
                Nodo<TKey, TValor> nuevaRaiz = new Nodo<TKey, TValor>(new ArbolBPlusEnlaces<TKey, TValor>(), null, null, null, NodoDividir.ArbolPadre, true);

                NodoIzquierdo.NodoPadre = nuevaRaiz;
                NodoDerecho.NodoPadre = nuevaRaiz;

                nuevaRaiz.Enlaces.Add(NodoIzquierdo.Enlaces.Keys.Last(), NodoIzquierdo);
                nuevaRaiz.Enlaces.Add(NodoDerecho.Enlaces.Keys.Last(), NodoDerecho);

                Arbol = nuevaRaiz;
                ContadorNodos++;
            }
            if (NodoIzquierdo.NodoPadre.KeysCount > GradoMaximo)
            {
                Dividir(NodoIzquierdo.NodoPadre);
            }
        }

        /// <summary>
        /// Metodo que ingresa al metodo Add con el Nodo con su Key ingresados al arbol B+
        /// </summary>
        /// <param name="key">Key ingresado</param>
        /// <param name="valor">Valor ingresado</param>
        public void Insertar(TKey key, TValor valor)
        {
            Add(new KeyValuePair<TKey, TValor>(key, valor));
        }

        /// <summary>
        /// Ingresa el "KeyValuePair" primero creando un Nodo pivote para encontrar si una hoja del arbol B+ ya tiene la Key que 
        /// estamos ingresando para ver si es un Nodo repetido. Luego que si sabe si que no es repetido lo ingresa por medio de 
        /// Icomparable de Nodos.
        /// </summary>
        /// <param name="KeyValuePair">Es lo que contiene el (key,valor) para ingresarlo al arbol como Nodo</param>
        public void Add(KeyValuePair<TKey, TValor> KeyValuePair)
        {
            Nodo<TKey, TValor> pivote = EncontrarHojaPorKey(KeyValuePair.Key);

            if (pivote != null)
            {
                if (pivote.Valores.ContainsKey(KeyValuePair.Key))
                {

                }
                if (pivote.Valores.Last().Key.CompareTo(KeyValuePair.Key) < 0 && pivote.NodoPadre != null)
                {
                    TKey KeyVieja = pivote.Valores.Keys.Last();
                    pivote.Valores.Add(KeyValuePair.Key, KeyValuePair.Value);
                    pivote.NodoPadre.Enlaces.Remove(KeyVieja);
                    pivote.NodoPadre.Enlaces.Add(KeyValuePair.Key, pivote);

                    Nodo<TKey, TValor> padre = pivote.NodoPadre;

                    while (padre.NodoPadre != null)
                    {
                        padre.NodoPadre.Enlaces.Remove(KeyVieja);
                        padre.NodoPadre.Enlaces.Add(KeyValuePair.Key, padre);
                        padre = padre.NodoPadre;
                    }

                }
                else
                {
                    if (!pivote.Valores.ContainsKey(KeyValuePair.Key))
                    {
                        pivote.Valores.Add(KeyValuePair.Key, KeyValuePair.Value);
                    }
                }
                if (pivote.KeysCount > GradoMaximo)
                {
                    Dividir(pivote);
                }
            }
            else
            {
                ArbolBPlusValores<TKey, TValor> list = new ArbolBPlusValores<TKey, TValor>();
                list.Add(KeyValuePair.Key, KeyValuePair.Value);
                Arbol = new Nodo<TKey, TValor>(list, null, null, null, this, true);

                ContadorNodos++;
            }

            Count++;
        }
        //AgregarRango
        public void AgregarRango(IEnumerable<KeyValuePair<TKey, TValor>> coleccion)
        {
            foreach (KeyValuePair<TKey, TValor> c in coleccion)
            {
                Add(c);
            }
        }

        //Unir-Extraer Hoja
        protected void UnirOExtraerHoja(Nodo<TKey, TValor> hoja)
        {
            if (hoja.NodoHojaDerecho != null && hoja.NodoHojaIzquierdo != null)
            {
                Nodo<TKey, TValor> hojaMinima = (hoja.NodoHojaIzquierdo.KeysCount < hoja.NodoHojaDerecho.KeysCount) ? hoja.NodoHojaIzquierdo : hoja.NodoHojaDerecho;
                Nodo<TKey, TValor> hojaMaximo = (hoja.NodoHojaIzquierdo.KeysCount >= hoja.NodoHojaDerecho.KeysCount) ? hoja.NodoHojaIzquierdo : hoja.NodoHojaDerecho;

                if (hoja.KeysCount + hojaMinima.KeysCount <= GradoMaximo)
                {
                    if (hoja.NodoHojaDerecho == hojaMinima)
                    {
                        UnirHojas(hoja, hojaMinima);
                    }
                    else
                    {
                        UnirHojas(hojaMinima, hoja);
                    }
                }
                else
                {
                    ExtraerHojas(hoja, hojaMaximo);
                }
            }
            else if (hoja.NodoHojaIzquierdo != null)
            {
                if (hoja.KeysCount + hoja.NodoHojaIzquierdo.KeysCount <= GradoMaximo)
                {
                    UnirHojas(hoja.NodoHojaIzquierdo, hoja);
                }
                else
                {
                    ExtraerHojas(hoja, hoja.NodoHojaIzquierdo);
                }
            }
            else if (hoja.NodoHojaDerecho != null)
            {
                if (hoja.KeysCount + hoja.NodoHojaDerecho.KeysCount <= GradoMaximo)
                {
                    UnirHojas(hoja, hoja.NodoHojaDerecho);
                }
                else
                {
                    ExtraerHojas(hoja, hoja.NodoHojaDerecho);
                }
            }
            else
            {
                if (!hoja.EsRaiz)
                {
                    hoja.EsRaiz = true;
                    hoja.NodoPadre = null;
                    Arbol = hoja;
                    ContadorNodos--;
                }
            }
        }

        //UnirHojas
        protected void UnirHojas(Nodo<TKey, TValor> hojaIzquierda, Nodo<TKey, TValor> hojaDerecha)
        {
            ContadorNodos--;
            Nodo<TKey, TValor> nuevaHoja = new Nodo<TKey, TValor>(new ArbolBPlusValores<TKey, TValor>(), null, null, null, hojaIzquierda.ArbolPadre, false);
            ArbolBPlusValores<TKey, TValor> ValoresUnidos = new ArbolBPlusValores<TKey, TValor>();

            ValoresUnidos.AgregarRango(hojaIzquierda.Valores);
            ValoresUnidos.AgregarRango(hojaDerecha.Valores);

            nuevaHoja.Valores = ValoresUnidos;
            nuevaHoja.NodoPadre = hojaDerecha.NodoPadre;
            nuevaHoja.NodoHojaIzquierdo = hojaIzquierda.NodoHojaIzquierdo;
            nuevaHoja.NodoHojaDerecho = hojaDerecha.NodoHojaDerecho;

            if (nuevaHoja.NodoHojaIzquierdo != null)
            {
                nuevaHoja.NodoHojaIzquierdo.NodoHojaDerecho = nuevaHoja;
            }
            if (nuevaHoja.NodoHojaDerecho != null)
            {
                nuevaHoja.NodoHojaDerecho.NodoHojaIzquierdo = nuevaHoja;
            }
            nuevaHoja.NodoPadre.Enlaces.Remove(hojaDerecha.Valores.Keys.Max());
            nuevaHoja.NodoPadre.Enlaces.Add(nuevaHoja.Valores.Keys.Max(), nuevaHoja);
            hojaIzquierda.NodoPadre.Enlaces.Remove(hojaIzquierda.Valores.Keys.Max());

            if (hojaIzquierda.NodoPadre != hojaDerecha.NodoPadre)
            {
                hojaIzquierda.NodoPadre.NodoPadre.Enlaces.Remove(hojaIzquierda.Valores.Keys.Max());
                hojaIzquierda.NodoPadre.NodoPadre.Enlaces.Add(hojaIzquierda.NodoPadre.Enlaces.Keys.Max(), hojaIzquierda.NodoPadre);
                if (hojaIzquierda.NodoPadre.KeysCount < GradoMinimo && !hojaIzquierda.NodoPadre.EsRaiz)
                {
                    UnirOExtraerNodo(hojaIzquierda.NodoPadre);
                }
            }
            if (nuevaHoja.NodoPadre.KeysCount < GradoMinimo && !nuevaHoja.NodoPadre.EsRaiz)
            {
                UnirOExtraerNodo(nuevaHoja.NodoPadre);
            }
            else if (nuevaHoja.NodoPadre.EsRaiz && nuevaHoja.NodoPadre.KeysCount == 1)
            {
                nuevaHoja.EsRaiz = true;
                nuevaHoja.NodoPadre = null;
                Arbol = nuevaHoja;
                ContadorNodos--;
            }
        }

        //ExtraerHojas
        protected void ExtraerHojas(Nodo<TKey, TValor> hojaAExtraer, Nodo<TKey, TValor> nuevaHoja)
        {
            int numeroValoresAExtraer = hojaAExtraer.KeysCount + (nuevaHoja.KeysCount - hojaAExtraer.KeysCount) / 2;
            if (hojaAExtraer.NodoHojaDerecho == nuevaHoja)
            {
                ArbolBPlusValores<TKey, TValor> ValoresExtraidos = new ArbolBPlusValores<TKey, TValor>();
                for (int i = 0; i < numeroValoresAExtraer; i++)
                {
                    ValoresExtraidos.Add(nuevaHoja.Valores.Keys[0], nuevaHoja.Valores.Values[0]);
                    nuevaHoja.Valores.RemoveAt(0);
                }

                TKey viejaKey = hojaAExtraer.Valores.Keys.Max();
                hojaAExtraer.Valores.AgregarRango(ValoresExtraidos);
                hojaAExtraer.NodoPadre.Enlaces.Remove(viejaKey);
                hojaAExtraer.NodoPadre.Enlaces.Add(hojaAExtraer.Valores.Keys.Max(), hojaAExtraer);
            }
            else
            {
                ArbolBPlusValores<TKey, TValor> ValoresExtraidos = new ArbolBPlusValores<TKey, TValor>();
                TKey viejaKey = nuevaHoja.Valores.Keys.Max();
                for (int i = 0; i < numeroValoresAExtraer; i++)
                {
                    ValoresExtraidos.Add(nuevaHoja.Valores.Keys[nuevaHoja.KeysCount - 1], nuevaHoja.Valores.Values[nuevaHoja.KeysCount - 1]);
                    nuevaHoja.Valores.RemoveAt(nuevaHoja.KeysCount - 1);
                }
                hojaAExtraer.Valores.AgregarRango(ValoresExtraidos);
                nuevaHoja.NodoPadre.Enlaces.Remove(viejaKey);
                nuevaHoja.NodoPadre.Enlaces.Add(nuevaHoja.Valores.Keys.Max(), nuevaHoja);
            }
        }

        //Unir-Extraer Nodo
        protected void UnirOExtraerNodo(Nodo<TKey, TValor> nodo)
        {
            if (nodo.NodoDerecho != null && nodo.NodoIzquierdo != null)
            {
                Nodo<TKey, TValor> NodoMinimo = (nodo.NodoIzquierdo.KeysCount < nodo.NodoDerecho.KeysCount) ? nodo.NodoIzquierdo : nodo.NodoDerecho;
                Nodo<TKey, TValor> NodoMaximo = (nodo.NodoIzquierdo.KeysCount >= nodo.NodoDerecho.KeysCount) ? nodo.NodoIzquierdo : nodo.NodoDerecho;

                if (nodo.KeysCount + NodoMinimo.KeysCount <= GradoMaximo)
                {
                    if (nodo.NodoDerecho == NodoMinimo)
                    {
                        UnirNodos(nodo, NodoMinimo);
                    }
                    else
                    {
                        UnirNodos(NodoMinimo, nodo);
                    }
                }
                else
                {
                    ExtraerNodos(nodo, NodoMaximo);
                }
            }
            else if (nodo.NodoIzquierdo != null)
            {
                if (nodo.KeysCount + nodo.NodoIzquierdo.KeysCount <= GradoMaximo)
                {
                    UnirNodos(nodo.NodoIzquierdo, nodo);
                }
                else
                {
                    ExtraerNodos(nodo, nodo.NodoIzquierdo);
                }
            }
            else if (nodo.NodoDerecho != null)
            {
                if (nodo.KeysCount + nodo.NodoDerecho.KeysCount <= GradoMaximo)
                {
                    UnirNodos(nodo, nodo.NodoDerecho);
                }
                else
                {
                    ExtraerNodos(nodo, nodo.NodoDerecho);
                }
            }
            else
            {
                if (!nodo.EsRaiz)
                {
                    nodo.EsRaiz = true;
                    nodo.NodoPadre = null;
                    Arbol = nodo;
                    ContadorNodos--;
                }
            }
        }


        //UnirNodos
        protected void UnirNodos(Nodo<TKey, TValor> NodoIzquierdo, Nodo<TKey, TValor> NodoDerecho)
        {
            ContadorNodos--;
            Nodo<TKey, TValor> nuevoNodo = new Nodo<TKey, TValor>(new ArbolBPlusEnlaces<TKey, TValor>(), null, null, null, NodoIzquierdo.ArbolPadre, false);
            ArbolBPlusEnlaces<TKey, TValor> unirEnlaces = new ArbolBPlusEnlaces<TKey, TValor>();

            unirEnlaces.AgregarRango(nuevoNodo, NodoIzquierdo.Enlaces);
            unirEnlaces.AgregarRango(nuevoNodo, NodoDerecho.Enlaces);
            nuevoNodo.Enlaces = unirEnlaces;
            nuevoNodo.NodoPadre = NodoDerecho.NodoPadre;
            nuevoNodo.NodoIzquierdo = NodoIzquierdo.NodoIzquierdo;
            nuevoNodo.NodoDerecho = NodoDerecho.NodoDerecho;

            if (nuevoNodo.NodoDerecho != null)
            {
                nuevoNodo.NodoDerecho.NodoIzquierdo = nuevoNodo;
            }
            if (nuevoNodo.NodoIzquierdo != null)
            {
                nuevoNodo.NodoIzquierdo.NodoDerecho = nuevoNodo;
            }
            nuevoNodo.NodoPadre.Enlaces.Remove(NodoDerecho.Enlaces.Keys.Max());
            nuevoNodo.NodoPadre.Enlaces.Add(nuevoNodo.Enlaces.Keys.Max(), nuevoNodo);
            NodoIzquierdo.NodoPadre.Enlaces.Remove(NodoIzquierdo.Enlaces.Keys.Max());

            if (NodoIzquierdo.NodoPadre != NodoDerecho.NodoPadre)
            {
                NodoIzquierdo.NodoPadre.NodoPadre.Enlaces.Remove(NodoIzquierdo.Enlaces.Keys.Max());
                NodoIzquierdo.NodoPadre.NodoPadre.Enlaces.Add(NodoIzquierdo.NodoPadre.Enlaces.Keys.Max(), NodoIzquierdo.NodoPadre);
                if (NodoIzquierdo.NodoPadre.KeysCount < GradoMinimo && !NodoIzquierdo.NodoPadre.EsRaiz)
                {
                    UnirOExtraerNodo(NodoIzquierdo.NodoPadre);
                }
            }
            if (nuevoNodo.NodoPadre.KeysCount < GradoMinimo && !nuevoNodo.NodoPadre.EsRaiz)
            {
                UnirOExtraerNodo(nuevoNodo.NodoPadre);
            }
            else if (nuevoNodo.NodoPadre.EsRaiz && nuevoNodo.NodoPadre.KeysCount == 1)
            {
                nuevoNodo.EsRaiz = true;
                nuevoNodo.NodoPadre = null;
                Arbol = nuevoNodo;
                ContadorNodos--;
            }
        }

        //ExtraerNodos
        protected void ExtraerNodos(Nodo<TKey, TValor> NodoAExtraer, Nodo<TKey, TValor> nuevoNodo)
        {
            int numeroEnlacesAExtraer = NodoAExtraer.KeysCount + (nuevoNodo.KeysCount - NodoAExtraer.KeysCount) / 2;
            if (NodoAExtraer.NodoDerecho == nuevoNodo)
            {
                ArbolBPlusEnlaces<TKey, TValor> EnlacesExtraidos = new ArbolBPlusEnlaces<TKey, TValor>();
                for (int i = 0; i < numeroEnlacesAExtraer; i++)
                {
                    EnlacesExtraidos.Add(nuevoNodo.Enlaces.Keys[0], nuevoNodo.Enlaces.Values[0]);
                    nuevoNodo.Enlaces.RemoveAt(0);
                }
                TKey viejaKey = NodoAExtraer.Enlaces.Keys.Max();
                NodoAExtraer.Enlaces.AgregarRango(NodoAExtraer, EnlacesExtraidos);
                NodoAExtraer.NodoPadre.Enlaces.Remove(viejaKey);
                NodoAExtraer.NodoPadre.Enlaces.Add(NodoAExtraer.Enlaces.Keys.Max(), NodoAExtraer);
            }
            else
            {
                ArbolBPlusEnlaces<TKey, TValor> EnlacesExtraidos = new ArbolBPlusEnlaces<TKey, TValor>();
                TKey viejaKey = nuevoNodo.Enlaces.Keys.Max();
                for (int i = 0; i < numeroEnlacesAExtraer; i++)
                {
                    EnlacesExtraidos.Add(nuevoNodo.Enlaces.Keys[nuevoNodo.KeysCount - 1], nuevoNodo.Enlaces.Values[nuevoNodo.KeysCount - 1]);
                    nuevoNodo.Enlaces.RemoveAt(nuevoNodo.KeysCount - 1);
                }
                NodoAExtraer.Enlaces.AgregarRango(NodoAExtraer, EnlacesExtraidos);
                nuevoNodo.NodoPadre.Enlaces.Remove(viejaKey);
                nuevoNodo.NodoPadre.Enlaces.Add(nuevoNodo.Enlaces.Keys.Max(), nuevoNodo);
            }
        }

        /// <summary>
        /// Metodo que ingresa al metodo Remove para eliminar un Nodo del arbol B+
        /// </summary>
        /// <param name="key">Key a ingresar para buscar la hoja a eliminar</param>
        /// <returns>retorna true si encontro la Key (hoja buscada) o false si no existe esa Key insertada</returns>
        public bool Eliminar(TKey key)
        {
            try
            {
                Nodo<TKey, TValor> pivote = EncontrarHojaPorKey(key);

                if (pivote != null)
                {
                    if (pivote.Valores.ContainsKey(key))
                    {
                        Count--;
                        if (pivote.Valores.Keys.Max().CompareTo(key) == 0 && pivote.NodoPadre != null)
                        {
                            pivote.Valores.Remove(key);
                            pivote.NodoPadre.Enlaces.Remove(key);
                            pivote.NodoPadre.Enlaces.Add(pivote.Valores.Keys.Max(), pivote);
                            Nodo<TKey, TValor> padre = pivote.NodoPadre;

                            while (padre.Enlaces.Keys.Max().CompareTo(pivote.Valores.Keys.Max()) == 0 && padre.NodoPadre != null)
                            {
                                padre.NodoPadre.Enlaces.Remove(key);
                                padre.NodoPadre.Enlaces.Add(pivote.Valores.Keys.Max(), padre);
                                padre = padre.NodoPadre;
                            }
                        }
                        else
                        {
                            pivote.Valores.Remove(key);
                        }

                        if (pivote.KeysCount < GradoMinimo && !pivote.EsRaiz)
                        {
                            if (pivote.EsHoja)
                            {
                                UnirOExtraerHoja(pivote);
                            }
                            else
                            {
                                UnirOExtraerNodo(pivote);
                            }
                        }
                        if (pivote.EsRaiz && pivote.KeysCount == 0)
                        {
                            ContadorNodos--;
                            Arbol = null;
                        }
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                ContadorNodos++;
                return true;
            }
        }


        /// <summary>
        /// El "KeyValuePair" es lo que contiene el Key con su Valor (Nodo) para eliminarlo del arbol B+
        /// </summary>
        /// <param name="KeyValuePair">Es lo que contiene el (key,valor) para eliminarlo del arbol B+</param>
        public bool Remove(KeyValuePair<TKey, TValor> KeyValuePair)
        {
            if (Contains(KeyValuePair))
            {
                Eliminar(KeyValuePair.Key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que elimina todo los contadores y el arbol lo vuelvo null
        /// </summary>
        public void Clear()
        {
            Count = 0;
            ContadorNodos = 0;
            Arbol = null;
        }


        /// <summary>
        /// Metodo que busca un Nodo dependiendo de su "Key" y "Valor"
        /// </summary>
        /// <param name="key">Key a ingresar</param>
        /// <param name="valor">Valor a ingresar</param>
        /// <returns>retorna true si encontro el valor, false si no lo encontro</returns>
        public bool Buscar(TKey key, out TValor valor)
        {
            Nodo<TKey, TValor> hoja = EncontrarHojaPorKey(key);
            if (hoja != null && EncontrarValorPorKey(hoja, key, out valor))
            {
                return true;
            }
            valor = default(TValor);
            return false;
        }

        /// <summary>
        /// Metodo booleano que nos ayuda a verificar si nuestro KeyValuePair(Key,Valor) contiene la Key a buscar del metodo "Buscar"
        /// </summary>
        /// <param name="KeyValuePair">nuestro pair que contiene el conjunto de Key y Value</param>
        /// <returns>true si contiene ese valor Pivote en el KeyValuePair, false si no</returns>
        public bool Contains(KeyValuePair<TKey, TValor> KeyValuePair)
        {
            TValor ValorPivote;

            if (Buscar(KeyValuePair.Key, out ValorPivote) && EqualityComparer<TValor>.Default.Equals(ValorPivote, KeyValuePair.Value))
            {
                return true;
            }
            return false;
        }


        //CopyTo
        public void CopyTo(KeyValuePair<TKey, TValor>[] arreglo, int IndexArreglo)
        {
            if (Arbol != null)
            {
                Nodo<TKey, TValor> pivote = Arbol;
                while (!pivote.EsHoja)
                {
                    pivote = pivote.Enlaces.Values[0];
                }
                do
                {
                    foreach (KeyValuePair<TKey, TValor> pair in pivote.Valores)
                    {
                        arreglo[IndexArreglo++] = pair;
                    }
                    pivote = pivote.NodoHojaDerecho;
                } while (pivote != null);
            }
        }

        //GetEnumerator
        public IEnumerator<KeyValuePair<TKey, TValor>> GetEnumerator()
        {
            if (Arbol != null)
            {
                Nodo<TKey, TValor> pivote = Arbol;
                while (!pivote.EsHoja)
                {
                    pivote = pivote.Enlaces.Values[0];
                }
                do
                {
                    foreach (KeyValuePair<TKey, TValor> KeyValuePair in pivote.Valores)
                    {
                        //usamos yield  para indicar al compilador que estamos dentro de un bloque de 
                        //iteración y nos permite acceder a los elementos de una lista (IEnumerable)
                        yield return KeyValuePair;
                    }
                    pivote = pivote.NodoHojaDerecho;
                }
                while (pivote != null);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}