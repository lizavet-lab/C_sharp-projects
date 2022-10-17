using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Fleury
{
 public class AlgDeykstry
 {
  static int[,] a;
  static int N;
  static int[,] Short;//Матрица кратчайших расстояний между вершинами
  const int INF = int.MaxValue;
  public static int[,] Deykstr(int[,] Matr)
  {
   N = Matr.GetLength(0);
   a = new int[N, N];
   Short = new int[N, N];
   for (int i = 0; i < N; i++)
      for (int j = 0; j < N; j++)
          a[i, j] = Matr[i, j];
   int[] d = new int[N]; // массив минимальных расстояний
   int[] v = new int[N]; //массив посещенных вершин
   int temp, currentindex, min;
   int begin_index;

   for (int ups = 0; ups < N; ups++)// для каждой вершины
    {
       begin_index = ups; //индекс в массиве минимальных расстояний делаем равным номеру вершины
       //Инициализация вершин и расстояний
       for (int i = 0; i < N; i++)
        {
         d[i] = INF;
         v[i] = 0; //непосещенным вершинам присваиваем в массиве значение 0
        }
       d[begin_index] = 0;
       // Шаг алгоритма
       do
       {
         currentindex = INF;
         min = INF;
         for (int i = 0; i < N; i++)
           // Если вершину ещё не обошли и вес меньше min
           if ((v[i] == 0) && (d[i] < min))
            { // Переприсваиваем значения
              min = d[i];
              currentindex = i;
            }
          // Добавляем найденный минимальный вес
          // к текущему весу вершины
          // и сравниваем с текущим минимальным весом вершины
          if (currentindex != INF)
          {
            for (int i = 0; i < N; i++)
             {
               if (Matr[currentindex, i] > 0)
               {
                 temp = min + Matr[currentindex, i];
                 if (temp < d[i])
                 {
                   d[i] = temp;
                 }
               }
             }
            v[currentindex] = 1;//отмечаем вершину посещенной
          }
       } while (currentindex < INF);

       // Запись кратчайших расстояний до вершин в матрицу Short
       for (int j = 0; j < N; j++)
          //if(odd.Contains(ups) && odd.Contains(j)) 
          Short[ups, j] = d[j];  
       d = new int[N]; // массив минимальных расстояний
       v = new int[N]; //массив посещенных вершин
       begin_index = 0;
    }
   return Short;
  }
 }
}