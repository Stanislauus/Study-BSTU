using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04
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

    public abstract class TVProgram : IShowInfo
    {
        public string Title { get; set; }
        public int Duration { get; set; }

        //private Director _director; 
        public Director Director { get; set; }//композиция

        public TVProgram(string title, int duration, Director director)
        {
            Title = title;
            Duration = duration;

            Director = director;//композиция
        }

        public abstract void ShowInfo();

        public virtual void ShowBaseInfo()
        {
            Console.WriteLine("Метод, который может быть переопределен. ShowBaseInfo()");
        }

        public override string ToString()
        {
            //происходит неявный вызов метода ToString() для объекта Director.
            return $"Тип: {GetType().Name}, Название: {Title}, Длительность: {Duration} мин, Режиссер: {Director}";
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
        public Movie(string title, int duration, Director director) 
            : base(title, duration, director)//передает параметры в базовый класс
        {}
        public override void ShowInfo()
        {
            Console.WriteLine($"Фильм: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    public class FeatureFilm : Movie
    {
        //создается иерархия TVProgram -> Movie -> FeatureFilm
        public FeatureFilm(string title, int duration, Director director)
            : base(title, duration, director) {}
        public override void ShowInfo()
        {
            Console.WriteLine($"Худ. фильм: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    public sealed class Cartoon : Movie //Cartoon нельзя наследовать
    {
        public Cartoon(string title, int duration, Director director)
            : base(title, duration, director) {}

        public override void ShowInfo()
        {
            Console.WriteLine($"Мультфильм: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    public class News : TVProgram
    {
        public News(string title, int duration, Director director)
            : base(title, duration, director) {}
        public override void ShowInfo()
        {
            Console.WriteLine($"Новости: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    public class AD : TVProgram
    {
        public AD(string title, int duration, Director director)
            : base(title, duration, director) {}
        public override void ShowInfo()
        {
            Console.WriteLine($"Реклама: {Title} ({Duration} мин), Режиссер: {Director.FullName}");
        }
    }

    //------------------------------7)--------------------------------------
    public class Printer
    {
        public void IAmPrinting(TVProgram someobj)
        {
            Console.WriteLine(someobj.ToString());
        }
    }
    //----------------------------------------------------------------------

    internal class Program
    {
        static void Main(string[] args)
        {
            Director director = new Director("Bob", 12);
            Console.WriteLine(director.ToString() + "\n");

            //работа с объектами через ссылки на абстрактные классы и интерфейсы
            TVProgram movie = new Movie("Seven", 127, director);
            movie.ShowInfo();
            Console.WriteLine(movie.ToString() + "\n");

            TVProgram featureFilm = new FeatureFilm("Interstellar", 169, new Director("Tom", 25));
            featureFilm.ShowInfo();
            Console.WriteLine(featureFilm.ToString() + "\n");

            TVProgram cartoon = new Cartoon("Shrek", 89, new Director("Jack", 18));
            cartoon.ShowInfo();

            TVProgram news = new News("EN", 40, new Director("Steve", 10));
            news.ShowInfo();
            
            TVProgram ad = new AD("Good prices", 5, new Director("Anna", 5));
            ad.ShowInfo();

            Console.WriteLine("\n---------------------5)------------------\n");

            // Создан массив, позволяет работать с объектами через ссылки на абстрактный класс
            TVProgram[] programs = {movie, featureFilm, cartoon, news, ad};


            //-----------5)--------
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

            //-----------7)--------
            Printer printer = new Printer();

            Console.WriteLine("\n---------------------7)------------------\n");

            foreach (var program in programs)
            {
                printer.IAmPrinting(program);   
            }

            Console.ReadKey();
            

        }
    }
}
