using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public partial class Phone
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
    }
}
