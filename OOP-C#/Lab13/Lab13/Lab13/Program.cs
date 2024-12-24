using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json;
using System.Xml;
using System.Xml.XPath;
using System.Text.Json.Serialization;

namespace Lab13
{
    public interface ISerializer
    {
        void Serialize<T>(T obj, string filePath);
        T Deserialize<T>(string filePath);
    }



    public interface IShowInfo
    {
        void ShowInfo();
    }

    [Serializable]
    public abstract class TVProgram : IShowInfo
    {
        [XmlIgnore] //[JsonIgnore] //[SoapIgnore] //[NonSerialized]
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

    [Serializable]
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

    //---------------------------------

    // Реализация для Binary
    public class BinarySerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(stream, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    // Реализация для SOAP
    public class SoapSerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(stream, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    // Реализация для JSON
    public class JsonSerializerWrapper : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            File.WriteAllText(filePath, jsonString);
        }

        public T Deserialize<T>(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }

    // Реализация для XML
    public class XmlSerializerWrapper : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream);
            }
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

            // Печать информации о каждом фильме
            foreach (var movie in movies)
            {
                movie.ShowInfo();
            }

            //для работы с SOAP
            Movie[] movieArray = movies.ToArray();

            Console.WriteLine("\nСериализация и десериализация коллекции фильмов:");

            // Сериализация коллекции в файл
            ISerializer binarySerializer = new BinarySerializer();
            binarySerializer.Serialize(movies, "movies.bin");

            // Десериализация коллекции из файла
            List<Movie> binaryDeserializedMovies = binarySerializer.Deserialize<List<Movie>>("movies.bin");
            Console.WriteLine("Binary Deserialized:");
            foreach (var movie in binaryDeserializedMovies)
            {
                Console.WriteLine(movie);
            }

            // SOAP сериализация и десериализация
            ISerializer soapSerializer = new SoapSerializer();
            soapSerializer.Serialize(movieArray, "movies.soap");
            Movie[] soapDeserializedMovies = soapSerializer.Deserialize<Movie[]>("movies.soap");
            Console.WriteLine("\nSOAP Deserialized:");
            foreach (var movie in soapDeserializedMovies)
            {
                Console.WriteLine(movie);
            }

            // JSON сериализация и десериализация
            ISerializer jsonSerializer = new JsonSerializerWrapper();
            jsonSerializer.Serialize(movies, "movies.json");
            List<Movie> jsonDeserializedMovies = jsonSerializer.Deserialize<List<Movie>>("movies.json");
            Console.WriteLine("\nJSON Deserialized:");
            foreach (var movie in jsonDeserializedMovies)
            {
                Console.WriteLine(movie);
            }

            // XML сериализация и десериализация
            ISerializer xmlSerializer = new XmlSerializerWrapper();
            xmlSerializer.Serialize(movies, "movies.xml");
            List<Movie> xmlDeserializedMovies = xmlSerializer.Deserialize<List<Movie>>("movies.xml");
            Console.WriteLine("\nXML Deserialized:");
            foreach (var movie in xmlDeserializedMovies)
            {
                Console.WriteLine(movie);
            }

            //---------------------3)-------------------

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("movies.xml");

            // Создаем объект XPath
            XPathNavigator navigator = xmlDocument.CreateNavigator();

            // 1. Выбор всех узлов <Movie>
            XPathNodeIterator movieNodes = navigator.Select("/ArrayOfMovie/Movie");

            // Вывод информации о фильмах
            Console.WriteLine("\nXPath: Все фильмы:");

            // Вывод информации о фильмах
            while (movieNodes.MoveNext())
            {
                string duration = movieNodes.Current.SelectSingleNode("Duration")?.Value;
                Console.WriteLine($"Длительность: {duration} мин");
            }

            // 2. Выбор длительности второго фильма
            XPathNavigator secondMovieDuration = navigator.SelectSingleNode("/ArrayOfMovie/Movie[2]/Duration");

            Console.WriteLine($"\nXPath: Продолжительность второго фильма: {secondMovieDuration?.Value}");

            Console.ReadKey();
        }
    }
}






