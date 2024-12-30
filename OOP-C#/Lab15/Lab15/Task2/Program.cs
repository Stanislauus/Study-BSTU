using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lab15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем объект Stopwatch для измерения времени
            Stopwatch stopwatch = new Stopwatch();

            int numberOfRuns = 3; // Количество прогонов

            for (int n = 1; n <= numberOfRuns; n++)
            {
                Console.WriteLine($"\nПрогон {n} из {numberOfRuns}:");

                //Создаем объект, который будет управлять токеном отмены.
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                //Получаем сам токен отмены.
                CancellationToken token = cancellationTokenSource.Token;

                stopwatch.Start();

                // Создаем задачу с токеном отмены
                Task task1 = Task.Run(() =>
                {
                    int size = 100;
                    int[] vector = new int[size];

                    for (int i = 0; i < size; i++)
                    {
                        // Проверяем, был ли запрошен токен отмены
                        if (token.IsCancellationRequested)
                        {
                            Console.WriteLine("\nЗадача отменена до завершения создания вектора.");
                            return;
                        }
                        vector[i] = i + 1;
                        Thread.Sleep(10); // Эмулируем длительный процесс
                    }

                    Console.WriteLine($"\nВектор размером {size} элементов создан:\n");
                    foreach (int item in vector)
                    {
                        // Проверяем отмену перед каждой итерацией
                        if (token.IsCancellationRequested)
                        {
                            Console.WriteLine("\nЗадача отменена во время обработки вектора.");
                            return;
                        }
                        Console.Write($"{item * 2}\t");
                        Thread.Sleep(10); // Эмулируем длительный процесс
                    }
                }, token);

                // Для демонстрации отмены задачи в одном из прогонов
                if (n == 2)
                {
                    Thread.Sleep(200); // Даем задаче немного времени на выполнение
                    Console.WriteLine("\nЗапрос на отмену задачи...");
                    cancellationTokenSource.Cancel(); // Отменяем задачу
                }

               
                task1.Wait();// Основной поток блокируется, пока задача не завершится


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
