using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task12
{
    internal class Program
    {
        class A
        {
            private int _num;
            public A(int num) { Num = num; }
            public int Num { get { return _num; } set { _num = value; } }
        }
        static void Main(string[] args)
        {
            A a = new A(5);
            A b = a;
            Console.WriteLine(a.Num + " " + b.Num);

            a.Num = 7;
            Console.WriteLine(a.Num + " " + b.Num);

            Console.ReadKey();
        }
    }
}
