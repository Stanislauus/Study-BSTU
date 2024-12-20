using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10
{

    public class Phone
    {
        //Поле только для чтения 
        private readonly int id;

        //Поле-константа
        public const int MaxCallTime = 300;//пришлось сделать 300 из-за последнего задания

        private string lastName;
        private string firstName;
        private string middleName;
        private string address;
        private string creditCardNumber;
        private decimal debit;
        private decimal credit;
        private int localCallTime; // Время разговоров в минутах
        private int longDistanceCallTime;

        // Статическое поле для подсчета созданных объектов
        private static int count;




        public int Id//Свойство для доступа к полю
        {
            get { return id; }//метод, который позволяет получать значение приватного поля
            /*set//метод, который устанавливает новое значение для поля 
            {
                if (value <= 0)//value - это встроенная переменнная, которая автоматически содержит значение, присваемое свойству
                {
                    throw new ArgumentException("ID должен быть положительным.");//инструкция, которая используется для выброса исключения
                }
                id = value;
            }*/
        }


        public string LastName
        {
            get { return lastName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Фамилия не может быть пустой.");
                }
                lastName = value;
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя не может быть пустым.");
                }
                firstName = value;
            }
        }

        public string MiddleName
        {
            get { return middleName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Отчество не может быть пустым.");
                }
                middleName = value;
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Адрес не может быть пустым.");
                }
                address = value;
            }
        }

        public string CreditCardNumber
        {
            get { return creditCardNumber; }
            set
            {
                if (value.Length != 16)
                {
                    throw new ArgumentException("Номер кредитной карты должен содержать 16 цифр.");
                }
                creditCardNumber = value;
            }
        }

        public decimal Debit
        {
            get { return debit; }
            private set // Доступ только для внутреннего использования
            {
                if (value < 0)
                {
                    throw new ArgumentException("Дебет не может быть отрицательным.");
                }
                debit = value;
            }
        }

        public decimal Credit
        {
            get { return credit; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Кредит не может быть отрицательным.");
                }
                credit = value;
            }
        }

        public int LocalCallTime
        {
            get { return localCallTime; }
            set
            {
                if (value < 0 || value > MaxCallTime)
                {
                    throw new ArgumentException("Время городских разговоров не может быть отрицательным.");
                }
                localCallTime = value;
            }
        }

        public int LongDistanceCallTime
        {
            get { return longDistanceCallTime; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Время международных разговоров не может быть отрицательным.");
                }
                longDistanceCallTime = value;
            }
        }

        //Расчет текущего баланса
        public decimal Balance()
        {
            return debit - credit;
        }


        //Статический конструктор
        //Статический конструктор выполняется только один раз — при первом обращении к классу
        static Phone()
        {
            count = 0;
            Console.WriteLine("Статический конструктор вызван.");
        }

        //Приватный конструктор
        //Ограничивает создание объектов класса из внешнего кода.
        private Phone()
        {
            id = ++count;// Инкрементируем количество объектов
            Console.WriteLine("Приватный конструктор вызван.");
        }

        // Конструктор по умолчанию
        /*public Phone()
        {
            id = GenerateId();
            lastName = "Не указано";
            firstName = "Не указано";
            middleName = "Не указано";
            address = "Не указано";
            creditCardNumber = "0000000000000000";
            debit = 0;
            credit = 0;
            localCallTime = 0;
            longDistanceCallTime = 0;
        }*/

        //Конструктор с параметрами по умолчанию
        public Phone(string lastName = "Не указано", string firstName = "Не указано", decimal debit = 0)
        {
            //Выичсление хэша для id на основе данных объекта
            id = GenerateId();
            //id = ++count;
            this.lastName = lastName;
            this.firstName = firstName;
            this.middleName = "Не указано";
            this.address = "Не указано";
            this.creditCardNumber = "0000000000000000";
            this.debit = debit;
            this.credit = 0;
            this.localCallTime = 0;
            this.longDistanceCallTime = 0;
        }

        // Конструктор с параметрами
        public Phone(string lastName, string firstName, string middleName, string address, string creditCardNumber, decimal debit, decimal credit, int localCallTime, int longDistanceCallTime)
        {
            //Выичсление хэша для id на основе данных объекта
            id = GenerateId();
            //id = ++count;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            Address = address;
            CreditCardNumber = creditCardNumber;
            Debit = debit;
            Credit = credit;
            LocalCallTime = localCallTime;
            LongDistanceCallTime = longDistanceCallTime;
        }

        //Метод для генерации уникального хэша на основе полей объекта
        private int GenerateId()
        {
            return $"{lastName}{firstName}{middleName}{address}{creditCardNumber}".GetHashCode();
        }

        //Пример вызова приватного конструктора через статический метод
        public static Phone WithPrConstr()
        {
            return new Phone();
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, ФИО: {LastName} {FirstName} {MiddleName}, Адрес: {Address}");
            Console.WriteLine($"Кредитная карта: {CreditCardNumber}, Дебет: {Debit}, Кредит: {Credit}");
            Console.WriteLine($"Время городских разговоров: {LocalCallTime} мин, Время междугородных разговоров: {LongDistanceCallTime} мин");
            Console.WriteLine($"Текущий баланс: {Balance()}\n");
        }

        //ref - передать переменную в метод для чтения и изменения and out - метод должен вернуть значение через параметр
        public void UpdateBalanceAndCallTime(ref decimal updatedDebit, out int totalCallTime)
        {
            //обновляем debit через ref параметр
            Debit = updatedDebit;

            //Находим общее время через out 
            totalCallTime = LocalCallTime + LongDistanceCallTime;
        }

        //Статический метод для вывода информации о классе
        public static void DisplayClassInfo()
        {
            Console.WriteLine($"Количество созданных объектов: {count}");
        }

        // встроенным методом класса Object
        public override bool Equals(object obj)
        {
            // Если переданный объект является текущим объектом
            if (ReferenceEquals(this, obj)) return true;

            // Если переданный объект null или его тип отличается от текущего объекта
            if (obj == null || this.GetType() != obj.GetType()) return false;

            // Приводим объект к типу Phone и сравниваем поля
            Phone other = (Phone)obj;
            return this.CreditCardNumber == other.CreditCardNumber &&
                   this.LastName == other.LastName &&
                   this.FirstName == other.FirstName;
        }

        public override int GetHashCode()
        {
            unchecked // Игнорирование переполнения при вычислении хэша
            {
                int hash = 17;
                hash = hash * 23 + (CreditCardNumber != null ? CreditCardNumber.GetHashCode() : 0);
                hash = hash * 23 + (LastName != null ? LastName.GetHashCode() : 0);
                hash = hash * 23 + (FirstName != null ? FirstName.GetHashCode() : 0);
                return hash;
            }
        }

        public override string ToString()
        {
            return $"ID: {Id}, ФИО: {LastName} {FirstName} {MiddleName}, Адрес: {Address}, " +
                   $"Кредитная карта: {CreditCardNumber}, Дебет: {Debit}, Кредит: {Credit}, " +
                   $"Время городских разговоров: {LocalCallTime} мин, Время международных разговоров: {LongDistanceCallTime} мин.";
        }
    }

    //----------------------------------Задание 5---------------------------
    public class City
    {
        public string CityName { get; set; }
        public string Region { get; set; } // Регион
    }
    //----------------------------------------------------------------------

    internal class Program
    {

        static void Main(string[] args)
        {
            string[] months = {"June", "July", "May", "December", "January", "March", "April", "August", "February", "September", "October", "November"};

            Console.WriteLine("Введите длину строки (n):");
            int n = int.Parse(Console.ReadLine());

            var containI = from month in months
                           where month.Contains("i")
                           select month;

            //1. Запрос: выбор месяцев с длиной строки равной n
            var lengthEqualN = from month in months
                               where month.Length == n
                               select month;

            Console.WriteLine($"\nМесяцы с длиной строки равной {n}:");
            foreach(var month in lengthEqualN)
            {
                Console.WriteLine(month);
            }


            //2. Запрос: только летние и зимние месяцы
            var summerAndWinter = from month in months
                                  where month == "June" || month == "July" || month == "August" ||
                                    month == "December" || month == "January" || month == "February"
                                  select month;

            Console.WriteLine("\nЛетние и зимние месяцы:");
            foreach(var month in summerAndWinter)
            {
                Console.WriteLine(month);
            }


            //3. Запрос: месяцы в алфавитном порядке
            var alphabetOrder = from month in months
                                orderby month
                                select month;

            Console.WriteLine("\nМесяцы в алфавитном порядке:");
            foreach(var month in alphabetOrder)
            {
                Console.WriteLine(month);
            }


            //4. Запрос: количество месяцев, содержащих букву "u" и длиной имени не менее 4
            var monthsWithU = from month in months
                             where month.Contains("u") && month.Length >= 4
                             select month;

            Console.WriteLine($"\nКоличество месяцев, содержащих букву 'u' и длиной не менее 4 символов: {monthsWithU.Count()}");
            foreach(var month in monthsWithU)
            {
                Console.WriteLine(month);
            }


            //----------------------------Задание 2------------------

            //создание списка с элементами Phone
            List<Phone> phoneList = new List<Phone>();

            phoneList.Add(new Phone("Иванов", "Иван", "Петрович", "Москва", "1234567812345678", 150m, 50m, 120, 30));
            phoneList.Add(new Phone("Сидоров", "Петр", "Иванович", "Питер", "8765432187654321", 200m, 30m, 80, 15));
            phoneList.Add(new Phone("Кузнецов", "Алексей", "Владимирович", "Казань", "1234987612349876", 300m, 100m, 90, 20));
            phoneList.Add(new Phone("Смирнов", "Дмитрий", "Сергеевич", "Новосибирск", "5678123456781234", 250m, 75m, 70, 25));
            phoneList.Add(new Phone("Волков", "Андрей", "Александрович", "Екатеринбург", "2345678923456789", 180m, 40m, 60, 10));
            phoneList.Add(new Phone("Козлов", "Сергей", "Олегович", "Ростов-на-Дону", "8765432187654321", 120m, 20m, 100, 50));
            phoneList.Add(new Phone("Новиков", "Михаил", "Денисович", "Челябинск", "3456781234567812", 160m, 60m, 40, 5));
            phoneList.Add(new Phone("Федоров", "Николай", "Игоревич", "Самара", "9876543298765432", 210m, 80m, 50, 35));
            phoneList.Add(new Phone("Морозов", "Олег", "Павлович", "Краснодар", "4321876543218765", 140m, 30m, 110, 45));
            phoneList.Add(new Phone("Павлов", "Владимир", "Борисович", "Сочи", "5678912345678912", 100m, 25m, 150, 0)); //=0

            Console.WriteLine("\n---------------Задание 2-----------------\n");

            Console.WriteLine("Информация об абонентах:");
            foreach (var phone in phoneList)
            {
                phone.DisplayInfo();  
            }

            //пример запроса LINQ
            Console.WriteLine("\nАбоненты с городскими звонками более 100 минут:");
            var localCallAbove100 = from phone in phoneList
                                    where phone.LocalCallTime > 100
                                    select phone;

            foreach (var phone in localCallAbove100)
            {
                Console.WriteLine(phone);               
            }

            //пример использования метода расширения LINQ
            Console.WriteLine("\nАбоненты, использовавшие междугородные звонки:");
            var longDistanceUsers = phoneList.Where(phone => phone.LocalCallTime > 0).ToList();//преобразует результат фильтрации (который является IEnumerable<Phone>) в List<Phone>

            //или
            /*var longDistanceUsers = from phone in phoneList
                        where phone.LongDistanceCallTime > 0
                        select phone;*/

            foreach (var phone in longDistanceUsers)
            {
                Console.WriteLine(phone);
            }

            //------------------------Задание 3----------------------

            decimal targetDebit = 150m;

            //Запрос: количество абонентов с заданным значением дебета
            var debitCount = phoneList.Count(phone => phone.Debit == targetDebit);
            Console.WriteLine($"\nКоличество абонентов с дебетом {targetDebit}: {debitCount}");

            //Запрос: максимальный абонент по критерию (например, максимальное время внутригородских разговоров)
            //var maxLocalCall = phoneList.Max(phone => phone.LocalCallTime);
            var maxLocalCall = phoneList.OrderByDescending(phone => phone.LocalCallTime).FirstOrDefault();
            Console.WriteLine("\nАбонент с максимальным временем внутригородских разговоров:");
            Console.WriteLine(maxLocalCall);

            //Запрос: упорядоченный список абонентов по фамилии
            var sortedByLastName = from phone in phoneList
                                   orderby phone.LastName
                                   select phone;
            Console.WriteLine("\nСписок абонентов, упорядоченный по фамилии:");
            foreach(var phone in sortedByLastName)
            {
                Console.WriteLine(phone);
            }

            //------------------------Задание 4----------------------
            //query будет содержать результат LINQ-запроса.
            var query = from phone in phoneList
                        where phone.LocalCallTime > 50 //условие
                        orderby phone.LocalCallTime descending //упорядочивание

                        //коллекция сгруппированных объектов, доступных по ключу true или false
                        group phone by phone.LongDistanceCallTime > 0 into groupedPhones //группировка

                        //проецирует данные из каждой группы в новый анонимный объект (содержит свойства)
                        select new
                        {
                            GroupKey = groupedPhones.Key ? "С международными звонками" : "Без международных звонков",
                            AverageBalance = groupedPhones.Average(p => p.Balance()), //агрегирование
                            Phones = groupedPhones.Select(p => new { p.FirstName, p.LastName, p.LocalCallTime }).Take(2) //разбиение

                        };

            //проверка наличия абонентов, у которых баланс больше 200 (квантор Any)
            bool hasHighBalance = phoneList.Any(p => p.Balance() >= 200);

            Console.WriteLine($"\nЕсть ли абоненты с высоким балансом: {hasHighBalance}");

            foreach (var group in query)
            {
                Console.WriteLine($"\nГруппа: {group.GroupKey}");
                Console.WriteLine($"Средний баланс: {group.AverageBalance}");
                Console.WriteLine("Абоненты:");
                foreach (var phone in group.Phones)
                {
                    Console.WriteLine($"  {phone.FirstName} {phone.LastName}, Время местных звонков: {phone.LocalCallTime} минут");
                }

            }

            //--------------------------Задание 5-------------------
            List<City> cityList = new List<City>
            {
                new City { CityName = "Москва", Region = "Центральный" },
                new City { CityName = "Питер", Region = "Северо-Западный" },
                new City { CityName = "Казань", Region = "Приволжский" },
                new City { CityName = "Новосибирск", Region = "Сибирский" },
                new City { CityName = "Екатеринбург", Region = "Уральский" }
            };

            //используем оператор Join для объединения данных по имени города
            var joinedData = from phone in phoneList
                             //объединение двух списков
                             join city in cityList   //вторая коллекция, с которой мы хотим связать данные из phoneList.
                             //условие объединения двух списков
                             on phone.Address equals city.CityName
                             select new
                             {
                                 FullName = $"{phone.LastName} {phone.FirstName} {phone.MiddleName}",
                                 phone.Address,
                                 city.Region,
                                 phone.Credit,
                                 phone.Debit
                             };

            Console.WriteLine("\nСписок абонентов с информацией о городе и регионе:");
            foreach (var item in joinedData)
            {
                Console.WriteLine($"ФИО: {item.FullName}, Город: {item.Address}, Регион: {item.Region}, Дебет: {item.Debit}, Кредит: {item.Credit}");
            }


            Console.ReadKey();

        }
    }
}
