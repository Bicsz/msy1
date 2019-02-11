using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace всякое
{
    class Program
    {
        class Dot
        {
            internal short myCost;
            internal short myNumber;
            internal List<Move> moves = new List<Move>();
            internal void AddMove(Move move)
            {
                moves.Add(move);
            }
        }
        class Move
        {
            internal short cost;
            internal short FromOwner;
            internal Move( short cost1, short FromOwner1)
            { 
                cost = cost1;
                FromOwner = FromOwner1;
            }
        }

        static void Main(string[] args)
        {
            List<Dot> dots = new List<Dot>();
            List<short> dots_inner = new List<short>();
            
            Console.WriteLine("Matrix is");

            List<string> matrix_rows = new List<string>();
            string[] matrix = File.ReadAllLines(Environment.CurrentDirectory+@"/lab1/matrix.txt");
            string[] matrix_costs = File.ReadAllLines(Environment.CurrentDirectory + @"/lab1/matrix_costs.txt");
            foreach (string str1 in matrix)
            {
                Console.WriteLine(str1);
                matrix_rows.Add(str1);
            }

            List<short> dots_ext = new List<short>();

            for (short i = 1; i <= matrix_rows[0].Length; i++)//перечисляем все точки, чтобы затем убрать те, которые точно не выход
                dots_ext.Add(i);
            Console.WriteLine("");
            short cost = -1;
            short row = -1;

            bool CanItBeInner = true;

            string str;
            foreach (string str1 in matrix_costs)
            {
                str = str1;
                row++;
                //Console.WriteLine(str);
                CanItBeInner = true;
                dots.Add(new Dot());
                for (short i = 0; i < matrix_rows[row].Length; i++)
                {
                    cost = Int16.Parse(str.Substring(0,str.IndexOf('.')));//принимает значение из матрицы цен
                    str = str.Substring(str.IndexOf('.')+1);
                    
                    if (i != dots.Count - 1)
                    {
                        if (matrix_rows[row][i] == '1')//если есть влияние на нее
                        {
                            dots[dots.Count - 1].AddMove(new Move(cost, (short)(i + 1)));
                            CanItBeInner = false;//точно не вход т.к на него оказывается воздействет
                            //если это влияние идет из точки, которая состоит в группе предположительных выходов, то удалить ее от тудова
                            for (short a = 0; a < dots_ext.Count; a++)//перечисляем все точки, чтобы затем убрать те, которые точно не выход
                                if(dots_ext[a]== i + 1)
                                {
                                    dots_ext.RemoveAt(a);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        //если главная диагональ
                        dots[dots.Count - 1].myCost = cost;
                    }
                }
                if(CanItBeInner)
                {
                    //является входом

                    //для сохранения номера точки
                    dots_inner.Add((short)dots.Count);
                }                         
            }

            Console.WriteLine("");
            Console.WriteLine("");

            for(short i=0;i< dots_inner.Count;i++)
                Console.WriteLine("inner: "+ dots_inner[i]);
            for (short i = 0; i < dots_ext.Count; i++)
                Console.WriteLine("exit: " + dots_ext[i]);

            Console.ReadKey(true);
            Console.WriteLine("");
            Console.WriteLine("now 2.3: rearrangement(inputs first, outputs at the end)");


           
            Console.WriteLine("" );
            //формирование новой матрицы
            for (short i=0;i<dots_inner.Count;i++)//вывод inputs
            {
                Console.WriteLine(matrix_rows[dots_inner[i]-1]);
            }

            bool canGo;
            for (short i = 0; i < dots.Count; i++)//вывод
            {
                canGo = true;
                for (short q=0;q< dots_inner.Count;q++)
                    if(dots_inner[q]==i+1)
                    {
                        canGo = false;
                        dots_inner.RemoveAt(q);
                        break;
                    }
                for (short q = 0; q < dots_ext.Count; q++)
                    if (dots_ext[q] == i+1)
                    {
                        canGo = false;
                        break;
                    }
                if(canGo)
                    Console.WriteLine(matrix_rows[i + 1-1]);
            }

            for (short i = 0; i < dots_ext.Count; i++)//вывод ends
            {
                Console.WriteLine(matrix_rows[dots_ext[i]-1]);
            }
            Console.ReadKey(true);
        }
    }
}
