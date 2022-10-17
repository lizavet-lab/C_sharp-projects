using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alg_Fleury
{
public partial class Form1 : Form
 {
  int[,] Dist ={//Матрица весов- длин ребер, соединяющих соответствующие вершины
   {0, 5, 0, 0, 0, 5 },
   {5, 0, 4, 0, 0, 6 },
   {0, 4, 0, 5, 6, 0 },
   {0, 0, 5, 0, 5, 0 },
   {0, 0, 6, 5, 0, 4 },
   {5, 6, 0, 0, 4, 0 } };
  Random rand=new Random();
  int[,] G;       //Матрица смежности, соответствующая матрице весов
  List<int> odd;  //Список для хранения нечетных вершин
  List<int> sootv;//Соответствующие им номера в матрице Shortodd
  int[,] Short;   //Матрица кратчайших расстояний между вершинами
  int[,] Shortodd;//Матрица кратчайших расстояний для нечетных вершин
  int N;
  const int INF = int.MaxValue;
  TextBox[,] Mass;
  TextBox[,] Weight;
  TextBox[,] Road;
  int X = 10, Y = 10;
  List<int> Stack = new List<int>();
  int v1;
  List<int> power;//для сохранения степеней каждой вершины
    public Form1()
     {
       InitializeComponent();
       N = (int)numericUpDown1.Value;
       power = new List<int>(N);
       Short = new int[N, N];
       CreateMatrix();
     }
    private void CreateMatrix()
     {
       X = 10;
       Y = 20;
       Weight = new TextBox[N, N];
       G = new int[N, N];//Матрица смежности
       for (int i = 0; i < N; ++i)
        {
           for (int j = 0; j < N; ++j)
            {
             if (N == 6)
               Weight[i, j] = new TextBox { Text = Dist[i, j].ToString(), Location = new Point(X, Y), Width = 30, Height = 30 };
             else
               Weight[i, j] = new TextBox { Text = " ", Location = new Point(X, Y), Width = 30, Height = 30 };
             groupBox1.Controls.Add(Weight[i, j]);
             X += 30;
            }
           X = 10;
           Y += 30;
        }
       }
    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
     {
       groupBox1.Controls.Clear();
       groupBox3.Controls.Clear();
       groupBox2.Controls.Clear();
       N = (int)numericUpDown1.Value;
       power = new List<int>(N);
       CreateMatrix();
     }
    private void Parity() //Проверка графа на четность вершин
     {
       odd = new List<int>();//список нечетных вершин
       sootv = new List<int>();//список нечетных вершин
       int S; //количество ребер, инцидентных выбранной вершине
       int sootvind = 0;
       for (int i = 0; i < N; i++)//для каждой вершины
        {
           S = 0;
           for (int j = 0; j < N; j++)
               S += Convert.ToInt32(Mass[i, j].Text);
           if (S % 2 != 0)//проверяем вершину на четность, если нечетная->
           {
             odd.Add(i);
             sootv.Add(sootvind);
             sootvind += 1;
           }
           power.Add(S);//сохраняем степень каждой вершины в стек 
        }
       odd.Sort();
       if (odd.Count == 0)
       {
         MessageBox.Show("Эйлеров граф. Построение возможно начать с любой вершины");
         label1.Visible = true;
         textBox1.Visible = true;
         button2.Visible = true;
         for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                G[i, j] = Convert.ToInt32(Mass[i, j].Text);
       }
       if (odd.Count == 2)
       {
         MessageBox.Show("Полуэйлеров граф, который необходимо достроить до эйлерова, соединяем 2 нечетные вершины..");
         Mass[odd[0], odd[1]].Text = (Convert.ToInt32(Mass[odd[0], odd[1]].Text) + 1).ToString();
         Mass[odd[1], odd[0]].Text = (Convert.ToInt32(Mass[odd[1], odd[0]].Text) + 1).ToString();
         G[odd[0], odd[1]] += 1;
         G[odd[1], odd[0]] += 1;
         power[odd[0]] += 1;
         power[odd[1]] += 1;
         label1.Visible = true;
         textBox1.Visible = true;
         button2.Visible = true;
         for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
               G[i, j] = Convert.ToInt32(Mass[i, j].Text);
       }
       if (odd.Count != 0 && odd.Count != 2 && odd.Count % 2 == 0)
       {
         MessageBox.Show("Граф, который возможно достроить до эйлерова. Cоединение нечетных вершин..");
         Short = AlgDeykstry.Deykstr(Dist);
         Road = new TextBox[N, N];
         X = 10;
         Y = 20;
         for (int i = 0; i < N; i++)
          {
            Short[i, i] = INF;
             for (int j = 0; j < N; j++){
                Road[i, j] = new TextBox { Text = Short[i, j].ToString(), Location = new Point(X, Y), Width = 40, Height = 30 };
                groupBox2.Controls.Add(Road[i, j]);
                X += 40;
              }
            X = 10;
            Y += 30;
            Road[i, i].Text = "INF";
          }
         //Уменьшаем матрицу расстояний, оставляя только нечетные вершины
         Shortodd = new int[odd.Count, odd.Count];
         List<int> chort = new List<int>();
         int up = -1;
         for (int i = 0; i < N; i++)
            if (odd.Contains(i))
            {
             up += 1;
             for (int j = 0; j < N; j++)
                if (odd.Contains(j))
                  chort.Add(Short[i, j]);
             for (int h = 0; h < odd.Count; h++)
                Shortodd[up, h] = chort[h];
             chort.Clear();
            }
         //Получили shortodd-матрицу расстояний только с нечетными вершинами
         int[,] f = VengAlg.Vengr(Shortodd);
         for (int i = 0; i < f.GetLength(0); i++)
            for (int j = 0; j < f.GetLength(0); j++)
               if (f[i, j] == 0)
               {
                 f[j, i] = INF;
                if (sootv.Contains(i) && sootv.Contains(j))
                  {
                    int v1 = odd.ElementAt(i);
                    int v2 = odd.ElementAt(j);
                    MessageBox.Show("Соединяем вершины " + v1.ToString() + " и " + v2.ToString());
                    G[v1, v2] += 1;
                    G[v2, v1] += 1;
                    power[v1] += 1;
                    power[v2] += 1;
                    Mass[v1, v2].Text = (Convert.ToInt32(Mass[v1, v2].Text) + 1).ToString();
                    Mass[v2, v1].Text = (Convert.ToInt32(Mass[v2, v1].Text) + 1).ToString();
                  }
               }
         MessageBox.Show("Изменили матрицу смежности");
         groupBox2.Visible = true;
         label1.Visible = true;
         textBox1.Visible = true;
         button2.Visible = true;
         label3.Visible = true;
         button3.Visible = true;
       }
     }
    private void button1_Click(object sender, EventArgs e)
     {
      X = 10;
      Y = 20;
      Mass = new TextBox[N, N];//Массив для отображения матрицы смежности
      Dist = new int[N, N];
      G = new int[N, N];
      for (int i = 0; i < N; i++)
       {
          for (int j = 0; j < N; j++)
           {
             G[i, j] = 0;
             Dist[i, j] = Convert.ToInt32(Weight[i, j].Text);
             if (Dist[i, j] != 0)
               G[i, j] = 1;
             Mass[i, j] = new TextBox { Text = G[i, j].ToString(), Location = new Point(X, Y), Width = 30, Height = 30 };
             groupBox3.Controls.Add(Mass[i, j]);
             X += 30;
           }
          X = 10;
          Y += 30;
       }
      groupBox3.Visible = true;
      if (WidthRound.Round(G, rand.Next(0, N)) == true)
         Parity();
      else       
         MessageBox.Show("Неэйлеров граф. Построение эйлерова цикла невозможно.");
     }

        private void button2_Click(object sender, EventArgs e)
     {
       v1 = Convert.ToInt32(textBox1.Text);
       int[,] Gcopy= new int[N,N];
       List<int> powercopy = new List<int>(N);
       for (int i = 0; i < N; i++)
        {
          powercopy.Add(power[i]);
          for (int j = 0; j < N; j++)
              Gcopy[i, j] = G[i, j];
        }
       string seq = AlgFleury.Fleury(Gcopy, v1,powercopy);
       MessageBox.Show(seq);
      }

        private void button3_Click(object sender, EventArgs e)
     {
       v1 = Convert.ToInt32(textBox1.Text);
       int[,] Gcopy = new int[N, N];
       List<int> powercopy = new List<int>(N);
       for (int i = 0; i < N; i++)
        {
          powercopy.Add(power[i]);
          for (int j = 0; j < N; j++)
              Gcopy[i, j] = G[i, j];
        }
       Genetic_Process.Tour(Gcopy, v1, powercopy);
     }
    }
}
