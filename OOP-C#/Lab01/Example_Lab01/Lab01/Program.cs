using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;//Для работы с StringBuilder
using System.Threading.Tasks;

namespace Lab01
{
    class Class01
    {
        static void Main(string[] args)
        {
            bool Bool = true;//2 байт
            string String = "Hello";
            byte Byte = 255; //беззнаковое, от 0 до 255
            sbyte Sbyte = 127; //знаковое, от -128 до 127
            char Char = '!'; //2 байт
            decimal Decimal = 7922816251426433759354395033m; //16 байт
            double Double = 12.13245;//8 байт
            float Float = 3.14f;//4 байт
            int Int = 1234;//знаковое, 4 байт
            uint Uint = 5678;//беззнаковое, 4 байт
            long Long = 12345678912345;//знаковое, 8 байт
            ulong Ulong = 12334434344;//беззнаковое, 8 байт
            short Short = 327;//знаковое, 2 байт
            ushort Ushort = 657;//беззнаковое, 2 байт

            Console.WriteLine("Задание 1\n");

            Console.WriteLine("bool: " + Bool);
            Console.WriteLine("string: " + String);
            Console.WriteLine("byte: " + Byte);
            Console.WriteLine("sbyte: " + Sbyte);
            Console.WriteLine("char: " + Char);
            Console.WriteLine("decimal: " + Decimal);
            Console.WriteLine("double: " + Double);
            Console.WriteLine("float: " + Float);
            Console.WriteLine("int: " + Int);
            Console.WriteLine("uint: " + Uint);
            Console.WriteLine("long: " + Long);
            Console.WriteLine("ulong: " + Ulong);
            Console.WriteLine("short: " + Short);
            Console.WriteLine("ushort: " + Ushort);

            Console.WriteLine("--------------------");

            Console.WriteLine("Введите значение для bool: ");
            string boolValue = Console.ReadLine();
            Bool = Convert.ToBoolean(boolValue);
            Console.WriteLine("Значение для bool: " + Bool);

            Console.WriteLine("Введите значение для string: ");
            String = Console.ReadLine();
            Console.WriteLine("Значение для string: " + String);

            Console.WriteLine("Ведите значение для int: ");
            string intValue = Console.ReadLine();
            Int = Convert.ToInt32(intValue);
            Console.WriteLine("Значение для int: " + Int);

            Console.WriteLine("Введите значение для double: ");
            string doubleValue = Console.ReadLine();
            Double = Convert.ToDouble(doubleValue);
            Console.WriteLine("Значение для double: " + Double);

            Console.WriteLine("--------------------");

            // Неявное приведение 
            int a = 10;
            double b = a;  
            float c = a;   
            long d = a;    

            short e = 5;
            int f = e;
            double g = f;

            Console.WriteLine("Неявные преобразования:");
            Console.WriteLine("int: " + a);
            Console.WriteLine("int -> double: " + b);
            Console.WriteLine("int -> float: " + c);
            Console.WriteLine("int -> long: " + d);
            Console.WriteLine("short: " + e);
            Console.WriteLine("short -> int: " + f);
            Console.WriteLine("int -> double: " + g);

            // Явное приведение 
            double h = 9.99;
            int i = (int)h; 
            float j = (float)h; 
            long k = (long)h; 
            byte l = (byte)h; 
            sbyte m = (sbyte)h; 

            Console.WriteLine("\nЯвные преобразования:");
            Console.WriteLine("double: " + h);
            Console.WriteLine("double -> int: " + i);
            Console.WriteLine("double -> float: " + j);
            Console.WriteLine("double -> long: " + k);
            Console.WriteLine("double -> byte: " + l);
            Console.WriteLine("double -> sbyte: " + m);

            Console.WriteLine("--------------------");

            // Упаковка 
            int myValue = 123;
            object boxedValue = myValue; 
            Console.WriteLine("Упакованное значение: " + boxedValue);

            // Распаковка
            int unboxedValue = (int)boxedValue; 
            Console.WriteLine("Распакованное значение: " + unboxedValue);

            Console.WriteLine("--------------------");

            // Неявное определение типов с использованием var
            var myInt = 10; 
            var myString = "Hello, World!"; 
            var myDouble = 3.14; 
            var myBool = true;
            
            // Вывод типов и значений
            Console.WriteLine("Неявное определение типов с использованием var:");
            Console.WriteLine("myInt (int): " + myInt);
            Console.WriteLine("myString (string): " + myString);
            Console.WriteLine("myDouble (double): " + myDouble);
            Console.WriteLine("myBool (bool): " + myBool);

            Console.WriteLine("--------------------");

            //Испльзование Nullable
            Nullable<int> num1 = null;
            if (num1.HasValue) //HasValue — это свойство nullable типа, которое возвращает true или false
            {
                Console.WriteLine("num1: " + num1.Value); //Value возвращает само значение, которое хранится в nullable переменной
            }
            else
            {
                Console.WriteLine("num1: не имеет значения (null)");
            }

            Nullable<int> num2 = 2;
            if (num2.HasValue)  
            {
                Console.WriteLine("num2: " + num2.Value);
            }
            else
            {
                Console.WriteLine("num2: не имеет значения (null)");
            }

            Console.WriteLine("--------------------");

            //Определение переменной типа var
            Console.WriteLine("Причина ошибки с var:");
            //var — это не динамический тип
            var myVar = 10; // Компилятор выводит тип как int
            //myVar = "Hello"; // Ошибка: Невозможно присвоить строку переменной типа int
            Console.WriteLine("Вывод переменной типа var: " + myVar);

            Console.WriteLine("--------------------\n\nЗадание 2\n");

            //Строковые литералы
            string str1 = "hi";
            string str2 = "wow";
            string str3 = "HI";
            string str4 = "hi";

            Console.WriteLine("Строковые литералы:");
            Console.WriteLine("str1: " + str1);
            Console.WriteLine("str2: " + str2);
            Console.WriteLine("str3: " + str3);
            Console.WriteLine("str4: " + str4);

            Console.WriteLine("str1 == str2: " + (str1 == str2));
            Console.WriteLine("str1 == str3: " + (str1 == str3));
            Console.WriteLine("str1 == str4: " + (str1 == str4));

            Console.WriteLine("str1.Equals(str4): " + str1.Equals(str4));//учитывается регистр
            Console.WriteLine("String.Compare(str1, str4, true): " + String.Compare(str1, str4, true));//игнорируем регист, т.к true; 0, если строки равны; -1, если первая строка меньше второй; 1, если первая строка больше второй

            Console.WriteLine("--------------------");

            //Три строки string
            string x1 = "hi";
            string x2 = "wow";
            string x3 = "C# is not C++";

            //Сцепление
            string conc = x1 + " " + x2;
            Console.WriteLine("Сцепление x1 и x2: " + conc);

            //Копирование
            string copy = String.Copy(x1);
            Console.WriteLine("Копирование x1: " + copy);

            //Выделение подстроки
            string substr = x3.Substring(3, 2);
            Console.WriteLine("Подстрока: " + substr);//индексация начинается с 0

            //Разделение строки на слова
            Console.WriteLine("Разделение строки на слова:");
            string[] words = x3.Split(' ');//word на каждой итерации цикла будет хранить очередной элемент массива words
            foreach (string word in words)
            {
                Console.WriteLine(word);
            }

            //Вставка подстроки в заданную позицию
            string insert = x1.Insert(2, ", World");
            Console.WriteLine("Вставка подстроки: " + insert);

            //Удаление подстроки
            string remove = x3.Remove(5, 4);
            Console.WriteLine("Удаление подстроки: " + remove);

            //Интерполирование строк
            int age = 18;
            string name = "Stas";
            string message = $"My name is {name} and I am {age} years old";
            Console.WriteLine($"Интерполирование строк: {message}");

            Console.WriteLine("--------------------");

            //Пустая и null строка
            string emptyStr = "";//строка, которая не содержит символов, но существует как объект типа string
            string nullStr = null;//строковая переменная, которая не ссылается на какой-либо объект в памяти
            Console.WriteLine($"Пустая строка: {emptyStr}");
            Console.WriteLine($"Null строка: {nullStr}");

            //Использование метода
            Console.WriteLine("Использование метода String.IsNullOrEmpty:");

            //Проверка пустой строки
            bool isEmpty = String.IsNullOrEmpty(emptyStr);
            Console.WriteLine($"emptyStr пустая: {isEmpty}");

            //Проверка null строки
            bool isNull = String.IsNullOrEmpty(nullStr);
            Console.WriteLine($"nullStr is null: {isNull}");

            //Проверка строки с содержанием
            string nonEmptyStr = "Hi, World";
            bool isNonEpty = String.IsNullOrEmpty(nonEmptyStr);
            Console.WriteLine($"nonEmptyStr пустая: {isNonEpty}");

            //Дополнительно 
            Console.WriteLine("Дополнительно:");

            //с пустой строкой
            int lengthStr = emptyStr.Length;
            Console.WriteLine($"Длина emptyStr: {lengthStr}");

            //c null строкой
            Console.WriteLine("Применение оператора объединения с null:");
            string company = "Google";
            string displayCompany = nullStr ?? company;//используется для предоставления значения по умолчанию в случае, если выражение с левой стороны равно null
            Console.WriteLine("nullStr: присвоили null");
            Console.WriteLine($"company: {company}");
            Console.WriteLine($"nullStr ?? company: {displayCompany}");

            //Использование StringBuilder
            Console.WriteLine("Использование StringBuilder:");

            //Создание объекта StringBuilder
            StringBuilder sb = new StringBuilder("Hello");//с начальной строкой

            //Добавление текста в конец
            sb.Append(" World");
            Console.WriteLine($"Добавление текста в конец: {sb.ToString()}");//Метод ToString возвращает текущее значение StringBuilder

            //Метод вставки
            sb.Insert(0, "AI: ");
            Console.WriteLine($"Метод вставки: {sb.ToString()}");

            //Удаление части строки
            sb.Remove(9, 6);
            Console.WriteLine($"Удаление части строки: {sb.ToString()}");

            //Замена текста
            sb.Replace("AI", "Google");
            Console.WriteLine($"Замена текста: {sb.ToString()}");

            Console.WriteLine("--------------------\n\nЗадание 3\n");

            //Создание и инициализация двумерного массива
            int rows = 4;
            int cols = 5;
            int[,] matrix = new int[rows, cols];

            //Заполнение массива значениями
            Random random = new Random();//Класс для генерации случайных чисел
            for (int y1 = 0; y1 < rows; y1++)
            {
                for (int y2 = 0; y2 < cols; y2++)
                {
                    matrix[y1, y2] = random.Next(1, 100);//метод экземпляра класса Random
                }
            }

            //Вывод массива в формате матрицы
            Console.WriteLine("Matrix:");
            for (int y1 = 0; y1 < rows; y1++)
            {
                for (int y2 = 0; y2 < cols; y2++)
                {
                    Console.Write($"{matrix[y1, y2], 3}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("--------------------");

            //Создание и инициализация массива строк
            string[] strArr = { "Ben", "Bob", "Billy", "Lory" };

            //Вывод массива
            Console.WriteLine("Вывод массива:");
            for(int y3 = 0; y3 < strArr.Length; y3++)
            {
                Console.Write($"Index {y3}: {strArr[y3]}\t");
            }

            //Длина массива
            Console.WriteLine($"\nДлина массива: {strArr.Length}");

            //Изменение массива

            int index = -1;//Инициализация переменной для индекса
            bool validIndex = false;//Для отслеживания корректности ввода

            while (!validIndex)
            {
                Console.WriteLine("Введите индекс элемента, который вы хотите изменить (0 - 3)");
                index = Convert.ToInt32(Console.ReadLine());

                if (index >= 0 && index < strArr.Length)
                {
                    validIndex = true;
                }
                else
                {
                    Console.WriteLine("Введите подходящий индекс");
                }

            }

            //Если индекс введен корректно, то идем дальше
            Console.WriteLine("Введите новое значение типа string:");
            string newValue = Console.ReadLine();
            strArr[index] = newValue;

            //Вывод обновленного массива
            Console.WriteLine("Обновленный массив: ");
            for (int y4 = 0; y4 < strArr.Length; y4++)
            {
                Console.Write($"Index {y4}: {strArr[y4]}\t");
            }

            Console.WriteLine("\n--------------------");

            //Ступенчатый массив(зубчатый)

            double[][] myArr = new double[3][];//Массив, который содержит 3 массива

            //Для каждого элемента массива нужно выделить оперативную память
            myArr[0] = new double[2];
            myArr[1] = new double[3];
            myArr[2] = new double[4];

            //Заполнение массива
            for (int i1 = 0; i1 < myArr.Length; i1++)
            {
                for (int j1 = 0; j1 < myArr[i1].Length; j1++)
                {
                    Console.WriteLine($"Введите элемент массива под индексом ({i1},{j1})");
                    myArr[i1][j1] = Convert.ToDouble(Console.ReadLine() );
                    
                }
            }

            //Вывод массива
            Console.WriteLine("\nВывод массива: ");
            for (int i1 = 0; i1 < myArr.Length; i1++)
            {
                for (int j1 = 0; j1 < myArr[i1].Length; j1++)
                {
                    Console.Write($"{myArr[i1][j1]}\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("--------------------");

            //Неявно типизированные переменные для хранения массива и строки.
            Console.WriteLine("Неявно типизированные переменные для хранения массива и строки:");

            var numArr = new[] { 1, 2, 3, 4, 5 };//компилятор на основе переданных значений автоматически выводит тип элементов массива

            var message01 = "Hi, World";

            //Вывод массива 
            Console.WriteLine("Массив: ");

            foreach (var num in numArr)
            {
                Console.WriteLine(num);
            }

            //Вывод строки
            Console.WriteLine($"Строка: {message01}");

            Console.WriteLine("--------------------\n\nЗадание 4\n");

            //Кортеж из 5 элементов
            Console.WriteLine("Кортеж из 5 элементов:");
            var tuple = (23, "example1", 'A', "example2", 123456789UL);

            Console.WriteLine("Вывод всего кортежа: ");
            Console.WriteLine($"Integer: {tuple.Item1}");    
            Console.WriteLine($"First String: {tuple.Item2}"); 
            Console.WriteLine($"Char: {tuple.Item3}");        
            Console.WriteLine($"Second String: {tuple.Item4}"); 
            Console.WriteLine($"ULong: {tuple.Item5}");

            Console.WriteLine($"Другой способ: \n{tuple}");

            Console.WriteLine("Вывод выборочных лементов кортежа:");
            Console.WriteLine($"Integer: {tuple.Item1}");
            Console.WriteLine($"Char: {tuple.Item3}");
            Console.WriteLine($"Second String: {tuple.Item4}");

            Console.WriteLine("Распаковвка кортежа:");//Позволяет извлечь элементы кортежа и присвоить их отдельным переменным

            var (number, description1, initial, description2, largeNum) = tuple;

            Console.WriteLine($"number: {number}");
            Console.WriteLine($"description1: {description1}");
            Console.WriteLine($"initial: {initial}");
            Console.WriteLine($"description2: {description2}");
            Console.WriteLine($"largeNum: {largeNum}");

            Console.WriteLine("Распаковка с использованием переменных ( ):");
            var (number1, _, initial1, _, _) = tuple;//_ используется для игнорирования элементов кортежа
            Console.WriteLine($"number: {number1}");
            Console.WriteLine($"initial: {initial1}");

            //Распаковка в методе
            Console.WriteLine("Распаковка кортежа в методе:");
            var (number2, description01, initial2, description02, largeNum2) = GetTuple();

            Console.WriteLine($"number: {number2}");
            Console.WriteLine($"description1: {description01}");
            Console.WriteLine($"initial: {initial2}");
            Console.WriteLine($"description2: {description02}");
            Console.WriteLine($"largeNum: {largeNum2}");

            Console.WriteLine("--------------------\n\nЗадание 5\n");

            // Сравнение кортежей
            Console.WriteLine("Сравнение кортежей:");
            var tuple1 = (42, "example", 'A', 3.14, 12345678901234567890UL);
            var tuple2 = (42, "example", 'A', 3.14, 12345678901234567890UL);
            var tuple3 = (10, "test", 'B', 2.71, 9876543210UL);

            bool areEqual1 = tuple1 == tuple2;
            bool areEqual2 = tuple1 == tuple3; 
            bool areNotEqual = tuple1 != tuple3; 

            // Вывод результатов
            Console.WriteLine($"tuple1 == tuple2: {areEqual1}"); // True
            Console.WriteLine($"tuple1 == tuple3: {areEqual2}"); // False
            Console.WriteLine($"tuple1 != tuple3: {areNotEqual}"); // True

            Console.WriteLine("--------------------");

            //массива целых чисел
            int[] numbers = { 10, 20, 30, 5, 15 };

            string exampleString = "Hello";

            // Вызов локальной функции
            var result = ProcessArrayAndString(numbers, exampleString);//Функция возвращает кортеж, который сохраняется в переменной result

            // Вывод результатов
            Console.WriteLine($"Max: {result.max}, Min: {result.min}, Sum: {result.sum}, First letter: {result.firstChar}");

            // Локальная функция, выполняющая операции
            (int max, int min, int sum, char firstChar) ProcessArrayAndString(int[] array, string str) //В скобках указывается, что возвращается кортеж с четырьмя элементами.
            {
                //устанавливаем начальное значение
                int max = array[0];
                int min = array[0];
                int sum = 0;

                // Поиск максимального, минимального элементов и подсчет суммы
                foreach (int num in array)
                {
                    if (num > max) max = num;
                    if (num < min) min = num;
                    sum += num;
                }

                // Получение первой буквы строки
                char firstChar = str.Length > 0 ? str[0] : '\0';// \0 является символом null, это означает, что строка пустая

                // Возвращение кортежа с результатами
                return (max, min, sum, firstChar);
            }

            Console.WriteLine("--------------------\n\nЗадание 6\n");
            //checked и unchecked позволяют разработчикам решать, нужно ли проверять переполнение чисел при арифметических операциях.

            // Локальная функция с блоком checked
            void Checked()
            {
                //код, который может выбросить исключение.
                try
                {
                    // Блок checked проверяет переполнение
                    checked
                    {
                        int maxInt = int.MaxValue; // Максимальное значение int
                        Console.WriteLine($"Максимальное значение int: {maxInt}");

                        // Переполнение
                        maxInt++;
                        Console.WriteLine($"После прибавления 1 в checked block: {maxInt}");
                    }
                }
                //Еcли произошло исключение, то оно перехватывается блоком catch
                catch (OverflowException ex)//специализированный тип исключения в .NET, OverflowException наследуется от класса System.Exception
                {
                    //это свойство объекта OverflowException, которое содержит текстовое описание ошибки. Оно предоставляет более детальную информацию о причинах возникновения исключения
                    Console.WriteLine("Обнаружено переполнение в checked block: " + ex.Message);
                }
            }

            // Локальная функция с блоком unchecked
            void Unchecked()
            {
                // Блок unchecked игнорирует переполнение
                unchecked
                {
                    int maxInt = int.MaxValue; // Максимальное значение int
                    Console.WriteLine($"Максимальное значение int в unchecked block: {maxInt}");

                    // Переполнение
                    maxInt++;
                    Console.WriteLine($"После переполнения в unchecked block: {maxInt}");
                }
            }

            // Вызов функций
            Checked();
            Unchecked();

            Console.ReadKey();
        }

        //Метод, возвращающий кортеж
        static (int, string, char, string, ulong) GetTuple()
        {
            return (23, "example1", 'A', "example2", 123456789UL);
        }
    }

}
