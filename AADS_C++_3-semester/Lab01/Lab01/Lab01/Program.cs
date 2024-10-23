using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    class Class01
    {
        static void Main(string[] args)
        {
            bool Bool = true;//2 байт
            string String = "Hello";
            byte Byte = 255; //беззнаковое, от 0 до 255
            sbyte Sbyte = 127; //знаковое, от -128 до 127
            char Char = '!'; //2 байт
            decimal Decimal = 7922816251426433759354395033m; //16 байт
            double Double = 12.13245;//8 байт
            float Float = 3.14f;//4 байт
            int Int = 1234;//знаковое, 4 байт
            uint Uint = 5678;//беззнаковое, 4 байт
            long Long = 12345678912345;//знаковое, 8 байт
            ulong Ulong = 12334434344;//беззнаковое, 8 байт
            short Short = 327;//знаковое, 2 байт
            ushort Ushort = 657;//беззнаковое, 2 байт

            Console.WriteLine("bool: " + Bool);
            Console.WriteLine("string: " + String);
            Console.WriteLine("byte: " + Byte);
            Console.WriteLine("sbyte: " + Sbyte);
            Console.WriteLine("char: " + Char);
            Console.WriteLine("decimal: " + Decimal);
            Console.WriteLine("double: " + Double);
            Console.WriteLine("float: " + Float);
            Console.WriteLine("int: " + Int);
            Console.WriteLine("uint: " + Uint);
            Console.WriteLine("long: " + Long);
            Console.WriteLine("ulong: " + Ulong);
            Console.WriteLine("short: " + Short);
            Console.WriteLine("ushort: " + Ushort);

            Console.WriteLine("--------------------");

            Console.WriteLine("Введите значение для bool: ");
            string boolValue = Console.ReadLine();
            Bool = Convert.ToBoolean(boolValue);
            Console.WriteLine("Значение для boll: " + Bool);

            Console.WriteLine("Ведите значение для int: ");
            string intValue = Console.ReadLine();
            Int = Convert.ToInt32(intValue);
            Console.WriteLine("Значение для int: " + Int);


            Console.ReadKey();
        }
    }
}
