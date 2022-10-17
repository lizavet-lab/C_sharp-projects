using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Alg_Fleury
{
public class AlgFleury
{
 static int N;
 static int[,] G;//Матрица смежности
 static int[,] G2;//Копия матрицы смежности
 //static int[,] K; //Копия копии матрицы смежности
 //static int[,] Reduce; //Матрица, которая не будет содержать изолированную вершину
 static List<int> vertex=new List<int>(); //список вершин
 static List<int> power;         //список степеней вершин
 static List<int> zero = new List<int>();//список вершин, у которых степень стала равной нулю
 static List<int> Stack = new List<int>();//последовательность обхода вершин
 public static string Fleury(int[,] Matr, int v1, List<int> pow)
  {   
   N = Matr.GetLength(0);
   power = pow;
   G = new int[N, N];
   G2 = new int[N, N];
   int kol = 0;
   for (int i = 0; i < N; i++)
    {
       vertex.Add(i);
       for (int j = 0; j < N; j++)
        {
         G[i, j] = Matr[i, j];
         G2[i, j] = Matr[i, j];
         if (G[i, j] != 0)
           kol += G[i, j];
        }
    }
   kol = kol / 2;//rколичество ребер
            int v = v1;//начальная вершина обхода
   Stack.Add(v);
   int count = 0;//сколько ребер прошли
   List<int> banned=new List<int>();//список вершин, в которые ходить не нужно
   do
    {
     for (int j = 0; j < N; j++)
      {
         if (!banned.Contains(j) && G[v, j] != 0 && power[j] != 0)
          {
           G2[v, j] -= 1;
           G2[j, v] -= 1;
           if (power[v] == 1)
             zero.Add(v);
           if (WidthRound.Round(G2, j, zero) == true)//если ребро не мост
             {
              Stack.Add(j); //добавляем найденную вершину
              G[v, j] -= 1;
              G[j, v] -= 1;                             
              power[v] -= 1;
              power[j] -= 1;
              v = j;
              count += 1;
              banned.Clear();
              break;
             }
             else
             {                      
              banned.Add(j);//если ребро мост, то не рассматриваем соответствующую смежную вершину
              G2[v, j] += 1;
              G2[j, v] += 1;
              zero.Remove(j);
             }
          }
      }
    } while (count!=kol);

   string seq = "";
   for (int h = 0; h < Stack.Count-1; h++)
       seq += (Stack[h].ToString()+" - ");
   seq += Stack[Stack.Count - 1].ToString();
   return seq;
  }
 }
}