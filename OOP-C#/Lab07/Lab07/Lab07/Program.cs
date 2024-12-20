using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab07
{
    //обобщенный интерфейс 
    public interface IGeneralized<T>
    {
        void Add(T value);  
        void Remove(T value);   
        List<T> GetAll();

    }

    //реализация интерфейса с List<T>
    //обобщенный класс, реализующий интерфейс IGeneralized<T>
    public class CollectionType<T> : IGeneralized<T> where T : struct
    {
        private List<T> _list = new List<T>();

        //Используем методы List<T>
        public void Add(T value)
        {
            try
            {
                _list.Add(value);
                Console.WriteLine($"Элемент {value} добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении элеметна: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Метод Add завершил работу.");
            }
        }

        public void Remove(T value)
        {
            try
            {
                if (_list.Contains(value))
                {
                    _list.Remove(value);
                    Console.WriteLine($"Элемент {value} удалён.");
                }
                else
                {
                    Console.WriteLine($"Элемент {value} не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении элемента: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Метод Remove завершён.");
            }
        }

        public List<T> GetAll()
        {
            return _list;
        }

        //Predicate<T> — представляет метод, возвращающий логическое значение (true или false) для заданного объекта типа T
        public T Find(Predicate<T> predicate)
        {
            return _list.Find(predicate);//метод класса List<T>
        }

        
    }


    // Класс Set из Lab03, использующий CollectionType<T>
    public class Set
    {
        // Использование обобщённого типа CollectionType<int>
        private CollectionType<int> elements = new CollectionType<int>();

        //создание множества
        public Set(params int[] elements)
        {
            foreach (var item in elements)
            {
                this.elements.Add(item);
            }
        }

        // Методы для работы с множеством
        public void Add(int element)
        {
            elements.Add(element);
        }

        public void Remove(int element)
        {
            elements.Remove(element);
        }

        public void Display()
        {
            Console.WriteLine("Список целых чисел:");
            //вызывается для каждого элемента коллекции
            foreach (var item in elements.GetAll())
            {
                Console.WriteLine(item);
            }
        }
    }

    //-------------------------------(Lab04)---------------------

    public class TVProgram
    {
        public string Title { get; set; }
        public int Duration { get; set; } 

        public TVProgram(string title, int duration)
        {
            Title = title;
            Duration = duration;
        }

        public override string ToString()
        {
            return $"{Title}, {Duration}";
        }

        //----------------------------5)------------------------

        //метод для преобразования строки в объект TVProgram
        public static TVProgram Parse(string data)
        {
            var parts = data.Split(',');    
            string title = parts[0].Trim();
            int duration = int.Parse(parts[1].Trim());  
            return new TVProgram(title, duration);
        }

        //------------------------------------------------------
    }

    //-----------------------------------------------------------

    public class CollectionTypeClass<T> : IGeneralized<T> where T : class
    {
        private List<T> _list = new List<T>();

        public void Add(T value)
        {
            try
            {
                _list.Add(value);
                Console.WriteLine($"Элемент {value} добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении элемента: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Метод Add завершил работу.");
            }
        }

        public void Remove(T value)
        {
            try
            {
                if (_list.Contains(value))
                {
                    _list.Remove(value);
                    Console.WriteLine($"Элемент {value} удалён.");
                }
                else
                {
                    Console.WriteLine($"Элемент {value} не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении элемента: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Метод Remove завершён.");
            }
        }

        public List<T> GetAll()
        {
            return _list;
        }

        public T Find(Predicate<T> predicate)
        {
            return _list.Find(predicate);
        }

        //-----------------------------5)---------------------------
        
        public void SaveToFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var item in _list)
                    {
                        writer.WriteLine(item.ToString());
                    }
                }
                Console.WriteLine("Коллекция успешно сохранена в файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Метод SaveFile завершил работу.");
            }
        }

        public void LoadFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл не найден.");
                    return;
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    _list.Clear();
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (typeof(T).GetMethod("Parse") != null)
                        {
                            //используем рефлексию для вызова метода Parse
                            var parseMethod = typeof(T).GetMethod("Parse", new[] { typeof(string) });

                            if (parseMethod != null)
                            {
                                try
                                {
                                    //вызов метода Parse для строки line
                                    T value = (T)parseMethod.Invoke(null, new object[] { line });

                                    //добавляем объект в коллекцию
                                    _list.Add(value);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Ошибка при преобразовании строки {line} в тип {typeof(T)}: {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Тип {typeof(T)} не поддерживает метод Parse.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Тип {typeof(T)} не поддерживает преобразование строки.");
                        }
                    }

                }

                Console.WriteLine("Коллекция успешно загружена из файла.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке из файла: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Метод LoadFromFile завершил работу.");
            }
        }
        

        //------------------------------------------------------------
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            //создаём коллекцию, работающую с целыми числами
            CollectionType<int> collection = new CollectionType<int>();

            collection.Add(1);
            collection.Add(2);
            collection.Add(3);
            

            Console.WriteLine("Список целых чисел:");
            //вызывается для каждого элемента коллекции
            foreach (var item in collection.GetAll())
            {
                Console.WriteLine(item);
            }

            //напрямую вывести объект коллекции List<int> (Console.WriteLine просто вызовет метод ToString() у объекта.)
            //Console.WriteLine(collection.GetAll());

            collection.Remove(2);

            //определяем предикат для поиска четного числа
            Predicate<int> pr = x => x % 2 == 0;

            //используем метод Find для поиска первого четного числа
            int prFirst = collection.Find(pr);

            Console.WriteLine($"Первое четное число: {prFirst}");

            // Пример работы с классом Set
            Set set = new Set(1, 2, 3, 4, 5);
            Console.WriteLine("\nЭлементы множества:");
            set.Display();
            set.Add(6);
            set.Remove(3);
            set.Display();


            //--------------------------------------------4)----------------------------

            //использовнаие обощения с пользовательским классом

            CollectionTypeClass<TVProgram> tvPrograms = new CollectionTypeClass<TVProgram>();

            tvPrograms.Add(new TVProgram("Новости", 30));
            tvPrograms.Add(new TVProgram("Фильм", 120));
            tvPrograms.Add(new TVProgram("Мультфильм", 15));

            Console.WriteLine("\nСписок телепрограмм:");
            foreach (var program in tvPrograms.GetAll())
            {
                Console.WriteLine(program);
            }

            tvPrograms.Remove(new TVProgram("Фильм", 120));

            // Использование предиката для поиска телепрограммы длительностью более 20 минут
            Predicate<TVProgram> longProgram = x => x.Duration > 20;
            TVProgram foundProgram = tvPrograms.Find(longProgram);

            Console.WriteLine($"Первая телепрограмма длительностью более 20 минут: {foundProgram}");

            //--------------------------------------------------------------------------

            //--------------------------------------------5)----------------------------

            //сохранение коллекции в файл
            tvPrograms.SaveToFile("tvPrograms.txt");

            tvPrograms.LoadFromFile("tvPrograms.txt");



            Console.ReadKey();  
        }
    }
}
