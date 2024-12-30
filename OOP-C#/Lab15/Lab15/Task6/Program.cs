using System;
using System.Threading.Tasks;
using System.Threading;

namespace ParallelInvokeExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Начало выполнения программы.\n");

            // Параллельное выполнение блока операторов
            Parallel.Invoke(
                () => CalculateSquare(3),
                () => CalculateDouble(4),
                () => CalculateCube(2)
            );

            Console.WriteLine("\nВсе задачи выполнены.");
            Console.ReadKey();
        }

        // Метод для вычисления квадрата числа
        static void CalculateSquare(int number)
        {
            Console.WriteLine($"Вычисление квадрата числа {number} начато.");
            Thread.Sleep(1000); // Эмуляция длительной операции
            Console.WriteLine($"Квадрат числа {number} равен {number * number}.");
        }

        // Метод для вычисления удвоенного значения числа
        static void CalculateDouble(int number)
        {
            Console.WriteLine($"Вычисление удвоенного значения числа {number} начато.");
            Thread.Sleep(1500); // Эмуляция длительной операции
            Console.WriteLine($"Удвоенное значение числа {number} равно {number * 2}.");
        }

        // Метод для вычисления куба числа
        static void CalculateCube(int number)
        {
            Console.WriteLine($"Вычисление куба числа {number} начато.");
            Thread.Sleep(2000); // Эмуляция длительной операции
            Console.WriteLine($"Куб числа {number} равен {number * number * number}.");
        }
    }
}
