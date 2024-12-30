using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

class Program
{
    static void Main()
    {
        int[] array = new int[1000000];

        // Обычный цикл for
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Thread.Sleep(1000);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = array[i] * 2;  // Пример обработки (умножение на 2)
        }
        stopwatch.Stop();
        Console.WriteLine("Обычный цикл for: " + stopwatch.ElapsedMilliseconds + " ms");

        // Обычный цикл foreach
        stopwatch.Restart();
        Thread.Sleep(1000);
        foreach (var item in array)
        {
            // Пример обработки (умножение на 2)
            // Так как foreach не позволяет изменять элементы, делаем обработку внутри массива
            int index = Array.IndexOf(array, item);
            array[index] = item * 2;
        }
        stopwatch.Stop();
        Console.WriteLine("Обычный цикл foreach: " + stopwatch.ElapsedMilliseconds + " ms");

        // Parallel.For
        stopwatch.Restart();
        Thread.Sleep(1000);
        Parallel.For(0, array.Length, i =>
        {
            array[i] = array[i] * 2;  // Пример обработки (умножение на 2)
        });
        stopwatch.Stop();
        Console.WriteLine("Parallel.For: " + stopwatch.ElapsedMilliseconds + " ms");

        // Parallel.ForEach
        stopwatch.Restart();
        Thread.Sleep(1000);
        Parallel.ForEach(array, item =>
        {
            // Пример обработки (умножение на 2)
            int index = Array.IndexOf(array, item);
            array[index] = item * 2;
        });
        stopwatch.Stop();
        Console.WriteLine("Parallel.ForEach: " + stopwatch.ElapsedMilliseconds + " ms");

        Console.ReadKey();
    }
}
