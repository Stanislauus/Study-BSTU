using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public partial class Phone
    {
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
            Console.WriteLine($"Текущий баланс: {Balance()}");
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
}
