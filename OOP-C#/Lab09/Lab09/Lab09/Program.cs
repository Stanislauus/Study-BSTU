using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel; // для ObservableCollection<T>
using System.Collections.Specialized;

namespace Lab09
{
    public class Furniture
    {
        public string Name { get; set; }
        public string Material { get; set; }
        public decimal Price { get; set; }

        public Furniture(string name, string material, decimal price)
        {
            Name = name;
            Material = material;
            Price = price;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Material: {Material}, Price: {Price:C}";
        }

       
    }

    public class FurnitureCollection : IList<Furniture>
    {
        //ArrayList хранит объекты типа object

        private ArrayList furnitureList = new ArrayList();

        //IList<T> требует от нас реализации ряда методов

        public void Add(Furniture item)
        {
            furnitureList.Add(item);
        }

        public bool Remove(Furniture item)
        {
            if (furnitureList.Contains(item))
            {
                furnitureList.Remove(item);
                return true;
            }
            return false;
        }

        //индексатор
        public Furniture this[int index]
        {
            get
            {
                return (Furniture)furnitureList[index];
            }
            set
            {
                furnitureList[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return furnitureList.Count;
            }
        }

        //коллекция не является доступной только для чтения
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Clear()
        {
            furnitureList.Clear();
        }

        public bool Contains(Furniture item)
        {
            return furnitureList.Contains(item);
        }

        //копирует все элементы коллекции furnitureList в массив array
        public void CopyTo(Furniture[] array, int arrayIndex)
        {
            furnitureList.CopyTo(array, arrayIndex);
        }

        //интерфейс, который позволяет перебирать объекты типа Furniture в коллекции.
        public IEnumerator<Furniture> GetEnumerator()
        {
            foreach (Furniture furniture in furnitureList)
            {
                yield return furniture;//возвр. по одному (итератор)
            }
        }

        //нетипизированная версия метода GetEnumerator
        //Enumerator определяет(MoveNext(), Current { get; }, Reset())
        IEnumerator IEnumerable.GetEnumerator()//метод, который реализует интерфейс IEnumerable и возвращает объект, реализующий IEnumerator
        {
            return GetEnumerator();
        }

        //возвращает индекс первого вхождения элемента item
        public int IndexOf(Furniture item)
        {
            return furnitureList.IndexOf(item);
        }

        //вставляет элемент item в коллекцию furnitureList на позицию с индексом index.
        public void Insert(int index, Furniture item)
        {
            furnitureList.Insert(index, item);
        }

        //удаляет элемент из коллекции furnitureList по указанному индексу index
        public void RemoveAt(int index)
        {
            furnitureList.RemoveAt(index);
        }

        
        









    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //создали экз. коллекции
            FurnitureCollection furnitureCollection = new FurnitureCollection();

            furnitureCollection.Add(new Furniture("Chair", "Wood", 100.0m));
            furnitureCollection.Add(new Furniture("Table", "Metal", 300.0m));
            furnitureCollection.Add(new Furniture("Sofa", "Leather", 750.0m));

            Console.WriteLine("Furniture Collection:");
            foreach (var item in furnitureCollection)
            {
                Console.WriteLine(item);
            }

            //поиск и удаление объекта
            var chair = furnitureCollection[0];
            furnitureCollection.Remove(chair);

            //вывод после удаления
            Console.WriteLine("\nFurniture Collection после удаления:");
            foreach (var item in furnitureCollection)
            {
                Console.WriteLine(item);
            }

            //добавление нового объекта и вывод
            furnitureCollection.Add(new Furniture("Desk", "Glass", 450.0m));
            Console.WriteLine("\nFurniture Collection после добавления нового объекта:");
            foreach (var item in furnitureCollection)
            {
                Console.WriteLine(item);
            }

            //--------------------------------2-----------------------------

            List<int> numberCollection = new List<int>();

            numberCollection.Add(1);
            numberCollection.Add(2);
            numberCollection.Add(3);
            numberCollection.Add(4);
            numberCollection.Add(5);

            Console.WriteLine("\nКоллекция numberCollection (List<int>):");
            foreach (var item in numberCollection)
            {
                Console.WriteLine(item);
            }

            int n = 3; //количество элементов для удаления

            if (n <= numberCollection.Count)
            {
                numberCollection.RemoveRange(0, n);
            }

            Console.WriteLine($"\nПосле удаления {n} элеметов из numberCollection:");
            foreach (var item in numberCollection)
            {
                Console.WriteLine(item);
            }

            //добавление новых элементов
            numberCollection.Add(6);
            numberCollection.Insert(1, 3);

            List<int> numberCollection01 = new List<int>();
            numberCollection01.Add(11);
            numberCollection01.Add(12);

            numberCollection.AddRange(numberCollection01);

            Console.WriteLine("\nПосле добавления новых элементов:");
            foreach (var item in numberCollection)
            {
                Console.WriteLine(item);
            }

            //создание второй коллекции и заполнение данными из первой
            List<int> numberCollection2 = new List<int>();
            numberCollection2.Insert(0, 0);

            numberCollection2.AddRange(numberCollection);

            Console.WriteLine("\nПосле добавления данных из 1 коллекции во 2:");
            foreach (var item in numberCollection2)
            {
                Console.WriteLine(item);
            }

            //поиск заданного значения во второй коллекции
            int searchValue = 6;
            bool found = numberCollection2.Contains(searchValue);

            if (found == true)
            {
                Console.WriteLine($"\nЗначение {searchValue} найдено во второй коллекции.");
            }
            else
            {
                Console.WriteLine($"\nЗначение {searchValue} не найдено во второй коллекции.");             
            }

            //--------------------------------3-----------------------------

            ObservableCollection<Furniture> observableFurnitureCollection = new ObservableCollection<Furniture>();

            //регистрируем обработчик события CollectionChanged
            observableFurnitureCollection.CollectionChanged += OnCollectionChanged;//срабатывает всякий раз, когда происходит изменение в коллекции

            //добавляем элементы
            Console.WriteLine("\nДобавляем элементы в ObservableCollection:");

            observableFurnitureCollection.Add(new Furniture("Chair", "Wood", 100.0m));
            observableFurnitureCollection.Add(new Furniture("Table", "Metal", 300.0m));
            observableFurnitureCollection.Add(new Furniture("Sofa", "Leather", 750.0m));

            //удаляем элемент
            Console.WriteLine("\nУдаляем элемент из ObservableCollection:");
            observableFurnitureCollection.Remove(observableFurnitureCollection[0]);

            //

            Console.ReadKey();
        }


        //--------------------------------------3-------------------------

        //вызывается автоматически, когда коллекция меняется
        //Использует обработчик события CollectionChanged, которое предоставляет данные о том, что именно изменилось
        //обработчик события
        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) //этот параметр содержит данные о произошедших изменениях
        {

            switch (e.Action) //указывает, какое именно действие было выполнено над коллекцией
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Furniture newItem in e.NewItems) //список новых элементов, добавленных в коллекцию
                    {
                        Console.WriteLine($"Элемент добавлен: {newItem}");
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Furniture oldItem in e.OldItems) //список удаленных элементов из коллекции
                    {
                        Console.WriteLine($"Элемент удален: {oldItem}");
                    }
                    break;

                /*case NotifyCollectionChangedAction.Replace:
                    Console.WriteLine("Элемент заменен.");
                    break;

                case NotifyCollectionChangedAction.Move:
                    Console.WriteLine("Элемент перемещен.");
                    break; 

                case NotifyCollectionChangedAction.Reset:
                    Console.WriteLine("Коллекция очищена.");
                    break;*/
            }
        }

    }
}
