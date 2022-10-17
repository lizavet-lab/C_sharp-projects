using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alg_Fleury
{
    public class Genetic_Process
    {
        static int Size = 20000;//размер популяции
        static List<int[]> Pop = new List<int[]>();// популяция хромосом, которые являются массивами генов
        static List<int[]> Major;// список сильнейших
        static List<int[]> FirstHalf;//мужские особи
        static List<int[]> SecondHalf;//женские особи
        static List<int[]> Progeny;//Потомство от сильнейших
        static int n;   //количество вершин графа
        static int[,] G;//Матрица смежности
        static List<int> power = new List<int>();//список степеней вершин
        static int S = 0; // Сумма степеней вершин графа (всегда четна)
        static int kol;//количество ребер в графе
        static int v0;
        static int[] Parent1; // 1-ый родитель
        static int[] Parent2; // 2-ой родитель
        //static int p1, p2; // номера родителей в популяции
        static int[] Children1; // 1-ый ребенок
        static int[] Children2; // 2-ой ребенок
        static Random rand = new Random(DateTime.Now.Millisecond);
        static bool T = false; //найдено ли решение
        static int iteration = 0;

        public static void Tour(int[,] Mas, int v, List<int> pow)
        {
            n = Mas.GetLength(0);
            G = new int[n, n];
            v0 = v;
            for (int i = 0; i < n; i++)
            {
                int fi = pow[i];
                power.Add(fi);
                for (int j = 0; j < n; j++)
                {
                    int ksi = Mas[i, j];
                    G[i, j] = ksi;
                    S += ksi;
                }
            }
            kol = S / 2;
            GeneratePop(v0);
            while (T == false)
            {
                Selection(Pop);
                Separation();
                Crossover();
                if (Pop.Count() == 0)
                {
                    MessageBox.Show("Популяция вымерла на этапе поиска решения");
                    break;
                }
            };
        }

        public static void GeneratePop(int v0)
        //генерируем популяцию хромосом, в каждой из которых первый и последний ген принимает значение начальной вершины обхода
        {
            Pop = new List<int[]>();
            for (int i = 0; i < Size; i++)
            {
                Pop.Add(Enumerable.Range(0, kol + 1).Select(z => rand.Next(0, n)).ToArray());
                Pop[i][0] = v0;
                Pop[i][kol] = v0;
                rand = new Random(rand.Next(1,30000));
            }
            //for (int i = 0; i < Size; i++)
            //{
            //    string seq = "";
            //    for (int h = 0; h < 10; h++)
            //        seq += (Pop[i][h].ToString() + " - ");
            //    seq += Pop[i][10].ToString();
            //    MessageBox.Show("Pop[" + i.ToString() + "]: " + seq);
        }

        public static void Selection(List<int[]> Pop)
        {
            Major = new List<int[]>();
            int index=0;
            for (int i = 0; i < Pop.Count(); i++)
            {
                int k = Round(Pop[i]);
                if (k > iteration)
                {
                    Major.Add(Pop[i]);
                    index += 1;
                       string seq = "";
                       for (int h = 0; h < Pop[0].Count()-1; h++)
                           seq += (Major[index-1][h].ToString() + " - ");
                       seq += Major[index-1][Pop[0].Count()-1].ToString();
                }
            }
            //убираем сильную особь, которой не достанется пара
            if (Major.Count() % 2 == 1)
                Major.RemoveAt(rand.Next(0, Major.Count()));
        }

        public static void Separation()
        {
            //делим особей по парам
            int half = Major.Count() / 2;
            FirstHalf = new List<int[]>();
            SecondHalf = new List<int[]>();

            for (int i = 0; i < half; i++)
            {
                FirstHalf.Add(Major[i]);    
                SecondHalf.Add(Major[i + half]);
            }
        }

        public static void Crossover()
        {
            Progeny = new List<int[]>();
            for (int i = 0; i < FirstHalf.Count(); i++)
            {
                //Берем пару родителей
                Parent1 = FirstHalf[i];
                Parent2 = SecondHalf[i];
                //Проверяем их на длину решения
                int k1 = Round(Parent1);
                int k2 = Round(Parent2);
                if ((k1 == kol) || (k2 == kol))
                {
                    MessageBox.Show("Решение найдено на "+iteration.ToString()+" поколении");
                    if (k1 == kol)
                    {
                        string seq1 = "";
                        for (int h = 0; h < kol; h++)
                            seq1 += (Parent1[h].ToString() + " - ");
                        seq1 += Parent1[kol].ToString();
                        MessageBox.Show(seq1);
                    }
                    if (k2 == kol)
                    {
                        string seq2 = "";
                        for (int h = 0; h < kol; h++)
                            seq2 += (Parent2[h].ToString() + " - ");
                        seq2 += Parent2[kol].ToString();
                        MessageBox.Show(seq2);
                    }

                    T = true;
                    break;
                }

                if (T == false)//если решение не найдено
                {
                    //создаем потомков
                    Children1 = new int[kol + 1];
                    Children2 = new int[kol + 1];
                    for (int m = 0; m < kol + 1; m++)
                    {
                        Children1[m] = Parent1[m];
                        Children2[m] = Parent2[m];
                    }

                    //выбираем точку кроссовера родителей
                    int lokus = Math.Max(k1, k2);
                    //производим кроссовер и получаем потомство
                    for (int piece = 0; piece < lokus + 1; piece++)
                    {
                        Children1[piece] = Parent2[piece];
                        Children2[piece] = Parent1[piece];
                    }
                    Progeny.Add(Children1);
                    Progeny.Add(Children2);
                }
            }
            if (T == false)
            {
                MessageBox.Show("Поколение " + (iteration + 1).ToString() + " в количестве " + Progeny.Count().ToString());
                // Меняем поколение:
                Pop.Clear();
                for (int i = 0; i < Progeny.Count(); i++)
                    Pop.Add(Progeny[i]);
                iteration += 1;
            }

        }
        public static int Round(int[] chr)
        {
            int [,] G1 = new int[n, n];//копия #1 матрицы смежности 
            int [,] G2 = new int[n, n];//копия #2 матрицы смежности
            List<int> power1 = new List<int>();// копия списка степеней вершин
            for (int i = 0; i < n; i++)
            {
                int up = power[i];
                power1.Add(up);
                for (int j = 0; j < n; j++)
                {
                    int yp = G[i, j];
                    G1[i, j] = yp;
                    G2[i, j] = yp;
                }
            }
            List<int> zero = new List<int>();//список вершин, у которых степень стала равной нулю

            int count = 0;//сколько ребер удалось пройти


            for (int i = 0; i < chr.Count() - 1; i++)
            {
                if (G1[chr[i], chr[i + 1]] != 0 && power1[chr[i + 1]] != 0)
                {
                    G2[chr[i], chr[i + 1]] -= 1;
                    G2[chr[i + 1], chr[i]] -= 1;
                    if (power1[chr[i]] == 1)
                        zero.Add(chr[i]);
                    if (WidthRound.Round(G2, chr[i + 1], zero) == true)//если ребро не мост
                    {
                        G1[chr[i], chr[i + 1]] -= 1;
                        G1[chr[i + 1], chr[i]] -= 1;
                        power1[chr[i]] -= 1;
                        power1[chr[i + 1]] -= 1;
                        count += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                    break;
            }
            return count;
        }
    }
}
