using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
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

            // Ожидаем завершения первых трех задач 
            int square = await task1;
            int doubleValue = await task2;
            int cube = await task3;

            // Задача 4: Используем результаты для расчета по формуле
            Task<int> task4 = Task.Run(() =>
            {
                Console.WriteLine("Задача 4: Расчет по формуле (square + doubleValue - cube)...");
                return square + doubleValue - cube;
            });

            //Ожидание результата четвертой задачи
            int result = await task4;

            // Вывод результата
            Console.WriteLine($"Результат расчета: {result}");

            Console.ReadKey();
        }
    }
}
