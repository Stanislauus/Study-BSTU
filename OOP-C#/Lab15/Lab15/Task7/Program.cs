using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Создаем склад с максимальной вместимостью 10 товаров
        var warehouse = new BlockingCollection<string>(10);

        // Массив товаров
        string[] products = new string[] { "Телевизор", "Холодильник", "Стиральная машина", "Микроволновка", "Пылесос" };

        // Задаем поставщиков
        Task[] suppliers = new Task[5];
        for (int i = 0; i < 5; i++)
        {
            int supplierIndex = i;
            suppliers[i] = Task.Run(() =>
            {
                while (true)
                {
                    // Поставщик привозит товар
                    string product = products[supplierIndex];
                    warehouse.Add(product);
                    Console.WriteLine($"Поставщик {supplierIndex + 1} привез товар: {product}");
                    PrintWarehouseStatus(warehouse);

                    // Задержка для имитации времени завоза товара
                    Thread.Sleep(new Random().Next(500, 1000));
                }
            });
        }

        // Задаем покупателей
        Task[] buyers = new Task[10];
        for (int i = 0; i < 10; i++)
        {
            int buyerIndex = i;
            buyers[i] = Task.Run(() =>
            {
                while (true)
                {
                    // Покупатель пытается купить товар
                    if (warehouse.TryTake(out string boughtItem))
                    {
                        Console.WriteLine($"Покупатель {buyerIndex + 1} купил товар: {boughtItem}");
                    }
                    else
                    {
                        Console.WriteLine($"Покупатель {buyerIndex + 1} ушел: товара нет.");
                    }

                    // Задержка для имитации времени покупки товара
                    Thread.Sleep(new Random().Next(500, 1000));
                }
            });
        }

        // Ожидаем завершения всех задач (в реальной задаче можно использовать CancellationToken для корректного завершения)
        Task.WhenAll(suppliers.Concat(buyers)).Wait();
    }

    // Метод для вывода текущего состояния склада
    static void PrintWarehouseStatus(BlockingCollection<string> warehouse)
    {
        Console.WriteLine($"Товары на складе: {string.Join(", ", warehouse)}");
    }
}
