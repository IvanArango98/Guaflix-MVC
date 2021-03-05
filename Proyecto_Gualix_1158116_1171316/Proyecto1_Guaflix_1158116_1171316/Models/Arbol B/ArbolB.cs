using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1_Guaflix_1158116_1171316.Models.Arbol_B
{
   public class ArbolB
    {
        //numero de Nodos
        private int count { get; set; }
        //numero de Nodos indices
        private int Indexcount { get; set; }
        //Numero de Nodos hojas
        private int Hojascount { get; set; }
        //profundidad del arbol
        private int profundidad { get; set; }
        //tamaño de los Nodos
        private int NodoSize { get; set; }

        private Nodo raiz { get; set; }
        private Stack<Nodo> hojas { get; set; }

        //constructor
        public ArbolB(int GradoArbol)
        {
            NodoSize = GradoArbol;
            raiz = new ArbolBHoja(NodoSize);
            Indexcount = 0;
            Hojascount = 1;
            count = 1;
            profundidad = 0;
            hojas = new Stack<Nodo>();
        }

        /// <summary>
        /// Agrega valores al arbol B
        /// Si el valor agregado causa un overflow hace la division
        /// </summary>
        /// <param name="valor">El valor que se agregara</param>
        /// <returns>Verdadero si es ingresado exitosamente, Falso si es un valor duplicado</returns>
        public bool Insertar(string valor)
        {
            //bandera que indica si el ingreso del valor es un ingreso exitoso o un fracaso
            bool flag = false;

            // verificar si el valor no esta duplicado
            if (!Buscar(valor))
            {
                Agregar EstatusDeInsercion = ((ArbolBHoja)hojas.Peek()).Insertar(valor);
                if (EstatusDeInsercion == Agregar.HacerDivision)
                {
                    //la hoja necesita hacer division
                    DividirHoja(valor);
                }
                else if (EstatusDeInsercion == Agregar.Duplicar)
                {
                    //valor ingresado ya existe en el arbol B   
                }
                //valor agregado exitosamente
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// Busca un valor en el arbol B
        /// Empieza en la raiz y busca hasta encontrar el valor o hasta encontrar una hoja
        /// Si lo encontro retorna verdadero sino retorna falso
        /// </summary>
        /// <param name="valor">el valor a encontrar</param>
        /// <returns>Verdadero si es encontrado, Falso si no</returns>
        public bool Buscar(string valor)
        {
            bool flag = false;
            Nodo pivote = raiz;
            hojas.Clear();

            //ciclo hasta encontrar la hoja
            while (pivote is ArbolBIndex)
            {
                hojas.Push(pivote);

                for (int i = 0; i < pivote.Valores.Count; i++)
                {
                    //if (pivote.Valores[i] > valor)
                    if (pivote.Valores[i].CompareTo(valor) > 0)
                    {
                        pivote = ((ArbolBIndex)pivote).IndexNodos[i];
                        break;
                    }
                    if (i == pivote.Valores.Count - 1)
                    {
                        pivote = ((ArbolBIndex)pivote).IndexNodos[i + 1];
                        break;
                    }
                }
            }
            hojas.Push(pivote);
            if (pivote.Valores.Contains(valor))
                flag = true;

            return flag;
        }

        /// <summary>
        /// divide un nodo hoja
        /// Este metodo creara una nueva raiz cuando el nodo este en underflow, subiendo el valor de la mitad
        /// </summary>
        /// <param name="valor">el valor que causa el nodo underflow</param>
        private void DividirHoja(string valor)
        {
            //la nueva hoja
            ArbolBHoja nuevaHoja = new ArbolBHoja(NodoSize);
            //la hoja que necesita la division
            ArbolBHoja viejaHoja = (ArbolBHoja)hojas.Pop();

            //copiar los valores de la vieja hoja
            for (int i = viejaHoja.Valores.Count / 2; i < viejaHoja.Valores.Count; i++)
                nuevaHoja.Valores.Add(viejaHoja.Valores[i]);

            foreach (string v in nuevaHoja.Valores)
                viejaHoja.Valores.Remove(v);

            if (hojas.Count == 0)
            {
                //creamos la raiz y le insertamos los valores
                raiz = new ArbolBIndex(NodoSize);
                ((ArbolBIndex)raiz).IndexNodos.Add(viejaHoja);
                ((ArbolBIndex)raiz).Insertar(nuevaHoja.Valores[0], nuevaHoja);
                hojas.Push(raiz);
                profundidad++;
                Indexcount++;
                count++;
            }
            else
            {
                Agregar statusCode = ((ArbolBIndex)hojas.Peek()).Insertar(nuevaHoja.Valores[0], nuevaHoja);
                if (statusCode == Agregar.HacerDivision)
                    DividirIndex((ArbolBIndex)hojas.Pop(), valor);
                else if (statusCode == Agregar.Duplicar)
                {
                }
            }

            //poner la hoja correcta a la pila
            if (viejaHoja.Valores.Contains(valor))
                hojas.Push(viejaHoja);
            else
                hojas.Push(nuevaHoja);

            Hojascount++;
            count++;
        }

        /// <summary>
        /// dividi un nodo Index
        /// </summary>
        /// <param name="viejoIndex">el nodo que hace overflowing</param>
        /// <param name="nuevoValor">el valor el cual en la adicion se creo una division de nodos</param>
        private void DividirIndex(ArbolBIndex viejoIndex, string nuevoValor)
        {
            ArbolBIndex nuevaIndex = new ArbolBIndex(NodoSize);

            //copiar la mitad de los indices viejos al nuevo
            for (int i = viejoIndex.IndexNodos.Count / 2; i < viejoIndex.IndexNodos.Count; i++)
                nuevaIndex.IndexNodos.Add(viejoIndex.IndexNodos[i]);

            foreach (Nodo n in nuevaIndex.IndexNodos)
                viejoIndex.IndexNodos.Remove(n);

            viejoIndex.Valores.Clear();
            nuevaIndex.Valores.Clear();
            for (int i = 1; i < viejoIndex.IndexNodos.Count; i++)
                viejoIndex.Valores.Add(MinValor(viejoIndex.IndexNodos[i]));
            for (int i = 1; i < nuevaIndex.IndexNodos.Count; i++)
                nuevaIndex.Valores.Add(MinValor(nuevaIndex.IndexNodos[i]));

            if (hojas.Count == 0)
            {
                //se crea una nueva raiz
                raiz = new ArbolBIndex(NodoSize);
                ((ArbolBIndex)raiz).IndexNodos.Add(viejoIndex);
                ((ArbolBIndex)raiz).Insertar(MinValor(nuevaIndex), nuevaIndex);
                hojas.Push(raiz);
                profundidad++;
                Indexcount++;
                count++;
            }
            else
            {
                Agregar statusCode = ((ArbolBIndex)hojas.Peek()).Insertar(MinValor(nuevaIndex), nuevaIndex);
                if (statusCode == Agregar.HacerDivision)
                    //nodo padre necesita hacer una division
                    DividirIndex((ArbolBIndex)hojas.Pop(), nuevoValor);
                else if (statusCode == Agregar.Duplicar)
                {
                }
            }
            // if (nuevoValor < MinValor(nuevaIndex))
            if (MinValor(nuevaIndex).CompareTo(nuevoValor) < 0)
                hojas.Push(viejoIndex);
            else
                hojas.Push(nuevaIndex);

            Indexcount++;
            count++;
        }

        /// <summary>
        /// Encuentra el valor minimo contenido del subarbol, utilizado para hacer la division correctamente          
        /// </summary>
        /// <param name="nodo">la raiz del subarbol a buscar</param>
        /// <returns>el valor minimo</returns>
        private string MinValor(Nodo nodo)
        {
            //hasta encontrar una hoja
            while (nodo is ArbolBIndex)
                nodo = ((ArbolBIndex)nodo).IndexNodos[0];

            //returnar el valor minimo de la hoja
            return nodo.Valores[0];
        }


    }
}