using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;

namespace Lab15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //создаем объект Stopwatch для измерения времени
            Stopwatch stopwatch = new Stopwatch();

            int numberOfRuns = 3;

            for (int n = 1; n <= numberOfRuns; n++)
            {
                Console.WriteLine($"\nПрогон {n} из {numberOfRuns}:");

                Thread.Sleep(1500);

                stopwatch.Start();

                Task task1 = Task.Run(() =>
                {
                    int size = 100;
                    int[] vector = new int[size];

                    for (int i = 0; i < size; i++)
                    {
                        vector[i] = i + 1;
                    }

                    Console.WriteLine($"\nВектор размером {size} элементов создан:\n");
                    foreach (int item in vector)
                    {
                        Console.Write($"{item * 2}\t");
                    }
                });

                //получаем информацию о задаче
                Console.WriteLine($"task1 Id: {task1.Id}");
                Console.WriteLine($"task1 is Completed: {task1.IsCompleted}");
                Console.WriteLine($"task1 Status: {task1.Status}");

                task1.Wait();// Основной поток блокируется, пока задача не завершится

                //для примера после Wait()
                /*
                Console.WriteLine($"\n\ntask1 Id: {task1.Id}");
                Console.WriteLine($"task1 is Completed: {task1.IsCompleted}");
                Console.WriteLine($"task1 Status: {task1.Status}");
                */

                stopwatch.Stop();

                Console.WriteLine($"\n\nВремя выполнения прогона {n}: {stopwatch.ElapsedMilliseconds} мс");

                // Сбрасываем stopwatch для следующего прогона
                stopwatch.Reset();

                Console.WriteLine("\n------------------------------------------\n");

            }


            Console.ReadKey();
        }
    }
}
