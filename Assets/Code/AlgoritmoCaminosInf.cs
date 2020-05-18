
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class AlgoritmoCaminosInf : MonoBehaviour
{
    public class SaveAndLoad
    {
        string ruta = "C:/Users/sergio/Desktop/AI Projects/GUARDADO/CircuitosTotales/";
        private int ultimo;

        public SaveAndLoad()
        {
            //Aqui se busca cual es el último archivo de pila creado, para que se utilice ese
            ultimo = 0;
            if (File.Exists(@ruta + "pila" + 0 + ".txt"))
            {
                int auxiliar = 1;
                while (File.Exists(@ruta + "pila" + auxiliar + ".txt"))
                {
                    auxiliar++;

                }

                ultimo = auxiliar - 1;

            }



        }

        public bool existe()
        {
            return File.Exists(@ruta + "pila" + 0 + ".txt");
        }

        public string Leer(string arch)
        {
            long tamanio = new System.IO.FileInfo(@ruta + arch + ultimo + ".txt").Length;
            if (tamanio == 0) ultimo--;

            string res;
            using (StreamReader archivo = new StreamReader(@ruta + arch + ultimo + ".txt"))
            {
                res = archivo.ReadLine() ?? "";
            }
            // string resultado = File.ReadLine(@ruta+arch+".txt").First();

            var lines = File.ReadAllLines(@ruta + arch + ultimo + ".txt");
            File.WriteAllLines(@ruta + arch + ultimo + ".txt", lines.Skip(1).ToArray());




            return res;
        }

        public void Escribe(string testo, string arch)

        {
            long tamanio = 0;
            if (File.Exists(@ruta + "pila" + ultimo + ".txt"))
            {
                tamanio = new System.IO.FileInfo(@ruta + arch + ultimo + ".txt").Length;
            }


            if (tamanio > 5000000) ultimo++;

            using (System.IO.StreamWriter archivo =
                   new System.IO.StreamWriter(@ruta + arch + ultimo + ".txt", true))
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




        public class camino
        {


            /*
                   Inicial almacena la celda inicial del camino, ultimo el ultimo nodo añadido
                   y acumulado el resto de nodos del camino en orden.

           */
            public int inicial;
            public int ultimo;
            public List<int> acumulado;

            /*Constructor con una cadena de texto, no se realizan las comprobaciones necesarias, asumimos que se le
             pasa como parámetro una cadena de texto con los valores codificados conforme la función print. Dada una 
             cadena de este tipo la codifica separando los nodos y adecuandolos a la estructura de representación interna.
            */
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

            //Constructor de copia
            public camino(camino valor)
            {
                ultimo = valor.ultimo;
                inicial = valor.inicial;
                acumulado = new List<int>(valor.acumulado);
            }

            //Al añadir un nodo al camino este pasa a ser el ultimo añadido
            public void aniade(int nodo)
            {
                if (ultimo != inicial) { acumulado.Add(ultimo); }
                ultimo = nodo;

            }
            //Constructor de camino dada una terna salida-medio-entrada, salida corresponde al primer nodo que se expandira,
            //entrada al de llegada que finalizará el camino. Se asume que los nodos estan conectados no se comprueba.
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

            /*Construimos una cadena de texto representativa del camino para que sea sencillo del almacenar y leer posteriormente,
             se añade el identificador en orden de cada elemento del camino separados por el carácter ';', y los dos primeros nodos 
             representan el inicial y el último, lo hacemos así para que sea sencilo de leer para la representación de la clase camino.*/
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
            /*Al igual que la función anterior, esta construye una cadena de carácteres que representa el camino, difiere de la anterior
             en que en este caso el primer nodo que aparece es el primero de la lista acumulado y asi en orden hasta el último que será el
             nodo de entrada, esta representación es más sencilla para ser leída por una persona u otro programa que no conozca la representación
             interna de la clase.
             */
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

        //Crea una lista con los ids de las celdas vecinas a la pasada como parámetro acuerdo a las restricciones que se tienen
        //del grid
        public List<int> SacaVecinos(int celda)
        {
            List<int> vecinos = new List<int>();
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








        public void main()
        {
            //Este valor booleano controla que queden valores por leer, si no quedaran se establece a true.
            bool finito = false;
            //Clase que se encarga del guardado y carga de los caminos
            SaveAndLoad salvador = new SaveAndLoad();
            camino actual;
            //Si ya existe un archivo pila, carga los caminos desde él.
            if (salvador.existe())
            {

                actual = new camino(salvador.Leer("pila"));

            }
            //En caso de que no exista, añadimos las ternas válidas.
            else
            {
                actual = new camino(84, 63, 43);
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

                actual = new camino(64, 63, 62);
            }



            while (!finito)
            {

                //Obtenemos los vecinos del último nodo del camino.
                List<int> vecinos = SacaVecinos(actual.idUltima());

                //Para cada vecino, comprobamos si seria válido dentro del camino(que no este conectado con ningún nodo
                //ya incluido en el camino) y de ser asi, añadimos a la pila el camino anterior con este vecino válido.
                for (int i = 0; i < vecinos.Count; i++)
                {
                    int vecinaactual = vecinos[i];

                    List<int> vecinasvecina = SacaVecinos(vecinaactual);

                    bool valida = true;

                    for (int j = 0; j < vecinasvecina.Count && valida; j++)
                    {
                        valida = actual.Comprueba(vecinasvecina[j]);


                    }

                    if (valida)
                    {
                        bool auxiliar = (vecinasvecina.Contains(actual.GetInicial()) && actual.getTamanio() > 1);
                        if (auxiliar)
                        {

                            camino caminoauxiliar = new camino(actual);
                            caminoauxiliar.aniade(vecinaactual);
                            salvador.Escribe2(caminoauxiliar.toprint2(), "def");

                        }
                        else
                        {
                            camino caminoauxiliar = new camino(actual);
                            caminoauxiliar.aniade(vecinaactual);
                            salvador.Escribe(caminoauxiliar.toprint(), "pila");


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



        // Start is called before the first frame update
        void Start()
        {
            Prueba main = new Prueba();
            main.main();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
// Start is called before the first frame update
    

