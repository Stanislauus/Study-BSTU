using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;// Для Regex - класс для для поиска и работы с текстом по шаблону(рег. выр.)

namespace Lab03
{
    //------------2)---------------
    public class Production
    {
        private int id; 
        private string organizationName;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string OrganizationName
        {
            get { return organizationName; }
            set { organizationName = value; }
        }

        public Production(int id, string organizationName)
        {
            this.id = id;
            this.organizationName = organizationName;
        }

        public void Display()
        {
            Console.WriteLine($"Id: {Id}, Organization Name: {OrganizationName}");
        }

    }

    //------------1)---------------
    public class Set
    {

        private List<int> elements;//список, как дин. масс.; хранятся элементы множества

        //свойство
        public List<int> Elements
        {
            get { return elements; }
        }

        //для вложенного объекта Production
        Production production;


        //------------3)---------------

        // Вложенный класс Developer
        public class Developer
        {
            private string fullName;
            private int id;
            private string department;

            public string FullName
            {
                get { return fullName; }
                set { fullName = value; }
            }

            public int Id
            {
                get { return id; }
                set { id = value; }
            }

            public string Department
            {
                get { return department; }
                set { department = value; }
            }

            public Developer(string fullName, int id, string department)
            {
                this.fullName = fullName;
                this.id = id;
                this.department = department;
            }

            public void Display()
            {
                Console.WriteLine($"ФИО: {FullName}, ID: {Id}, Отдел: {Department}");
            }
        }
        //объект Developer
        Developer developer;



        //Создаем множество из уникальных значений
        public Set(params int[] elements)
        {
            this.elements = elements.Distinct().ToList();//исключаем дубликаты

            //инициализируем объект Production
            production = new Production(1, "My Organization");

            // Инициализируем объект Developer
            developer = new Developer("Серебряный Станислав Сергеевич", 123, "Разработка");
        }

        public void Add(int element)
        {
            if (!elements.Contains(element))
            {
                elements.Add(element); 
            }
        }

        public void Remove(int element)
        {
            elements.Remove(element);
        }

        //Индексатор
        //предоставляет способ доступа к элементам коллекции по индексу
        public int this[int index]
        {
            get { return elements[index]; }
            set
            {
                if (!elements.Contains(value))
                {
                    elements[index] = value;
                }
            }
        }

        //Перегрузка операторов:

        public static bool operator >(Set set, int element)
        {
            return set.elements.Contains(element);
        }
        //т.к пергрузка парами:
        public static bool operator <(Set set, int element)
        {
            // Перегрузка < для (Set, int) как противоположное >
            return !set.elements.Contains(element);
        }

        public static bool operator <(Set subset, Set superset)
        {
            //является ли одно множество подмножеством другого
            return subset.elements.All(e => superset.elements.Contains(e));
        }
        //т.к пергрузка парами:
        public static bool operator >(Set subset, Set superset)
        {
            // Перегрузка > для (Set, Set) как противоположное <
            // superset должно содержать все элементы subset, но не наоборот
            return superset.elements.All(e => subset.elements.Contains(e)) && subset != superset;
        }

        public static Set operator *(Set set1, Set set2)
        {
            //пересечение множеств
            var intersection = set1.elements.Intersect(set2.elements).ToArray();//нахождение общих элементов
            return new Set(intersection);

        }
        //вывод пересечения множеств
        public void Display()
        {
            Console.WriteLine("{" + string.Join(", ", elements) + "}");
        }

        //Метод для отображения информации о Production
        public void DisplayProduction()
        {
            production.Display();
        }

        // Метод для отображения информации о Developer
        public void DisplayDeveloper()
        {
            developer.Display();
        }

        //Преобразование типа
        public static explicit operator DateTime(Set set)
        {
            // Предположим, что в `Set` могут быть такие элементы:
           
            // Если элементов недостаточно, используем текущую дату
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;

            // Если первый элемент есть, задаем год
            if (set.elements.Count > 0) year = set.elements[0];

            // Если второй элемент есть, задаем месяц (1–12)
            if (set.elements.Count > 1) month = Math.Max(1, Math.Min(12, set.elements[1]));

            // Если третий элемент есть, задаем день (1–28/31)
            if (set.elements.Count > 2) day = Math.Max(1, Math.Min(DateTime.DaysInMonth(year, month), set.elements[2]));

            // Возвращаем полученную дату
            return new DateTime(year, month, day);
        }

    }

    //------------4)---------------

    //Статический класс

    public static class StatisticOperation
    {
        //метод для вычисления суммы элементов
        public static int Sum(Set set)
        {
            return set.Elements.Sum();
        }

        //разница между максимальным и минимальным
        public static int Difference(Set set)
        {
            if (set.Elements.Count == 0)
                throw new InvalidOperationException("Нельзя вычислить разницу для пустого множества.");

            int max = set.Elements.Max();
            int min = set.Elements.Min();
            return max - min;
        }

        //подсчет количества элементов
        public static int CountElements(Set set)
        {
            return set.Elements.Count;
        }

        //------------5)---------------

        // 1) Метод расширения для выделения первого числа в строке
        public static int? FirstNumber(this string input)
        {
            //метод, который ищет первое совпадение строки с шаблоном
            var match = Regex.Match(input, @"\d+");
            return match.Success ? int.Parse(match.Value) : (int?)null;
        }

        // 2) Метод расширения для удаления положительных элементов из множества
        public static void RemovePositiveElements(this Set set)
        {
            //метод List, который удаляет все элементы, соответствующие условию
            set.Elements.RemoveAll(element => element > 0);
        }

    }



    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем два множества
            Set set1 = new Set(1, 2, 3, 4, 5);
            Set set2 = new Set(4, 5, 6, 7);

            // Вывод информации о Prodaction
            Console.WriteLine("Информация о производстве для set1:");
            set1.DisplayProduction();

            // Вывод информации о разработчике для set1
            Console.WriteLine("Информация о разработчике для set1:");
            set1.DisplayDeveloper();

            // Проверка оператора > (принадлежность элемента множеству)
            Console.WriteLine("3 принадлежит set1: " + (set1 > 3));
            Console.WriteLine("6 принадлежит set1: " + (set1 > 6));

            // Проверка оператора < (проверка на подмножество)
            Set set3 = new Set(1, 2, 3);
            Console.WriteLine("set3 является подмножеством set1: " + (set3 < set1));

            // Проверка оператора * (пересечение множеств)
            Set intersections = set1 * set2;
            Console.WriteLine("Пересечение set1 и set2:");
            intersections.Display();

            // Явное преобразование типа
            Set set4 = new Set(2006, 12, 29);
            DateTime date = (DateTime)set4;
            Console.WriteLine("Преобразованная дата: " + date);

            // Вывод статических данных
            Console.WriteLine("Сумма элементов set1: " + StatisticOperation.Sum(set1));
            Console.WriteLine("Разница между максимальным и минимальным элементом в set1: " + StatisticOperation.Difference(set1));
            Console.WriteLine("Количество элементов в set1: " + StatisticOperation.CountElements(set1));

            // Пример использования метода расширения для string
            string text = "В строке 123 встречается первое число.";
            int? firstNumber = text.FirstNumber();
            Console.WriteLine($"Первое число в строке: {firstNumber}");

            // Пример использования метода расширения для Set
            Set set5 = new Set(-2, 1, 3, -5, 4);
            Console.WriteLine("Множество до удаления положительных элементов:");
            set5.Display();
            //Удаляем положительные элементы
            set5.RemovePositiveElements();
            Console.WriteLine("Множество после удаления положительных элементов:");
            set5.Display();

            Console.ReadKey();
        }
    }
}
