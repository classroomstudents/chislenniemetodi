using System;
using System.Linq;
using System.Threading.Tasks;

namespace PUZIRRRRR
{
    class Program
    {
        //Количество элементов массива:
        static int N = 20000;
        //Кол-во участков деления
        static int NumOfArea = 20;
        //Кол-во элементов на участке
        static int NumOfValue = N / NumOfArea;
        //показатель изменения элементов в массиве
        static bool NeedContinue;
        //Массив который будет учавствовать в сортировке(в методе ChoTY и NeChoTY
        static double[] array;
        //функция для нагрузки цп
        static double Load_CPU(int i) {
            return Math.Log(Math.Sin(Math.Exp(Math.Cos(i))));               //return 1;
        }

        //Сортировка чётных участков:
        static void ChoTY(int i)
        {
            //Индекс участка
            int areaIndex = i * 2;
            //startArea - индекс первого элемента участка, endArea - последнего
            int startArea = areaIndex * NumOfValue, endArea = (areaIndex + 2) * NumOfValue - 1;
            //показатель изменения элементов в массиве
            bool CanBreak;
            double boofer;
            //в худшем случае метод пузырька выполняется (endArea - startArea) раз
            for(int count = 0; count < endArea - startArea; count++) 
            {
                CanBreak = true;
                //после прохождения цикла на старшем индексе будет максимальный элемент,
                //т.е. его можно не рассматривать
                for(int j = startArea; j < endArea - count; j++) 
                {
                    Load_CPU(j);
                    if(array[j] > array[j+1]) 
                    {
                        boofer = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = boofer;
                        CanBreak = false;
                        NeedContinue = true;
                    }
                }
                if(CanBreak) break;
            }
        }
    
        //Сортировка нечётных участков:
        static void NeChoTY(int i)
        {
            //double boofer;
            //if (array[i * 2 + 1] > array[i * 2 + 2])
            //{
            //    boofer = array[i * 2 + 1];
            //    array[i * 2 + 1] = array[i * 2 + 2];
            //    array[i * 2 + 2] = boofer;
            //}

            //Индекс участка
            int areaIndex = i * 2 + 1;
            //startArea - индекс первого элемента участка, endArea - последнего
            int startArea = areaIndex * NumOfValue, endArea = (areaIndex + 2) * NumOfValue - 1;
            //показатель изменения элементов в массиве
            bool CanBreak;
            double boofer;
            //в худшем случае метод пузырька выполняется (endArea - startArea) раз
            for(int count = 0; count < endArea - startArea; count++) 
            {
                CanBreak = true;
                //после прохождения цикла на старшем индексе будет максимальный элемент,
                //т.е. его можно не рассматривать
                for(int j = startArea; j < endArea - count; j++)
                    if(array[j] > array[j + 1]) 
                    {
                        boofer = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = boofer;
                        CanBreak = false;
                        NeedContinue = true;
                    }
                if(CanBreak) break;
            }
        }

        //Сортировка
        static void S()
        {
            NeedContinue = true;
            //for (int i = 0; i <= N; i++)
            while(NeedContinue)
            {
                NeedContinue = false;
                //double boofer;
                //Сортировка чётных (начиная с нулевого) элементов массива
                for(int j = 0; j < NumOfArea / 2; j++) {
                    ChoTY(j);
                }
                //Сортировка нечётных (начиная с первого) элементов массива
                //если количество участков - нечетное, то вычитать единицу не нужно
                //( булево_выражение ? выражение_1 : выражение_2 )  аналогично    ( if(булево_выражение) выражение_1;else выражение_2 )
                for(int j = 0; j < (NumOfArea % 2 == 0 ? NumOfArea / 2 - 1 : NumOfArea / 2); j++) {
                    NeChoTY(j);
                }
            }
        }

        //Сортировка (параллельное):
        static void P()
        {
            NeedContinue = true;
            //for (int i = 0; i <= N; i++)
            while(NeedContinue)
            {
                //double boofer;
                NeedContinue = false;
                //Сортировка чётных (начиная с нулевого) элементов массива
                Parallel.For(0, NumOfArea / 2, ChoTY);
                //Сортировка нечётных (начиная с первого) элементов массива
                Parallel.For(0, (NumOfArea % 2 == 0 ? NumOfArea / 2 - 1 : NumOfArea / 2), NeChoTY);
            }
        }
        
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //Переменные для хранения времени работы функций:
            double TS, TP;
            //Создание массива узлов:
            double[] array1 = new double[N];
            double[] array2 = new double[N];
            //Заполнение массива:
            Random rnd = new Random();
            for (int i = 0; i < N; i++)
            {
                array1[i] = array2[i] = rnd.Next(-1000,1000);
                //Console.Write("{0} ",array1[i]);
            }
            //Console.WriteLine();

            sw.Restart();
            //Последовательный пузырёк:
            array = array1;
            S();
            array1 = array;
            TS = sw.Elapsed.TotalSeconds;
            //Вывод результата и времени работы:
            Console.Write("TIME_1 = "); Console.WriteLine(TS);

            //Console.WriteLine("ARRAY VAR 1:");
            //for (int i = 0; i < N; i++)
            //    Console.Write(array1[i]+ " ");
            //Console.WriteLine(" ");

            //Начало отсчёта времени работы функции:
            sw.Restart();
            //Параллельный пузырёк:
            array = array2;
            P();
            array2 = array;
            TP = sw.Elapsed.TotalSeconds;
            Console.Write("TIME_2 = "); Console.WriteLine(TP);

            //Console.WriteLine("ARRAY VAR 2:");
            //for(int i = 0; i < N; i++) 
            //    Console.Write(array2[i] + " ");            
            //Console.WriteLine(" ");

            //Проверка
            Console.WriteLine("Проверка сортировки по библиотеке LINQ");
            double[] check;
            check = array1.AsParallel().OrderBy(a => a).ToArray();
            if(array1.SequenceEqual(check)) Console.WriteLine(nameof(array1) + " отсортирован правильно");
            check = array2.AsParallel().OrderBy(a => a).ToArray();
            if(array2.SequenceEqual(check)) Console.WriteLine(nameof(array2) + " отсортирован правильно");

            //Коэффициент времени:
            Console.Write("RELATION OF TIMES: ");
            Console.WriteLine(TS / TP);
            Console.ReadKey();
        }
    }
}


