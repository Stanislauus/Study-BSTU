using System;
using System.Text.RegularExpressions;

class Program
{
    // Метод для удаления знаков препинания
    static string RemovePunctuation(string input)
    {
        return Regex.Replace(input, @"[^\w\s]", "");
    }

    // Метод для добавления символов в начало и конец строки
    static string AddSymbols(string input, char symbol)
    {
        return $"{symbol}{input}{symbol}";
    }

    // Метод для замены всех символов на заглавные
    static string ToUpperCase(string input)
    {
        return input.ToUpper();
    }

    // Метод для удаления лишних пробелов
    static string RemoveExtraSpaces(string input)
    {
        return Regex.Replace(input.Trim(), @"\s+", " ");
    }

    // Метод для вывода текущей строки (используем Action)
    static void PrintCurrentString(string current)
    {
        Console.WriteLine("Текущая строка: " + current);
    }

    // Метод для проверки, пуста ли строка (используем Predicate)
    static bool IsStringEmpty(string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    // Главный метод для демонстрации
    static void Main(string[] args)
    {
        string input = "Пример, строки! с лишними  пробелами  и знаками, препинания.";
        Console.WriteLine("Исходная строка: " + input);

        // Последовательная обработка строки
        Func<string, string> processString = RemovePunctuation;
        processString += inputStr => AddSymbols(inputStr, '*');
        processString += ToUpperCase;
        processString += RemoveExtraSpaces;

        // Используем Action для вывода текущего состояния строки после каждой операции
        Action<string> printAction = PrintCurrentString;

        // Используем Predicate для проверки, пуста ли строка после обработки
        Predicate<string> isEmptyCheck = IsStringEmpty;

        // Выполнение обработки с промежуточным выводом
        string result = input;
        foreach (Func<string, string> func in processString.GetInvocationList()) //метод, который возвращает массив всех методов
        {
            result = func(result);
            printAction(result);//для вывода промежуточных результатов

            // Проверка, не стала ли строка пустой
            if (isEmptyCheck(result))
            {
                Console.WriteLine("Строка стала пустой после обработки. Останавливаем процесс.");
                break;
            }
        }

        Console.WriteLine("Конечная обработанная строка: " + result);

        Console.ReadKey();
    }
}
