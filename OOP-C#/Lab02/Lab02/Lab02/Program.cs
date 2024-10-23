using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Сохдание объекта через конструктор с параметрами
            Phone phone1 = new Phone("Иванов", "Иван", "Иванович", "Минск", "1234567890123456", 1000m, 500m, 120, 45); 
            phone1.DisplayInfo();
            //для сравнения:
            Phone phone11 = new Phone("Иванов", "Иван", "Иванович", "Минск", "1234567890123456", 1000m, 500m, 120, 45);

            //Используем ref и out
            decimal newDebit = 1200m;
            int totalCallTime;
            phone1.UpdateBalanceAndCallTime(ref newDebit, out totalCallTime);
            Console.WriteLine($"Обновленный дебет: {phone1.Debit}, Общее время звонков: {totalCallTime} минут\n");

            Phone phone2 = new Phone();
            phone2.DisplayInfo();

            Phone phone3 = Phone.WithPrConstr();
            phone3.DisplayInfo();

            //Статические методы принадлежат классу
            Phone.DisplayClassInfo();

            //Метод Equals
            Console.WriteLine(phone1.Equals(phone11));

            //Переопределение gethashcode
            int hash1 = phone1.GetHashCode();
            int hash11 = phone11.GetHashCode();
            Console.WriteLine($"phone1 HashCode: {hash1}, phone11 HashCode: {hash11}");

            Console.WriteLine(phone1.ToString());

            //----------------------------------------

            // Создаем массив объектов Phone
            Phone[] phones = new Phone[]
            {
                new Phone("Иванов", "Иван", "Иванович", "Минск", "1234567890123456", 1000m, 500m, 150, 0),
                new Phone("Петров", "Петр", "Петрович", "Гродно", "9876543210987654", 500m, 200m, 90, 30),
                new Phone("Сидоров", "Алексей", "Васильевич", "Гомель", "1231231231231231", 1200m, 300m, 200, 60),
                new Phone("Кузнецов", "Сергей", "Сергеевич", "Могилев", "6546546546546546", 800m, 100m, 60, 0)
            };

            // Заданное время для фильтрации внутригородских разговоров
            int givenLocalCallTime = 100;

            // a) Сведения об абонентах, у которых время внутригородских разговоров превышает заданное
            Console.WriteLine($"Абоненты с внутригородскими разговорами больше {givenLocalCallTime} минут:");
            foreach (var phone in phones)
            {
                if (phone.LocalCallTime > givenLocalCallTime)
                {
                    phone.DisplayInfo();
                    Console.WriteLine();
                }
            }

            // b) Сведения об абонентах, которые пользовались междугородной связью
            Console.WriteLine("Абоненты, пользовавшиеся междугородной связью:");
            foreach (var phone in phones)
            {
                if (phone.LongDistanceCallTime > 0)
                {
                    phone.DisplayInfo();
                    Console.WriteLine();
                }
            }

            //---------------------------------------------------

            // Создаем и выводим анонимный тип в конце программы
            var anonymousPhone = new
            {
                Id = 1,
                LastName = "Анонимов",
                FirstName = "Аноним",
                MiddleName = "Анонимович",
                Address = "Неизвестно",
                CreditCardNumber = "0000000000000000",
                Debit = 1000m,
                Credit = 500m,
                LocalCallTime = 120,
                LongDistanceCallTime = 30
            };

            // Выводим информацию об объекте анонимного типа
            Console.WriteLine("\nАнонимный тип:");
            Console.WriteLine($"ID: {anonymousPhone.Id}, ФИО: {anonymousPhone.LastName} {anonymousPhone.FirstName} {anonymousPhone.MiddleName}");
            Console.WriteLine($"Адрес: {anonymousPhone.Address}");
            Console.WriteLine($"Кредитная карта: {anonymousPhone.CreditCardNumber}, Дебет: {anonymousPhone.Debit}, Кредит: {anonymousPhone.Credit}");
            Console.WriteLine($"Время городских разговоров: {anonymousPhone.LocalCallTime} мин, Время международных разговоров: {anonymousPhone.LongDistanceCallTime} мин");

            Console.ReadKey();
        }
         







































        
    }

 }


