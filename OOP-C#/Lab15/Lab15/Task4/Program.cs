using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuationTaskExample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Задача 1: Вычисляем квадрат числа
            Task<int> task1 = Task.Run(() =>
            {
                int number = 3;
                Console.WriteLine($"Задача 1: Квадрат числа {number}...");
                return number * number;
            });

            // Задача 2: Вычисляем удвоенное значение числа
            Task<int> task2 = Task.Run(() =>
            {
                int number = 4;
                Console.WriteLine($"Задача 2: Удвоенное значение числа {number}...");
                return number * 2;
            });

            // Задача 3: Вычисляем куб числа
            Task<int> task3 = Task.Run(() =>
            {
                int number = 2;
                Console.WriteLine($"Задача 3: Куб числа {number}...");
                return number * number * number;
            });

            // 1. Создаем задачу продолжения с использованием ContinueWith
            Task continuation = Task.WhenAll(task1, task2, task3)// запускает задачу, которая завершится, когда завершатся все переданные задачи
                .ContinueWith(a =>
                {
                    int square = task1.Result;
                    int doubleValue = task2.Result;
                    int cube = task3.Result;

                    int result = square + doubleValue - cube;
                    Console.WriteLine($"ContinueWith: Результат вычисления (square + doubleValue - cube) = {result}");
                });

            await continuation;

            // 2. Использование GetAwaiter() и GetResult()
            var awaiter1 = task1.GetAwaiter(); //Блокирует выполнение текущего потока, если задача ещё не завершен
            var awaiter2 = task2.GetAwaiter(); //предоставляет низкоуровневый доступ к результатам задачи.
            var awaiter3 = task3.GetAwaiter();

            int squareResult = awaiter1.GetResult();//синхронно
            int doubleValueResult = awaiter2.GetResult();
            int cubeResult = awaiter3.GetResult();

            // Задача продолжения
            Task continuationWithAwaiter = Task.Run(() =>
            {
                int result = squareResult + doubleValueResult - cubeResult;
                Console.WriteLine($"GetAwaiter: Результат вычисления (squareResult + doubleValueResult - cubeResult) = {result}");
            });

            await continuationWithAwaiter;

            Console.ReadKey();
        }
    }
}
