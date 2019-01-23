using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sudoku
{
    class Program
    {
        //Crea la estructura del tablero
        static void Tablero()
        {
            //Le asigno la dimension de la consola
            Console.SetWindowSize(80, 24);
            Console.SetBufferSize(80, 24);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            //Bucles para pintar la estructura del sudoku
            //Fila
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 37; j += 4)
                {
                    Console.SetCursorPosition(j, i);
                    if (j == 0 || j == 12 || j == 24 || j == 36)
                    {
                        Console.ForegroundColor = ConsoleColor.Black; Console.WriteLine("|");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine("|");
                    }
                }
            }
            //Columna
            for (int i = 0; i < 37; i++)
            {
                for (int j = 0; j < 19; j += 2)
                {
                    Console.SetCursorPosition(i, j);
                    if (j == 0 || j == 6 || j == 12 || j == 18)
                    {
                        Console.ForegroundColor = ConsoleColor.Black; Console.WriteLine("-");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine("-");
                    }
                }
            }
        }
        //Crea el struct Celda
        struct Celda
        {
            //Almacena el valor
            public int Valor;
            //Booleano que si esta en true es porque hay una repeticion y si esta en false que no esta repetido.(filas columnas y cuadrante)
            public bool HayRepeticiones;
        }
        //Crea la matriz inicial para un nuevo juego
        static Celda[,] CreaMatriz()
        {
            Celda[,] sudoku = new Celda[9, 9];
            sudoku[0, 2].Valor = 6;
            sudoku[0, 5].Valor = 2;
            sudoku[0, 6].Valor = 3;
            sudoku[0, 8].Valor = 4;
            sudoku[1, 0].Valor = 9;
            sudoku[1, 2].Valor = 4;
            sudoku[1, 3].Valor = 7;
            sudoku[1, 4].Valor = 5;
            sudoku[1, 7].Valor = 8;
            sudoku[1, 8].Valor = 2;
            sudoku[2, 2].Valor = 8;
            sudoku[2, 5].Valor = 6;
            sudoku[2, 8].Valor = 5;
            sudoku[3, 2].Valor = 3;
            sudoku[3, 7].Valor = 4;
            sudoku[4, 0].Valor = 2;
            sudoku[4, 3].Valor = 4;
            sudoku[4, 6].Valor = 8;
            sudoku[4, 7].Valor = 3;
            sudoku[5, 0].Valor = 4;
            sudoku[5, 2].Valor = 7;
            sudoku[5, 3].Valor = 5;
            sudoku[6, 3].Valor = 6;
            sudoku[6, 8].Valor = 8;
            sudoku[7, 0].Valor = 7;
            sudoku[7, 4].Valor = 2;
            sudoku[7, 6].Valor = 4;
            sudoku[7, 7].Valor = 5;
            sudoku[7, 8].Valor = 3;
            sudoku[8, 3].Valor = 3;
            sudoku[8, 4].Valor = 7;
            sudoku[8, 7].Valor = 6;
            sudoku[8, 8].Valor = 9;
            return sudoku;
        }
        //Dibuja los valores de la matriz sudoku
        static void DibujaNumeros(Celda[,] sudoku)
        {
            int fila = 1;
            int columna = 2;
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                columna = 2;
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    Console.SetCursorPosition(columna,fila);
                    if (sudoku[i, j].Valor == 0) Console.Write(" ");
                    else
                    {
                        if (sudoku[i, j].HayRepeticiones == false) Console.ForegroundColor=ConsoleColor.Green;
                        else Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(sudoku[i, j].Valor); 
                    }
                    columna += 4;
                }
                fila += 2;
                Console.WriteLine();
            }
        }
        //Carga un un fichero con la partida guardada anteriormente
        static Celda[,] CargaPartida()
        {
            Celda[,] sudoku=new Celda[9,9];
            Console.Write("¿Desea cargar partida? (S/N)");
            string opcion=Console.ReadLine().ToUpper(); //Almacena la opcion seleccionada
            //Opcion S carga la partida con el nombre que seleccionemos si existe 
            if (opcion == "S")
            {
                Console.Clear();
                Tablero();
                Console.Write("Introduce el nombre de usuario: ");
                string nombreUsuario = Console.ReadLine();
                //Ruta donde esta las partidas guardadas
                string ruta = "C:" + Path.DirectorySeparatorChar + "Sudoku" + Path.DirectorySeparatorChar + nombreUsuario + ".dat";
                //Si existe la ruta y el fichero indicado lo carga
                if (File.Exists(ruta))
                {
                    FileStream archivo = new FileStream(ruta, FileMode.Open);
                    for (int i = 0; i < sudoku.GetLength(0); i++)
                    {
                        for (int j = 0; j < sudoku.GetLength(1); j++)
                        {
                            sudoku[i, j].Valor = archivo.ReadByte();
                        }
                    }
                    archivo.Close();
                    Console.WriteLine("Partida cargada con exito.");
                    Console.ReadLine();
                }
                //Sino vuelve a pedir si quieres cargar una partida o no
                else
                {
                    Console.WriteLine("El usuario {0} no tiene una partida guardada.", nombreUsuario);
                    CargaPartida();
                }
            }
            //Opcion N, crea la partida predeterminada
            else if (opcion == "N")
            {
                sudoku = CreaMatriz();
                Console.WriteLine("Partida nueva creada.");
            }
            //Filtra el error de introducir una opcion invalida
            else
            {
                Console.WriteLine("Opcion no valida.");
                CargaPartida();
            }
            return sudoku;
        }
        //Asigna el valor a la matriz sudoku con la posicion indicada
        static Celda[,] AsignaValor(Celda[,] partidaActual,ConsoleKeyInfo tecla,int columna,int fila)
        {
            Celda[,] sudoku = partidaActual;
            if (tecla.KeyChar >= '1' && tecla.KeyChar <= '9')
            {
                int num = int.Parse(tecla.KeyChar.ToString());
                if (fila == 1) fila = 0;
                else if (fila>1) fila = fila / 2;
                if (columna == 2) columna = 0;
                else if (columna > 2) columna = columna / 4;
                sudoku[fila, columna].Valor = num;
                ActualizarFlag(sudoku,columna,fila);
                return sudoku;
            }
            else
            {
                if (fila == 1) fila = 0;
                else if (fila > 1) fila =fila/ 2;
                if (columna == 2) columna = 0;
                else if (columna > 2) columna =columna/ 4;
                sudoku[fila, columna].Valor =0;
                ActualizarFlag(sudoku, columna, fila);
                return sudoku;
            }
            
        }
        //Asigna true o false si hay repeticiones en fila, columna y cuadrante
        static Celda[,] ActualizarFlag(Celda[,] partidaActual, int columna, int fila)
        {
            Celda[,] sudoku = new Celda[9, 9];
            sudoku = CompruebaRepeticionesColumna(partidaActual, columna, fila);
            bool columnaRepeticiones = sudoku[fila, columna].HayRepeticiones;
            sudoku = CompruebaRepeticionesFila(partidaActual, columna, fila);
            bool filaRepeticiones = sudoku[fila, columna].HayRepeticiones;
            sudoku = CompruebaRepeticionesCuadrante(partidaActual, columna, fila);
            bool cuadranteRepeticiones = sudoku[fila, columna].HayRepeticiones;
            if (columnaRepeticiones == true || filaRepeticiones == true || cuadranteRepeticiones == true) sudoku[fila, columna].HayRepeticiones = true;
            else if (columnaRepeticiones == false && filaRepeticiones == false && cuadranteRepeticiones == false) sudoku[fila, columna].HayRepeticiones = false;
            return sudoku;
        }
        //Comprueba si hay repeticiones en la columna donde se asigna el valor
        static Celda[,] CompruebaRepeticionesColumna(Celda[,] partidaActual,int columna,int fila)
        {
            bool repetido=false;
            for (int i = 0; i < partidaActual.GetLength(1); i++)
            {
                if (partidaActual[i, columna].Valor == partidaActual[fila, columna].Valor)
                {
                    if (i != fila)
                    {
                        partidaActual[i, columna].HayRepeticiones = true;
                        repetido = true;
                    }
                }
                else
                {
                    partidaActual[i, columna].HayRepeticiones = false;
                }
            }
            partidaActual[fila, columna].HayRepeticiones = repetido;
            return partidaActual;
        }
        //Comprueba si hay repeticiones en la fila donde se asigna el valor
        static Celda[,] CompruebaRepeticionesFila(Celda[,] partidaActual, int columna, int fila)
        {
            bool repetido = false;
            for (int i = 0; i < partidaActual.GetLength(0); i++)
            {
                if (partidaActual[fila, i].Valor == partidaActual[fila, columna].Valor)
                {
                    if(i!=columna)
                    {
                        partidaActual[fila, i].HayRepeticiones = true;
                        repetido = true;
                    }
                }
                else
                {
                    partidaActual[fila, i].HayRepeticiones = false;
                }
            }
            partidaActual[fila, columna].HayRepeticiones = repetido;
            return partidaActual;
        }
        //Comprueba si hay repeticiones en el cuadrante donde se asigna el valor
        static Celda[,] CompruebaRepeticionesCuadrante(Celda[,] partidaActual, int columna, int fila)
        {
            int[,] cuadrante = new int[1, 2];
            cuadrante=CalcularCuadrante(columna,fila);
            int x = cuadrante[0, 0];
            int y;
            for (int i = 0; i < 3; i++)
            {
                y = cuadrante[0, 1];
                for (int j = 0; j < 3; j++)
                {
                    if (partidaActual[x, y].Valor == partidaActual[fila, columna].Valor)
                    {
                            
                        if(x==fila && y==columna) partidaActual[fila, columna].HayRepeticiones = false;
                        else
                        {
                            partidaActual[x, y].HayRepeticiones = true;
                            partidaActual[fila, columna].HayRepeticiones = true;
                        }
                    }
                    else
                    {
                        partidaActual[x, y].HayRepeticiones = false;
                    }
                    y++;
                }
                x++;
            }
            return partidaActual;
        }
        //Calcula en que cuadrante se encuentra el valor introducido y devuelve la posicion del cuadrante
        static int[,] CalcularCuadrante(int columna,int fila)
        {
            int[,] cuadrante=new int[1,2];
            if (fila >= 0 && fila < 3)
            {
                cuadrante[0, 0] = 0;
                if(columna >= 0 && columna < 3)
                {
                    cuadrante[0, 1] = 0;
                }
                if (columna >= 3 && columna < 6)
                {
                    cuadrante[0, 1] = 3;
                }
                if (columna >= 6 && columna < 9)
                {
                    cuadrante[0, 1] = 6;
                }
            }
            if (fila >= 3 && fila < 6)
            {
                cuadrante[0, 0] = 3;
                if (columna >= 0 && columna < 3)
                {
                    cuadrante[0, 1] = 0;
                }
                if (columna >= 3 && columna < 6)
                {
                    cuadrante[0, 1] = 3;
                }
                if (columna >= 6 && columna < 9)
                {
                    cuadrante[0, 1] = 6;
                }
            }
            if (fila >= 6 && fila < 9)
            {
                cuadrante[0, 0] = 6;
                if (columna >= 0 && columna < 3)
                {
                    cuadrante[0, 1] = 0;
                }
                if (columna >= 3 && columna < 6)
                {
                    cuadrante[0, 1] = 3;
                }
                if (columna >= 6 && columna < 9)
                {
                    cuadrante[0, 1] = 6;
                }
            }
            return cuadrante;
        }
        //Guarda la partida en un fichero con el nombre indicado
        static void GuardaPartida(Celda[,] sudoku)
        {
            Console.Write("¿Desea salvar partida? (S/N)");
            string opcion = Console.ReadLine().ToUpper();
            if (opcion == "S")
            {
                Console.Write("Introduce el nombre de usuario: ");
                string nombreUsuario = Console.ReadLine();
                string ruta = "C:" + Path.DirectorySeparatorChar + "Sudoku" + Path.DirectorySeparatorChar + nombreUsuario + ".dat";
                if (File.Exists(ruta))
                {
                    Console.WriteLine("Ya existe una partida con el nombre {0}, quiere sobreescribir?(Escribe S o N)", nombreUsuario);
                    string opcion1 = Console.ReadLine().ToUpper();
                    if (opcion1 == "S")
                    {
                        FileStream archivo = new FileStream(ruta, FileMode.Create);
                        for (int i = 0; i < sudoku.GetLength(0); i++)
                        {
                            for (int j = 0; j < sudoku.GetLength(1); j++)
                            {
                                archivo.WriteByte((byte)sudoku[i, j].Valor);
                            }
                        }
                        archivo.Close();
                        Console.WriteLine("Partida guardada con exito.");
                        Console.ReadLine();
                    }
                    else if (opcion == "N")
                    {
                        GuardaPartida(sudoku);
                    }
                }
                else
                {
                    if (!Directory.Exists("C:" + Path.DirectorySeparatorChar + "Sudoku"))
                    {
                        Directory.CreateDirectory("C:" + Path.DirectorySeparatorChar + "Sudoku");
                    }
                    FileStream archivo = new FileStream(ruta, FileMode.Create);
                    for (int i = 0; i < sudoku.GetLength(0); i++)
                    {
                        for (int j = 0; j < sudoku.GetLength(1); j++)
                        {
                            archivo.WriteByte((byte)sudoku[i, j].Valor);
                        }
                    }
                    archivo.Close();
                    Console.WriteLine("Partida guardada con exito.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Hasta la proxima.");
                Console.ReadLine();
            }
        }
        //Menu del sudoku, llama a los demas modulos para el correcto funcionamiento del sudoku ademas de implementar el movimiento del cursor.
        static void Menu()
        {
            Tablero();
            Celda[,] sudoku = CargaPartida();
            DibujaNumeros(sudoku);
            int columna = 2;
            int fila = 1;

            ConsoleKeyInfo tecla;
            do
            {
                Console.SetCursorPosition(columna, fila);
                tecla = Console.ReadKey(true);
                switch (tecla.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (Console.CursorLeft - 4 < 2)
                        {
                            columna = Console.CursorLeft;
                            fila = Console.CursorTop; break;
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);
                            columna = Console.CursorLeft;
                            fila = Console.CursorTop; break;
                        }
                    case ConsoleKey.RightArrow:
                        if (Console.CursorLeft + 4 > 36)
                        {
                            columna = Console.CursorLeft;
                            fila = Console.CursorTop; break;
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                            columna = Console.CursorLeft;
                            fila = Console.CursorTop; break;
                        }
                    case ConsoleKey.UpArrow:
                        if (Console.CursorTop - 2 < 1)
                        {
                            columna = Console.CursorLeft;
                            fila = Console.CursorTop; break;
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 2);
                            columna = Console.CursorLeft;
                            fila = Console.CursorTop; break;
                        }
                    case ConsoleKey.DownArrow:
                        if (Console.CursorTop + 2 > 18)
                        {
                            columna = Console.CursorLeft;
                            fila = Console.CursorTop; break;
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 2);
                            columna = Console.CursorLeft;
                            fila = Console.CursorTop; break;
                        }
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Tablero();
                        DibujaNumeros(sudoku);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(0,19);
                        GuardaPartida(sudoku);
                        break;
                    case ConsoleKey.Backspace:
                    case ConsoleKey.Delete:
                        sudoku = AsignaValor(sudoku, tecla, columna, fila);
                        DibujaNumeros(sudoku);
                        break;
                    default:
                        if (tecla.KeyChar >= '1' && tecla.KeyChar <= '9')
                        {
                            sudoku = AsignaValor(sudoku, tecla, columna, fila);
                            DibujaNumeros(sudoku);
                            Console.SetCursorPosition(columna, fila);
                        }
                        break;
                }

            }
            while (tecla.Key != ConsoleKey.Escape);
        }
        static void Main(string[] args)
        {
            //Inicializa el sudoku
            Menu(); 
        }
    }
}
