using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Runtime.InteropServices;

namespace Lab11
{

    public class TypeInfo
    {
        public string ClassName { get; set; }
        public string AssemblyName { get; set; }
        public bool HasPublicConstructors { get; set; }
        public IEnumerable<string> PublicMethods { get; set; }
        public IEnumerable<string> FieldsAndProperties { get; set; }
        public IEnumerable<string> Interfaces { get; set; }
        public IEnumerable<string> MethodsWithParameterType { get; set; }
        public IEnumerable<object> MethodWithParameterValue { get; set; }

 

    }
    public static class Reflector
    {

        //определение имени сборки, в которой  определен класс
        public static string GetAssemblyName(object obj)
        {
            Type type = obj.GetType();
            return type.Assembly.FullName;
        }

        //проверка наличия публичных конструкторов у объекта
        public static bool HasPublicConstructors(object obj)
        {
            Type type = obj.GetType();
            //имеет ли тип хотя бы один публичный экземплярный конструктор
            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any();
        }

        //извлечение публичных методов класса
        public static IEnumerable<string> GetPublicMethods(object obj)
        {
            Type type = obj.GetType();

            //метод GetMethods() возвращает MethodInfo[]
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(method => method.Name);
        }

        //получение о всех полях и свойствах
        public static IEnumerable<string> GetFieldsAndProperties(object obj)
        {
            Type type = obj.GetType();
            
            IEnumerable<string> fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(field => $"Field: {field.Name}");

            IEnumerable<string> properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(property => $"Property: {property.Name}");

            return fields.Concat(properties);
        }

        //получение всех реализованных классом интерфесов
        public static IEnumerable<string> GetInterfaces(object obj)
        {
            Type type = obj.GetType();
            return type.GetInterfaces().Select(i => i.FullName);
        }

        //выводим по имени класса имена методов, которые содержат заданный (пользователем) тип параметра 
        public static IEnumerable<string> GetMethodsWithParameterType(string className, Type parameterType)
        {
            //получаем тип по имени класса (другой спсоб без создания объекта)
            Type type = Type.GetType(className);

            if (type == null)
            {
                throw new ArgumentException($"Класс с именем {className} не найден.");
            }

            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(method => method.GetParameters().Any(parameter => parameter.ParameterType == parameterType)).Select(method => method.Name);
        }

        //-------------------Дополнительно------------------

        public static IEnumerable<object> GetMethodsWithParameterValue(object obj)
        {
            Type type = obj.GetType();

            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(method => method.GetParameters()

            .Any(parameter => parameter.HasDefaultValue)).Select(method =>
            {
                //собираем параметры со значениями по умолчанию
                var parametersWithDefaults = method.GetParameters().Where(parameter => parameter.HasDefaultValue)
                //LINQ, содержит функцию для ключа и функцию для значения
                .ToDictionary( 
                    parameter => parameter.Name,
                    parameter => parameter.DefaultValue
                 );

                //возвращаем анонимный объект
                return new
                {
                    Method = method.Name,
                    Parameters = parametersWithDefaults

                };

                
            });
        }
       



        //----------------------------------g)-----------------------------

        //меод для генерации значений для каждого типа параметра
        public static object GenerateValue(Type type)
        {
            object result = null;
            Random randomGenerator = new Random();  

            if (type == typeof(int))
            {
                result = randomGenerator.Next(1, 100);   
            }
            else if (type == typeof(string))
            {
                result = "Generated string";
            }
            else if (type == typeof(bool))
            {
                result = true;
            }

            return result;
        }   



        //метод Invoke, который вызывает метод класса с параметрами из JSON или сгенерированными значениями.
        public static void Invoked(object obj, string methodName, string jsonFilePath)
        {
        
            //чтение JSON-файла для получения информации о классе и методе.
            string jsonContent = File.ReadAllText(jsonFilePath);

            //десериализация, преобразование jsonContent в объект типа TypeInfo
            var typeInfo = JsonSerializer.Deserialize<TypeInfo>(jsonContent);

            if (typeInfo == null)
            {
                Console.WriteLine("Ошибка чтения JSON-файла.");
                return;
            }

            if(typeInfo.ClassName != obj.GetType().FullName)
            {
                Console.WriteLine($"Класс объекта {obj.GetType().FullName} не совпадает с данными в JSON: {typeInfo.ClassName}");
            }



            //-----------------------(берем данные из кода, а не из JSON)----------------------------

            //реализации рефлексии; для получения метаинформации о типе объекта; динамическое выполнение кода
            Type type = obj.GetType();

            //получение метода по имени (содержит информацию о методе)
            MethodInfo method = type.GetMethod(methodName);

            if (method == null)
            {
                Console.WriteLine($"Метод {methodName} не найден в классе {type.Name}.");
                return;
            }

            //получение параметров метода
            ParameterInfo[] parameters = method.GetParameters();

            //создание массива для хранения значений параметров
            object[] methodParameters = new object[parameters.Length];

            //---------------------------------------------------------------------------------------



            //хранит информацию о методе (имя и параметры) из JSON
            var methodInfoFromJson = typeInfo.MethodWithParameterValue
                .OfType<JsonElement>()
                .Select(e => JsonSerializer.Deserialize<Dictionary<string, object>>(e.GetRawText()))//каждый элемент коллекции (e) типа JsonElement и возвращает его JSON-строку
                .FirstOrDefault(m => m != null && m.ContainsKey("Method") && m["Method"].ToString() == methodName);

            //проверка наличия данных о параметрах в JSON
            if (methodInfoFromJson != null && methodInfoFromJson.ContainsKey("Parameters"))
            {
                //значение ключа "Parameters" (которое является JSON-строкой) снова десериализуется в Dictionary
                var parameterValues = JsonSerializer.Deserialize<Dictionary<string, object>>(methodInfoFromJson["Parameters"].ToString());//т.к JsonElement вложенный, то сперва .ToString()

                Console.WriteLine($"Параметры из JSON для метода {methodName}:");

                foreach (var param in parameterValues)
                {
                    Console.WriteLine($"Имя: {param.Key};\nЗначение: {param.Value}");
                }



                //-----------------------(берем данные из кода, а не из JSON)----------------------------

                //заполнение массива параметров (methodParameters)
                for (int i = 0; i < parameters.Length; i++)
                {


                    //проверяется,  существует ли в словаре parameterValues ключ с именем текущего параметра (parameters[i].Name).
                    if (parameterValues != null && parameterValues.TryGetValue(parameters[i].Name, out var value))
                    {

                        try
                        {
                            if (value is JsonElement jsonElement)
                            {
                                value = jsonElement.Deserialize(parameters[i].ParameterType);//дес. в нужный тип; теперь value - объект нужного типа;  является оберткой (object)
                            }
                            methodParameters[i] = Convert.ChangeType(value, parameters[i].ParameterType);//привести value к точному типу, который ожидает параметр метода
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine($"\nОшибка преобразования параметра {parameters[i].Name}: {ex.Message}");
                            methodParameters[i] = GenerateValue(parameters[i].ParameterType);
                        }


                    }
                    else
                    {
                        methodParameters[i] = GenerateValue(parameters[i].ParameterType);
                    }
                }

                //---------------------------------------------------------------------------------------



            }
            else
            {
                Console.WriteLine($"\nДанные о параметрах для метода {methodName} отсутствуют в JSON.");
               
                for (int i = 0; i < parameters.Length; i++)
                {
                    methodParameters[i] = GenerateValue(parameters[i].ParameterType); 
                }
                
            }


            
            //динамически вызываем метод (рефлексия)
            object result = method.Invoke(obj, methodParameters);


            Console.WriteLine($"\nМетод {methodName} вызван успешно.");


        }

        //-----------------------------------------------------------------

        public static void SaveToJson(object obj, string filePath, string className ,Type parameterType)
        {
            TypeInfo result = new TypeInfo();

            result.ClassName = obj.GetType().FullName;
            result.AssemblyName = GetAssemblyName(obj);
            result.HasPublicConstructors = HasPublicConstructors(obj);
            result.PublicMethods = GetPublicMethods(obj);
            result.FieldsAndProperties = GetFieldsAndProperties(obj);
            result.Interfaces = GetInterfaces(obj);
            result.MethodsWithParameterType = GetMethodsWithParameterType(className, parameterType);
            result.MethodWithParameterValue = GetMethodsWithParameterValue(obj);

           
            //сериализация
            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true});
            File.WriteAllText(filePath, json);
            Console.WriteLine("\n\n--------------------");
            Console.WriteLine($"Результаты сохранены в файл {filePath}");
            Console.WriteLine("--------------------\n\n");
        }



        //-------------------------Задание2-----------------------
        public static T Create<T>(params object[] parameters) where T : class
        {
            Type type = typeof(T);

            // Поиск подходящего конструктора
            ConstructorInfo constructor = type.GetConstructor(parameters.Select(p => p.GetType()).ToArray());//содержит информацию о конструкторе класса

            if (constructor == null)
            {
                throw new ArgumentException("Подходящий конструктор не найден.");
            }

            // Создание объекта
            return (T)constructor.Invoke(parameters);
        }
        //--------------------------------------------------------



    }

    public interface IMyClass
    {
        void ShowInfo();
    }
    public class MyClass : IMyClass
    {
        //public MyClass() { }
        public void MyMethod() { }
        public string MyName {  get; set; }

        public int myAge;
        //public void ShowInfo() { }
        public void AnotherMethodWhithValue(int value = 23)
        {
            Console.WriteLine($"\nПараметр по умолчанию: {value}");
        }
        public void AnotherMethod(string value)
        {
            Console.WriteLine($"\nGeneratedValue: {value}");
        }

        //------------------Задание2--------------
        public MyClass()
        {
            MyName = "DefaultName";
            myAge = 18;
        }

        public MyClass(string name, int age)
        {
            MyName = name;
            myAge = age;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Name: {MyName}, Age: {myAge}");
        }
        //----------------------------------------
    }

    

    internal class Program
    {
        static void Main(string[] args)
        {
            MyClass myClass = new MyClass();

            //-------------------a)-------------------

            string getAssemblyName = Reflector.GetAssemblyName(myClass);
            Console.WriteLine($"Полное имя сборки: {getAssemblyName}");

            //-------------------b)-------------------

            bool hasPublicConstructure = Reflector.HasPublicConstructors(myClass);
            Console.WriteLine($"\nПроверка наличия публичных конструкторов у объекта: {hasPublicConstructure}");

            //-------------------c)-------------------

            //или var
            IEnumerable<string> getPublicMethods = Reflector.GetPublicMethods(myClass);

            Console.WriteLine("\nИзвлечение публичных методов класса:");

            foreach (var item in getPublicMethods)
            {
                Console.WriteLine(item);
            }

            //-------------------d)-------------------

            IEnumerable<string> getFieldsAndProperties = Reflector.GetFieldsAndProperties(myClass);

            Console.WriteLine("\nИнформация о полях и свойствах класса:");

            foreach(var item in getFieldsAndProperties)
            {
                Console.WriteLine(item);
            }

            //-------------------e)-------------------

            IEnumerable<string> getInterfaces = Reflector.GetInterfaces(myClass);

            Console.WriteLine("\nИзвлечение всех реализованных классом интерфесов:");

            foreach (var item in getInterfaces)
            {
                Console.WriteLine(item);
            }

            //-------------------f)-------------------

            Console.WriteLine("\nВведите имя класса для поиска метода с определенным типом параметра (например, Lab11.MyClass):");

            string className = Console.ReadLine();

            Console.WriteLine("\nВведите имя типа параметра (например, System.String или System.Int32):");

            string parameterTypeName = Console.ReadLine();  

            //преобразуем строку с именем типа в объект Type
            Type parameterType = Type.GetType(parameterTypeName);

            if (parameterType == null)
            {
                Console.WriteLine("Указанный тип параметра не найден.");
            }
            else
            {
                IEnumerable<string> getMethodsWithParameterType = Reflector.GetMethodsWithParameterType(className, parameterType);
                Console.WriteLine($"\nМетоды, содержащие параметр типа {parameterType}:");
                foreach (var item in getMethodsWithParameterType)
                {
                    Console.WriteLine(item);
                }
            }

        

            string filePath = "myJson.json";
            Reflector.SaveToJson(myClass, filePath, className, parameterType);

            //-------------------g)-------------------

            Reflector.Invoked(myClass, "AnotherMethodWhithValue", filePath);//другой пример с использованием -> AnotherMethod

            //----------------------------------------

            //---------------------Задание3-------------------

            Console.WriteLine("\n\n---------------------Задание3-------------------");

            // Пример использования метода Create
            Console.WriteLine("\nСоздание объекта MyClass с параметрами через метод Reflector.Create:");
            var createdObject = Reflector.Create<MyClass>("GeneratedName", 25);
            createdObject.ShowInfo();

            Console.WriteLine("\nСоздание объекта MyClass с использованием конструктора без параметров:");
            var defaultObject = Reflector.Create<MyClass>();
            defaultObject.ShowInfo();

            //------------------------------------------------

            Console.ReadKey();
        }
    }
}
