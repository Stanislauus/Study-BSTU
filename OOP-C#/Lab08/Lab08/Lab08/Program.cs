using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab08
{
    //делегаты
    public delegate void UpgradeDelegate();

    public delegate void TurnOnDelegate(int voltage);


    public class Boss
    {
        //события
        public event UpgradeDelegate Upgrade;

        public event TurnOnDelegate TurnOn;

        //метод для вызова события Upgrade
        public void RaiseUpgrade()
        {
            Console.WriteLine("---Событие Upgrade запущено---");
            //есть хотя бы один подписчик?
            if (Upgrade != null)
            {
                //вызов события
                //вызывает все методы, подписанные на событие
                Upgrade();
            }
        }

        //метод для вызова события TurnOn
        public void RaiseTurnOn(int voltage)
        {
            Console.WriteLine($"\n---Событие TurnOn с напряжением {voltage}V запущено---");
            if (TurnOn != null)
            {
                TurnOn(voltage);//инициация события
            }
        }

    }

    //класс для техники
    public class Tech
    {
        public string Name { get; set; }
        public Tech(string name)
        {
            Name = name;
        }

        //реакция на событие Upgrade
        public void OnUpgrade()
        {
            Console.WriteLine($"Техника {Name} получила обновление!");
        }

        //реакция на событие TurnOn
        public void OnTurnOn(int voltage)
        {
            if (voltage >= 220)
            {
                Console.WriteLine($"Техника {Name} включена при {voltage}V");
            }
            else
            {
                Console.WriteLine($"Техника {Name} не включится при {voltage}V");
            }
        }
    }

    //класс для semitech
    public class SemiTech
    {
        public string Name { get; set; }    
        public SemiTech(string name)
        {
            Name = name;
        }

        public void OnUpgrade()
        {
            Console.WriteLine($"Робот {Name} обновляется... Это может занять немного времени!");
        }

        public void OnTurnOn(int voltage)
        {
            if (voltage >= 200)
            {
                Console.WriteLine($"Робот {Name} включен при {voltage}V");
            }
            else
            {
                Console.WriteLine($"Робот {Name} не включится при {voltage}V");
            }
        }

    }


    internal class Program
    {
        
        static void Main(string[] args)
        {
            Boss boss = new Boss();

            Tech tech1 = new Tech("Радио");
            Tech tech2 = new Tech("Станок");

            SemiTech semiTech1 = new SemiTech("SA-001");
            SemiTech semiTech2 = new SemiTech("SV-011");

            //подписываем объекты на событтия с разными реакциями
            //привязка обработчика к событию
            boss.Upgrade += tech1.OnUpgrade;
            boss.Upgrade += tech2.OnUpgrade;

            boss.Upgrade += semiTech1.OnUpgrade;
            boss.Upgrade += semiTech2.OnUpgrade;

            boss.TurnOn += tech1.OnTurnOn;
            boss.TurnOn += tech2.OnTurnOn;

            boss.TurnOn += semiTech1.OnTurnOn;
            boss.TurnOn += semiTech2.OnTurnOn;

            //вызываются методы, которые инициируют события (запускают)
            boss.RaiseUpgrade();
            boss.RaiseTurnOn(220);

            boss.RaiseTurnOn(100);


            Console.ReadKey();
        }
    }
}
