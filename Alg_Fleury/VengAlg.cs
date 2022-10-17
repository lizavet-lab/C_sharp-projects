using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Alg_Fleury
{
 public class VengAlg
 {
  static int [,] a;
  static int n;
  //матрица для дальнейшей копии массива а:
  static int [,] copy;
  const int INF = int.MaxValue;
  //список для подсчета количества нулей в строках:
  static List<int> strzero = new List<int>(); 
  public static int[,] Vengr (int[,] Mas)
  {
   n = Mas.GetLength(0);
   a = new int[n, n];
   for (int i = 0; i < n; i++)
     for (int j = 0; j < n; j++)
         a[i, j] = Mas[i, j];

   // 1. Редуцируем матрицу сначала по строкам, затем по столбцам:
   int min;
   for (int i = 0; i < n; i++) //для каждой строки
    {
      min = a[i, 0];
      for (int j = 1; j < n; j++)
         if (a[i, j] < min)
            min = a[i, j]; //Находим минимальный элемент в строке
      for (int j = 0; j < n; j++)
         a[i, j] -= min;    //Вычитаем из каждого элемента строки минимальный элемент
    }
   // 2. Затем по столбцам:
    for (int j = 0; j < n; j++) //для каждого столбца
     {
       min = a[0, j];
       for (int i = 1; i < n; i++)
          if (a[i, j] < min)
             min = a[i, j]; //Находим минимальный элемент в столбце
       for (int i = 0; i < n; i++)
          a[i, j] -= min;    //Вычитаем из каждого элемента столбца минимальный элемент
     }
    //3. С помощью преобразований метода Changezero выполняем поиск нулей матрицы : 
    Changezero();
    return copy;
  }
  static void Changezero()
  {
   bool T = true;
   int[,] Tag = new int[n, n];//Матрица для отмеченных нулей
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            Tag[i, j] = 0;     //Изначально пустая
    copy = new int[n, n];
    for (int i = 0; i < n; i++)
        {
          strzero.Add(0);
          for (int j = 0; j < n; j++)
             if (a[i, j] == 0)
                strzero[i] += 1;
        }
     do
     {
       int str = strzero.IndexOf(strzero.Min());//находим строку с минимальным количеством нулей
       int stb;
       //stritzero.RemoveAll(item => item == 0);
       //Оставляем по одному нулю на строку и столбец
       for (int j = 0; j < n; j++)
        {
         if (a[str, j] == 0)
            {
              stb = j;// Нашли первый ноль в строке str
              Tag[str, stb] = 1;
              for (int h = 0; h < n; h++) //идем заново по строке
                if (a[str, h] == 0 && h != j)
                {
                  a[str, h] = INF;//и все другие нули заменяем на INF
                  Tag[str, h] = 2;//будет означать зачеркнутый ноль
                }
              for (int h = 0; h < n; h++)//идем заново по столбцу
                if (a[h, stb] == 0 && h != str)
                {
                   a[h, stb] = INF;//и все другие нули заменяем на INF
                   Tag[h, stb] = 2;//будет означать зачеркнутый ноль
                }
             }
         }
       strzero[str] = INF;//отмечаем строку пройденной
       int u = 0;
       for (int i = 0; i < n; i++)
         if (strzero[i] == INF)
            u += 1;
       if (u == n)
          T = false;
     } while (T == true);

     for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
           copy[i, j] = a[i, j];

     List<int> metkastr = new List<int>();
     List<int> metkastb = new List<int>();
     //а) помечаем крестиком (x) все те строки, которые не содержат 
     //ни одного обведенного квадратиком нуля;
     bool T2 = true;// означает, что исследуемая строка без помеченных нулей
     for (int i = 0; i < n; i++)
      {
         for (int j = 0; j < n; j++)
          {
             if (Tag[i, j] == 1)//если нашелся помеченный ноль
             {
                T2 = false;//то строка содержит помеченные нули
                break;
             }
          }
         if (T2 == true)
            metkastr.Add(i);//отмечаем строку
         T2 = true;
      }
     //б) отмечаем каждый столбец, содержащий перечеркнутый нуль хотя бы в одной из помеченных строк;
     for (int i = 0; i < metkastr.Count; i++)//идем по отмеченным строкам
        for (int j = 0; j < n; j++)
           if (Tag[metkastr[i], j] == 2)//если в строке есть зачеркнутый ноль
              metkastb.Add(j);//отмечаем столбцы с зачеркнутиым нулем
     metkastb.Distinct();//оставляем столбцы без повтора

     //в) отметим каждую строку,имеющую обведенный квадратиком нуль хотя бы в 
     //одном из помеченных столбцов;
     for (int j = 0; j < metkastb.Count; j++)//идем по отмеченным столбцам
         for (int i = 0; i < n; i++)
             if (Tag[i, metkastb[j]] == 1)//если в строке есть помеченный ноль
                metkastr.Add(i);//отмечаем строки с зачеркнутиым нулем
     metkastr.Distinct();//оставляем строки без повтора

     //г) Перечеркиваем каждую непомеченную строку и каждый помеченный столбец
     for (int i = 0; i < n; i++)
        if (metkastr.Contains(i))
            { }
        else
           for (int j = 0; j < n; j++)
               a[i, j] = INF;
     for (int j = 0; j < n; j++)
        if (metkastb.Contains(j))
           for (int i = 0; i < n; i++)
              a[i, j] = INF;

      //Ищем минимальный неперечеркнутый элемент
      List<int> notcross = new List<int>();
      for (int i = 0; i < n; i++)
          for (int j = 0; j < n; j++)
             if (a[i, j] != INF)
                 notcross.Add(a[i, j]);
      if (notcross.Count != 0)
      {
        int min = notcross.Min();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
               if (a[i, j] != INF)
                  copy[i, j] -= min;
      }
 }}}
