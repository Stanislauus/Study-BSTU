using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

class Program
{
    static readonly object _lock = new object();
    static Timer timer;

    static void Main(string[] args)
    {
        // firstTask
        Console.WriteLine("All running processes:");
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            try
            {
                Console.WriteLine($"ID: {process.Id}, Name: {process.ProcessName}, Priority: {process.BasePriority}, " +
                    $"Start Time: {process.StartTime}, " +
                    $"Total Processor Time: {process.TotalProcessorTime}, " +
                    $"Status: {(process.Responding ? "Running" : "Not Responding")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot access process info: {ex.Message}");
            }
        }
        Console.WriteLine();

        // secondTask
        AppDomain currentDomain = AppDomain.CurrentDomain;
        Console.WriteLine($"Current Domain Name: {currentDomain.FriendlyName}");
        Console.WriteLine("Assemblies in the domain:");
        foreach (Assembly asm in currentDomain.GetAssemblies())
        {
            Console.WriteLine(asm.FullName);
        }
        Console.WriteLine();

        // thirdTask
        Console.Write("Enter the value of n: ");
        int n = int.Parse(Console.ReadLine());

        Thread primeThread = new Thread(() => CalculatePrimes(n));
        primeThread.Start();
        Thread.Sleep(1000);

        Console.WriteLine($"Thread Status: {primeThread.ThreadState}");

        if (primeThread.ThreadState != System.Threading.ThreadState.Stopped)
        {
            Console.WriteLine($"Thread Priority: {primeThread.Priority}");
        }
        else
        {
            Console.WriteLine("Thread has already stopped.");
        }

        Console.WriteLine($"Thread ID: {primeThread.ManagedThreadId}");
        primeThread.Join();
        Console.WriteLine();

        // fourthTask
        Thread evenThread = new Thread(() => PrintEvenNumbers(n));
        Thread oddThread = new Thread(() => PrintOddNumbers(n));

        evenThread.Priority = ThreadPriority.Highest;

        evenThread.Start();
        oddThread.Start();

        evenThread.Join();
        oddThread.Join();
        Console.WriteLine();

        // fifthTask
        TimerCallback timerCallback = new TimerCallback(PrintTime);
        timer = new Timer(timerCallback, null, 0, 1000);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void CalculatePrimes(int n)
    {
        using (StreamWriter writer = new StreamWriter("primes.txt"))
        {
            for (int number = 1; number <= n; number++)
            {
                if (IsPrime(number))
                {
                    Console.WriteLine(number);
                    writer.WriteLine(number);
                }
            }
        }
    }
    static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
    static void PrintEvenNumbers(int n)
    {
        for (int i = 0; i <= n; i += 2)
        {
            lock (_lock)
            {
                using (StreamWriter writer = new StreamWriter("numbers.txt", true))
                {
                    Console.WriteLine($"Even: {i}");
                    writer.WriteLine($"Even: {i}");
                }
            }
            Thread.Sleep(100);
        }
    }
    static void PrintOddNumbers(int n)
    {
        for (int i = 1; i <= n; i += 2)
        {
            lock (_lock)
            {
                using (StreamWriter writer = new StreamWriter("numbers.txt", true))
                {
                    Console.WriteLine($"Odd: {i}");
                    writer.WriteLine($"Odd: {i}");
                }
            }
            Thread.Sleep(150);
        }
    }
    static void PrintTime(object stateе)
    {
        Console.WriteLine($"Current Time: {DateTime.Now}");
    }
}
