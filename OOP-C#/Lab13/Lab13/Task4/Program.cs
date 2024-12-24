using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Task4
{
    public interface IShowInfo
    {
        void ShowInfo();
    }

    
    public abstract class TVProgram : IShowInfo
    {
        public string Title { get; set; }
        public int Duration { get; set; }

        public TVProgram() { } // Пустой конструктор для XML-сериализации

        public TVProgram(string title, int duration)
        {
            Title = title;
            Duration = duration;
        }

        public abstract void ShowInfo();

        public override string ToString()
        {
            //происходит неявный вызов метода ToString() для объекта Director.
            return $"Тип: {GetType().Name}, Название: {Title}, Длительность: {Duration} мин";
        }

    }

    
    public class Movie : TVProgram
    {
        public Movie() { }
        public Movie(string title, int duration)
            : base(title, duration)//передает параметры в базовый класс
        { }
        public override void ShowInfo()
        {
            Console.WriteLine($"Фильм: {Title} ({Duration} мин)");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем коллекцию объектов
            List<Movie> movies = new List<Movie>
            {
                new Movie("Movie 1", 120),
                new Movie("Movie 2", 150),
                new Movie("Movie 3", 90)
            };

            // Сериализация коллекции в JSON
            string json = JsonConvert.SerializeObject(movies, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("movies_linq.json", json);

            Console.WriteLine("JSON-файл создан: movies_linq.json");

            // Чтение JSON и создание объекта JArray для работы с LINQ
            JArray moviesArray = JArray.Parse(File.ReadAllText("movies_linq.json"));

            // Запрос 1: Вывод всех фильмов
            Console.WriteLine("\nLINQ to JSON: Все фильмы");
            foreach (var movie in moviesArray)
            {
                Console.WriteLine($"Название: {movie["Title"]}, Длительность: {movie["Duration"]} мин");
            }

            // Запрос 2: Выбор фильма с длительностью более 100 минут
            Console.WriteLine("\nLINQ to JSON: Фильмы с длительностью более 100 минут");
            var longMovies = moviesArray
                .Where(m => (int)m["Duration"] > 100)
                .Select(m => m["Title"]);

            foreach (var title in longMovies)
            {
                Console.WriteLine($"Название: {title}");
            }

            // Запрос 3: Средняя длительность фильмов
            Console.WriteLine("\nLINQ to JSON: Средняя длительность фильмов");
            double averageDuration = moviesArray.Average(m => (int)m["Duration"]);
            Console.WriteLine($"Средняя длительность: {averageDuration} мин");

            Console.ReadKey();
        }
    }
}
