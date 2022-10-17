using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg_Fleury
{
public class WidthRound
{
 static int N;
 static Random rand = new Random();
 static Queue<int> q;// очередь, хранящая номера вершин
 
  public static bool Round(int[,] Matr, int v1)
  {
   bool T = true;//Посещены ли все вершины
   N = Matr.GetLength(0);
   q = new Queue<int>();
   int u = v1;//начальная вершина обхода
   bool[] visit = new bool[N];  //массив посещения вершин (изнчально из нулей)

   visit[u] = true;//отмечаем начальную вершину как посещенную    
   q.Enqueue(u);//Добавляем начальную вершину в конец очереди
   bool[] obhod = new bool[N];
   while (q.Count != 0)//Пока очередь не пустая
    {
      u = q.Peek();//читаем элемент из головы очереди
      q.Dequeue();//удаляем его из головы очереди
      do
      {
        int j = rand.Next(0, N);//случайным образом выбираем вершину для скрещивания
        obhod[j] = true;
        if (!visit[j])//если мы в ней еще не были
        {
          if (Matr[u, j] >= 1)// и если вершина смежна с вершиной, которую взяли
          {
             visit[j] = true;//то отмечаем ее как посещенную в массиве посещения вершин
             q.Enqueue(j);//и добавляем ее в конец очереди               
          }
        }
      } while (obhod.Contains(false));
      obhod = new bool[N];
    }
   for (int i = 0; i < N; i++)
    if (visit[i] == false)
      {
        T = false;
        break;
      }
    return T;
  }
  public static bool Round(int[,] Matr, int v1, List<int> zero)
  {
    bool T = true;//Посещены ли все вершины
    N = Matr.GetLength(0);
    q = new Queue<int>();
    int u = v1;//начальная вершина обхода
    bool[] visit = new bool[N];  //массив посещения вершин (изначально из нулей)
    for (int k = 0; k < N; k++)
      if (zero.Contains(k))
         visit[k] = true;

    visit[u] = true;//отмечаем начальную вершину как посещенную    
    q.Enqueue(u);//Добавляем начальную вершину в конец очереди
    while (q.Count != 0)//Пока очередь не пустая
      {
        u = q.Peek();//читаем элемент из головы очереди
        q.Dequeue();//удаляем его из головы очереди
        for (int j = 0; j < Matr.GetLength(0); j++)//для каждой вершины из матрицы смежности
         {
           if (!zero.Contains(j))
             if (Matr[u, j] >= 1)//если вершина смежна с вершиной, которую взяли
               if (!visit[j])
                 {
                    visit[j] = true;//то отмечаем ее как посещенную в массиве посещения вершин
                    q.Enqueue(j);//и добавляем ее в конец очереди               
                 }
         }
       }
    for (int i = 0; i < N; i++)
      if (visit[i] == false)
        {
          T = false;
          break;
        }
    return T;
  }
 }
}
