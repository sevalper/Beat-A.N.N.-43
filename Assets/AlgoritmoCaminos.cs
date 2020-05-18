using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class SaveAndLoadNew : MonoBehaviour
{
    public class SaveAndLoad
    {
        string ruta = "C:/Users/sergio/Desktop/AI Projects/GUARDADO/CircuitosTotales/";
        private int numerico;

        public SaveAndLoad()
        {
            numerico = 2;
        }

        public string Leer(string arch)
        {
            long tamanio = new System.IO.FileInfo(@ruta + arch + numerico + ".txt").Length;
            if (tamanio == 0) numerico--;

            string res;
            using (StreamReader archivo = new StreamReader(@ruta + arch + numerico + ".txt"))
            {
                res = archivo.ReadLine() ?? "";
            }
            // string resultado = File.ReadLine(@ruta+arch+".txt").First();

            var lines = File.ReadAllLines(@ruta + arch + numerico + ".txt");
            File.WriteAllLines(@ruta + arch + numerico + ".txt", lines.Skip(1).ToArray());




            return res;
        }

        public void Escribe(string testo, string arch)

        {
            long tamanio = new System.IO.FileInfo(@ruta + arch + numerico + ".txt").Length;
            if (tamanio > 5000000) numerico++;

            using (System.IO.StreamWriter archivo =
                   new System.IO.StreamWriter(@ruta + arch + numerico + ".txt", true))
                archivo.WriteLine(testo);



        }

        public void Escribe2(string testo, string arch)

        {




            using (System.IO.StreamWriter archivo =
                new System.IO.StreamWriter(@ruta + arch + ".txt", true))
                archivo.WriteLine(testo);
        }





    }




    public class Prueba
    {
        private int num = 20;


        public class Carga
        {


        }


        public class camino
        {


            /*
                    Inicial almacena la celda inicial del camino, ultimo el ultimo nodo añadido
                    y acumulado el camino como tal.

            */
            public int inicial;
            public int ultimo;
            public List<int> acumulado;
            //Constructor básico

            public camino(string val)
            {
                string[] vect = val.Split(';');
                inicial = int.Parse(vect[0]);
                ultimo = int.Parse(vect[1]);
                acumulado = new List<int>();

                for (int i = 2; i < vect.Length; i++)
                {
                    acumulado.Add(int.Parse(vect[i]));
                }
            }

            public camino(int valor)
            {
                ultimo = valor;
                inicial = valor;
                acumulado = new List<int>();
            }

            public camino(camino valor)
            {
                ultimo = valor.ultimo;
                inicial = valor.inicial;
                acumulado = new List<int>(valor.acumulado);
            }

            //Al añadir un nodo al camino este pasa a ser el ultimo añadido y lo metemos en el camino acumulado
            public void aniade(int nodo)
            {
                if (ultimo != inicial) { acumulado.Add(ultimo); }
                ultimo = nodo;

            }

            public camino(int salida, int medio, int entrada)
            {
                inicial = entrada;
                ultimo = salida;
                acumulado = new List<int>();
                acumulado.Add(medio);
            }

            public int GetInicial()
            {
                return inicial;
            }

            public List<int> getAcumulado()
            {
                return acumulado;

            }

            //Tamaño del camino
            public int getTamanio()
            {
                return acumulado.Count;

            }

            //Que esto devuelva el id de la ultima celda añadida al camino,
            public int idUltima()
            {
                return ultimo;

            }

            public string toprint()
            {
                string resultado = "";
                resultado = resultado + inicial + ";";
                resultado = resultado + ultimo + "";
                for (int i = 0; i < acumulado.Count; i++)
                {
                    resultado = resultado + ";" + acumulado[i];
                }
                return resultado;
            }

            public string toprint2()
            {
                string resultado = "";
                resultado = resultado + acumulado[0];


                for (int i = 1; i < acumulado.Count; i++)
                {
                    resultado = resultado + ";" + acumulado[i];
                }
                resultado = resultado + ";" + ultimo;
                resultado = resultado + ";" + inicial;
                return resultado;
            }

            public bool Comprueba(int candidata)
            {
                //Comprobamos si la candidata esta ya en los vecinos
                //Opción eficiente guardar copia del camino como vector ordenado
                bool resultado = acumulado.Contains(candidata);
                return !resultado;

            }
        }

        private bool InRange2(int number) => 0 < number && number < num;

        public List<int> SacaVecinos(int celda)
        {
            List<int> vecinos = new List<int>();
            //grid[x, y].SetPosition(new Vector2(x,y), y + (x * gridSizeX));
            int x = celda / num;
            int y = celda - num * x;
            int aux;

            if (y % 2 == 0)
            {
                if (InRange2(x) && InRange2(y - 1))
                {
                    aux = y - 1 + num * (x);
                    vecinos.Add(aux);
                }
                if (InRange2(x - 1) && InRange2(y - 1))
                {
                    aux = y - 1 + num * (x - 1);
                    vecinos.Add(aux);
                }
                if (InRange2(x - 1) && InRange2(y))
                {
                    aux = y + num * (x - 1);
                    vecinos.Add(aux);
                }
                if (InRange2(x + 1) && InRange2(y))
                {
                    aux = y + num * (x + 1);
                    vecinos.Add(aux);
                }
                if (InRange2(x - 1) && InRange2(y + 1))
                {
                    aux = y + 1 + num * (x - 1);
                    vecinos.Add(aux);
                }
                if (InRange2(x) && InRange2(y + 1))
                {
                    aux = y + 1 + num * (x);
                    vecinos.Add(aux);
                }
            }
            else
            {
                if (InRange2(x + 1) && InRange2(y - 1))
                {
                    aux = y - 1 + num * (x + 1);
                    vecinos.Add(aux);
                }
                if (InRange2(x) && InRange2(y - 1))
                {
                    aux = y - 1 + num * (x);
                    vecinos.Add(aux);
                }
                if (InRange2(x - 1) && InRange2(y))
                {
                    aux = y + num * (x - 1);
                    vecinos.Add(aux);
                }
                if (InRange2(x + 1) && InRange2(y))
                {
                    aux = y + num * (x + 1);
                    vecinos.Add(aux);
                }
                if (InRange2(x) && InRange2(y + 1))
                {
                    aux = y + 1 + num * (x);
                    vecinos.Add(aux);
                }
                if (InRange2(x + 1) && InRange2(y + 1))
                {
                    aux = y + 1 + num * (x + 1);
                    vecinos.Add(aux);
                }
            }


            return vecinos;
        }















        public void main(int inicial)
        {
            bool finito = false;
            SaveAndLoad salvador = new SaveAndLoad();



            /*
            camino actual = new camino(84, 63, 43);
            salvador.Escribe(actual.toprint(), "pila");

            actual = new camino(84, 63, 62);
            salvador.Escribe(actual.toprint(), "pila");

            actual = new camino(84, 63, 82);
            salvador.Escribe(actual.toprint(), "pila");

            actual = new camino(83, 63, 64);
            salvador.Escribe(actual.toprint(), "pila");

            actual = new camino(83, 63, 43);
            salvador.Escribe(actual.toprint(), "pila");

            actual = new camino(83, 63, 62);
            salvador.Escribe(actual.toprint(), "pila");

            actual = new camino(82, 63, 64);
            salvador.Escribe(actual.toprint(), "pila");

            actual = new camino(82, 63, 43);
            salvador.Escribe(actual.toprint(), "pila");
            
            actual = new camino(64, 63, 62);*/

            camino actual = new camino(salvador.Leer("pila"));

            //while (caminospendientes.Count != 0)
            while (!finito)
            {

                //Console.WriteLine("adios");
                //Debug.Log("adios");


                // List<int> vecinos = actual.GetVecinos();
                List<int> vecinos = SacaVecinos(actual.idUltima());


                for (int i = 0; i < vecinos.Count; i++)
                {
                    int vecinaactual = vecinos[i];

                    List<int> vecinitas = SacaVecinos(vecinaactual);

                    bool valida = true;

                    for (int j = 0; j < vecinitas.Count && valida; j++)
                    {
                        valida = actual.Comprueba(vecinitas[j]);


                    }

                    if (valida)
                    {
                        bool auxiliar = (vecinitas.Contains(actual.GetInicial()) && actual.getTamanio() > 1);
                        if (auxiliar)
                        {
                            /*****************************************/
                            /******************Imprimir camino***********/
                            //  Debug.Log("asd");

                            camino caminoauxiliar = new camino(actual);
                            caminoauxiliar.aniade(vecinaactual);
                            salvador.Escribe2(caminoauxiliar.toprint2(), "def");

                        }
                        else
                        {
                            camino caminoauxiliar = new camino(actual);
                            caminoauxiliar.aniade(vecinaactual);
                            salvador.Escribe(caminoauxiliar.toprint(), "pila");
                            // caminospendientes.Push(caminoauxiliar);

                        }







                    }

                }

                string aux = salvador.Leer("pila");
                finito = string.IsNullOrEmpty(aux);
                if (!finito)
                {
                    actual = new camino(aux);
                }


            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Prueba prueba1 = new Prueba();
        prueba1.main(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

