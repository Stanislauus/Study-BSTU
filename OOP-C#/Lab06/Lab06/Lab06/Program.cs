﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Lab06
{
    public interface IShowInfo
    {
        void ShowInfo();
    }
    public class Director
    {

        public string FullName { get; set; }
        public int Experience { get; set; }

        public Director(string fullName, int experience)
        {
            FullName = fullName;
            Experience = experience;
        }

        public override string ToString()
        {
            return $"Режиссер: {FullName}, Стаж работы: {Experience} лет";
        }
        public override bool Equals(object obj)
        {
            if (obj is Director other)
            {
                return FullName == other.FullName && Experience == other.Experience;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return FullName.GetHashCode() * Experience.GetHashCode();
        }
    }

    public abstract partial class TVProgram : IShowInfo
    {


        public abstract void ShowInfo();

        public virtual void ShowBaseInfo()
        {
            Console.WriteLine("Метод, который может быть переопределен. ShowBaseInfo()");
        }

        /*public override string ToString() //Для лабы 4
        {
            //происходит неявный вызов метода ToString() для объекта Director.
            return $"Тип: {GetType().Name}, Название: {Title}, Длительность: {Duration} мин, Режиссер: {Director}";
        }*/



        
        public override string ToString()
        {
            return $"Тип {Type}, Название: {Title}, Длительность: {Duration} мин, {Director}, {Schedule}.";
        }
        



        public override bool Equals(object obj)
        {
            if (obj is TVProgram other)
            {
                return Title == other.Title && Duration == other.Duration && Director.Equals(other.Director);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode() * Duration.GetHashCode() * Director.GetHashCode();
        }

    }

    public class Movie : TVProgram
    {
        public Movie(string title, int duration, Director director, Schedule schedule, Type type)
            : base(title, duration, director, schedule, type)//передает параметры в базовый класс
        { }
        public override void ShowInfo()
        {
            Console.WriteLine($"Фильм: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    public class FeatureFilm : Movie
    {
        //создается иерархия TVProgram -> Movie -> FeatureFilm
        public FeatureFilm(string title, int duration, Director director, Schedule schedule, Type type)
            : base(title, duration, director, schedule, type) { }
        public override void ShowInfo()
        {
            Console.WriteLine($"Худ. фильм: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    public sealed class Cartoon : Movie //Cartoon нельзя наследовать
    {
        public Cartoon(string title, int duration, Director director, Schedule schedule, Type type)
            : base(title, duration, director, schedule, type) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"Мультфильм: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    public class News : TVProgram
    {
        public News(string title, int duration, Director director, Schedule schedule, Type type)
            : base(title, duration, director, schedule, type) { }
        public override void ShowInfo()
        {
            Console.WriteLine($"Новости: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    public class AD : TVProgram
    {
        public AD(string title, int duration, Director director, Schedule schedule, Type type)
            : base(title, duration, director, schedule, type) { }
        public override void ShowInfo()
        {
            Console.WriteLine($"Реклама: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }


    public class Printer
    {
        public void IAmPrinting(TVProgram someobj)
        {
            Console.WriteLine(someobj.ToString());
        }
    }







    public enum Type
    {
        Movie,
        FeatureFilm,
        Cartoon,
        News,
        AD
    }

    public struct Schedule
    {
        public string ChannelName { get; set; }
        public string Time { get; set; }

        public Schedule(string channelName, string time)
        {
            ChannelName = channelName;
            Time = time;
        }

        public override string ToString()
        {
            return $"Канал: {ChannelName}, Время показа: {Time}";
        }
    }

    

    public class TVProgramContainer
    {
        private List<TVProgram> _programs;

        public TVProgramContainer()
        {
            //исп. для инициализации списка при создании объекта TVProgramContainer()
            _programs = new List<TVProgram>();
        }

        //метод для добавления передачи в контейнер
        public void AddProgram(TVProgram program)
        {

            //-----------------1)-----------------

            if (program.Duration <= 0)
            {
                throw new InvalidTVProgramDuration(program.Duration);
            }
            if (program.Director == null)
            {
                throw new TVProgramDirectorNotFound();
            }
            _programs.Add(program);

            //------------------------------------
        }

        //удаление передачи из контейнера по индексу
        public void RemoveProgram(int index)
        {
            if (index >= 0 && index < _programs.Count)
            {
                _programs.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Некорректный индекс.");
            }
        }

        //получение передачи по индексу
        public TVProgram GetProgram(int index)
        {
            if (index >= 0 && index < _programs.Count)
            {
                return _programs[index];
            }
            else
            {
                //-------------------1)------------------

                throw new TVProgramNotFoundIndex(index);
                /*Console.WriteLine("Некорректный индекс.");
                return null;*/
                //---------------------------------------
            }
        }

        //метод для установки передачи по индексу
        public void SetProgram(int index, TVProgram program)
        {
            if (index >= 0 && index < _programs.Count)
            {
                _programs[index] = program;
            }
            else
            {
                Console.WriteLine("Некорректный индекс.");
            }
        }

        //вывод всех передач в контейнере
        public void PrintAllPrograms()
        {
            foreach (var program in _programs)
            {
                Console.WriteLine(program.ToString());

            }
        }



     
        public List<TVProgram> GetAllPrograms()
        {
            return _programs;
        }
        



    }

    

    //класс, предназначенный для управления программами в TVProgramContainer
    public class TVProgramController
    {
        private TVProgramContainer _container;

        // принцип инъекции зависимости
        //можно использовать разные реализации контейнеров без необходимости менять сам контроллер
        public TVProgramController(TVProgramContainer container)
        {
            _container = container;
        }

        //для поиска всех фильмов, снятых в определенный год
        public List<TVProgram> GetMoviesByYear(int year)
        {
            List<TVProgram> moviesInYear = new List<TVProgram>();

            foreach (var program in _container.GetAllPrograms())
            {
                if (program is Movie && program.Schedule.ChannelName.Contains(year.ToString()))
                {
                    moviesInYear.Add(program);
                }
            }

            return moviesInYear;
        }

        //для подсчета общей продолжительности программ
        public int GetTotalDuration()
        {
            int totalDuration = 0;

            foreach (var program in _container.GetAllPrograms())
            {
                totalDuration += program.Duration;
            }
            return totalDuration;
        }

        //для подсчета количества рекламных роликов
        public int CountAD()
        {
            int count = 0;

            foreach (var program in _container.GetAllPrograms())
            {
                if (program is AD)
                {
                    count++;
                }
            }
            return count;
        }
    }



    //-----------------------------1)--------------------


    //базовый класс для всех исключений
    public class TVProgramException : Exception 
    {
        public TVProgramException(string message) : base(message) { }
    }

    //при попытке получить программу по индексу
    public class TVProgramNotFoundIndex : TVProgramException 
    {
        public int Index {  get; }

        public TVProgramNotFoundIndex(int index) 
            : base($"Программа с индексом {index} не найдена в контейнере.")
        {
            Index = index;  
        }
    } 

    //при добавлении программы с неверной длительностью
    public class InvalidTVProgramDuration : TVProgramException
    {
        public int Duration { get; }

        public InvalidTVProgramDuration(int duration)
            : base($"Некорректная длительность программы: {duration} минут. Длительность должна быть положительным значением.") 
        {
            Duration = duration;
        }
    }

    //возникает, если поле Director не задано
    public class TVProgramDirectorNotFound : TVProgramException
    {
        public TVProgramDirectorNotFound() 
            :base("Режиссер не назначен для данной программы.")
        { }
    }

    //---------------------------------------------------

    //---------------------2)----------------------------

    //исключение для деления на ноль
    //выбрасывается в методе SimulateDivision
    public class DivisionByZeroException : TVProgramException
    {
        public DivisionByZeroException() : base("Ошибка: деление на ноль.") { }
    }

    //если добавляется null-программа
    public class NullTVProgramException : TVProgramException
    {
        public NullTVProgramException() : base("Ошибка: попытка добавить null-передачу в контейнер.") { }
    }

    //исключение для нехватки памяти (симуляция переполнения памяти)
    //выбрасывается в методе SimulateMemory
    public class MemoryOverflowException : TVProgramException
    {
        public MemoryOverflowException() : base("Ошибка: недостаточно памяти для обработки.") { }
    }

    //исключение для проблем с файлами (например, для будущего функционала работы с файлами)
    public class TVProgramFileException : TVProgramException
    {
        public TVProgramFileException() : base("Ошибка: проблема при работе с файлом.") { }
    }

    //---------------------------------------------------

    //-----------------------------5)--------------------
    public class Task5
    {
        public static void Method1()
        {
            try
            {
                Method2();
            }
            catch (InsufficientMemoryException ex)
            {
                Console.WriteLine($"Catch в Method1: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Выполнен finally для Method1.");
            }
        }

        public static void Method2()
        {
            try
            {
                int x1 = 5;
                int x2 = x1 / 0;
            }
            finally
            {
                Console.WriteLine("Выполнен finally для Method2.");
            }
        }
    }
    




    internal class Program
    {
        //-----------------------2)-------------------

        public int SimulateDivision(int divisor)
        {
            if (divisor == 0)
            {
                throw new DivisionByZeroException();
            }
            return 100 / divisor;
        }

        public void SimulateMemory()
        {
            throw new MemoryOverflowException();
        }

        //--------------------------------------------

        //-----------------------7)-------------------

        public static double CalcAverage(int sum, int count)
        {
            Debug.Assert(count > 0, "Количество элементов должно быть больше нуля");

            // Используем Debugger для остановки выполнения и анализа
            
            /*if (count == 0)
            {
                // Используем Debugger для остановки выполнения и анализа
                Debugger.Break();
            }*/
            return (double)sum / count;
        }

        //--------------------------------------------

        static void Main(string[] args)
        {
            try
            {
                Director director = new Director("Bob", 12);
                Console.WriteLine(director.ToString() + "\n");

                //работа с объектами через ссылки на абстрактные классы и интерфейсы
                TVProgram movie = new Movie("Seven", 127, director, new Schedule("First2023", "12:00"), Type.Movie);
                movie.ShowInfo();
                Console.WriteLine(movie.ToString() + "\n");

                TVProgram featureFilm = new FeatureFilm("Interstellar", 169, new Director("Tom", 25), new Schedule("Second", "09:00"), Type.FeatureFilm);
                featureFilm.ShowInfo();
                Console.WriteLine(featureFilm.ToString() + "\n");

                TVProgram cartoon = new Cartoon("Shrek", 89, new Director("Jack", 18), new Schedule("Third", "14:00"), Type.Cartoon);
                cartoon.ShowInfo();
                Console.WriteLine(cartoon.ToString() + "\n");

                TVProgram news = new News("EN", 40, new Director("Steve", 10), new Schedule("Fourth", "16:30"), Type.News);
                news.ShowInfo();
                Console.WriteLine(news.ToString() + "\n");

                TVProgram ad = new AD("Good prices", 5, new Director("Anna", 5), new Schedule("Fifth", "19:00"), Type.AD);
                ad.ShowInfo();
                Console.WriteLine(ad.ToString() + "\n");


                Console.WriteLine("\n---------------------5)------------------\n");

                // Создан массив, позволяет работать с объектами через ссылки на абстрактный класс
                TVProgram[] programs = { movie, featureFilm, cartoon, news, ad };


                foreach (var program in programs)
                {
                    program.ShowInfo();
                }

                foreach (var program in programs)
                {
                    /*if (program is Movie)
                    {
                        Console.WriteLine($"{program.Title} - это фильм.");
                    }*/
                    if (program is FeatureFilm)
                    {
                        Console.WriteLine($"{program.Title} - это худ. фильм.");
                    }
                    else if (program is News)
                    {
                        Console.WriteLine($"{program.Title} - это новости.");
                    }
                    else if (program is AD)
                    {
                        Console.WriteLine($"{program.Title} - это реклама.");
                    }
                    else if (program is Cartoon)
                    {
                        Console.WriteLine($"{program.Title} - это мультфильм.");
                    }
                }


                Printer printer = new Printer();

                Console.WriteLine("\n---------------------7)------------------\n");

                foreach (var program in programs)
                {
                    printer.IAmPrinting(program);
                }


                Console.WriteLine("\n-----------Задание 3(lab05)---------\n");

                TVProgramContainer container = new TVProgramContainer();

                container.AddProgram(featureFilm);
                container.AddProgram(movie);
                container.AddProgram(ad);

                Console.WriteLine("Все передачи:");
                container.PrintAllPrograms();

                //Get и Set
                var receivedProgram = container.GetProgram(0);
                Console.WriteLine($"\nПолученная передача: {receivedProgram}");

                TVProgram newMovie = new Movie("Nobody", 92, director, new Schedule("Sixth", "21:00"), Type.Movie);
                container.SetProgram(0, newMovie);
                Console.WriteLine("\nПосле изменения:");
                container.PrintAllPrograms();

                container.RemoveProgram(0);
                Console.WriteLine("\nВсе передачи после удаления:");
                container.PrintAllPrograms();

                //создание контроллера и выполнение запросов
                TVProgramController controller = new TVProgramController(container);

                //найти фильмы 2023 года
                Console.WriteLine("\nФильмы, снятые в 2023 году:");
                var movies2023 = controller.GetMoviesByYear(2023);
                foreach (var movieX in movies2023)
                {
                    Console.WriteLine(movieX);
                }

                //общая длительность программ
                int totalDuration = controller.GetTotalDuration();
                Console.WriteLine($"\nОбщая длительность программ: {totalDuration} мин.");

                //количество рекламных роликов
                int adCount = controller.CountAD();
                Console.WriteLine($"\nКоличество рекламных роликов: {adCount}");


                //----------------------4)---------------


                Console.WriteLine("\n-------------Задание 4(Lab06)----------\n");




                //пытаемся получить программу по некорректному индексу
                try
                {
                    var invalidProgram = container.GetProgram(10);
                }
                catch (TVProgramNotFoundIndex ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine($"Метод, вызвавший исключение: {ex.TargetSite}");
                    Console.WriteLine($"Точное местоположение в коде: {ex.StackTrace}\n");
                }

                //когда передача имеет отрицательную или нулевую продолжительность
                try
                {
                    TVProgram invalidProgram = new Movie("Movie", -1, director, new Schedule("Channel", "12:00"), Type.Movie);
                    container.AddProgram(invalidProgram);
                }
                catch (InvalidTVProgramDuration ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine($"Метод, вызвавший исключение: {ex.TargetSite}");
                    Console.WriteLine($"Точное местоположение в коде: {ex.StackTrace}\n");
                }

                //если у передачи отсутствует режиссёр (поле Director равно null)
                try
                {
                    TVProgram noDirectorProgram = new Movie("Movie2", 110, null, new Schedule("Channel", "10:00"), Type.Movie);
                    container.AddProgram(noDirectorProgram);
                }
                catch (TVProgramDirectorNotFound ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine($"Метод, вызвавший исключение: {ex.TargetSite}");
                    Console.WriteLine($"Точное местоположение в коде: {ex.StackTrace}\n");
                }


                Program programObj = new Program();


                //исключение деления на ноль, вызывается в SimulateDivision(int divisor)
                try
                {
                    int result = programObj.SimulateDivision(0);
                }
                catch (DivisionByZeroException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine($"Метод, вызвавший исключение: {ex.TargetSite}");
                    Console.WriteLine($"Точное местоположение в коде: {ex.StackTrace}");
                }
                finally
                {
                    Console.WriteLine("Операция завершена.\n");
                }

                //исключение симулирует нехватку памяти (с помощью SimulateMemory())
                try
                {
                    programObj.SimulateMemory();
                }
                catch (MemoryOverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine($"Метод, вызвавший исключение: {ex.TargetSite}");
                    Console.WriteLine($"Точное местоположение в коде: {ex.StackTrace}");
                }
                finally
                {
                    Console.WriteLine("Проверка на переполнение памяти завершена.\n");
                }

                //------------------------5)------------------------------

                Console.WriteLine("\n-------------Задание 5(Lab06)----------\n");

                try
                {
                    Task5.Method1();
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine($"Catch в Main: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine("Выполнен finally для Main.");
                }

                //--------------------------------------------------------

                //------------------------7)------------------------------

                Console.WriteLine("\n-------------Задание 5(Lab06)----------\n");

                Debug.Write("Отладочное сообщение без новой строки");

                double trueCalc = Program.CalcAverage(12, 4);
                Console.WriteLine($"Среднее арифметическое trueCalc = {trueCalc}");

                double falseCalc = Program.CalcAverage(12, 0);
                Console.WriteLine($"Среднее арифметическое falseCalc = {falseCalc}");

                //--------------------------------------------------------


            }
            finally
            {
                Console.WriteLine("Программа завершена.");
            }

            Console.ReadKey();
        }
    }
}