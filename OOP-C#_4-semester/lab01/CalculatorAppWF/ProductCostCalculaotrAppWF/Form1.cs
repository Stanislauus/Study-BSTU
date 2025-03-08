using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductCostCalculaotrAppWF
{
    //WIN FORM
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //обработчик события от Calculator
        public void Calculator_CalculationPerformed(object sender, CalculationEventArgs e)
        {
            textBox5.Text = e.CostOneKg.ToString();
            textBox6.Text = e.CostPrice.ToString();
            textBox7.Text = e.ExpensesMonth.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int cost = Convert.ToInt32(textBox1.Text);
                int weight = Convert.ToInt32(textBox2.Text);
                int fixMarkup = Convert.ToInt32(textBox3.Text);
                int daily = Convert.ToInt32(textBox4.Text);

                ICalculator calculator = new Calculator();

                calculator.CalculationPerformed += Calculator_CalculationPerformed;

                //выполняем вычисления
                calculator.Calculate(cost, weight, fixMarkup, daily);
            }
            catch (Exception ex)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";

                Console.WriteLine("{0} Eception caught.", ex);
            }
        }
    }



    public delegate void CalculationDelegate(object sender, CalculationEventArgs e);

    public class CalculationEventArgs : EventArgs
    {
        public int CostOneKg { get; }
        public int CostPrice { get; }
        public int ExpensesMonth { get; }

        public CalculationEventArgs(int cost, int price, int expences)
        {
            CostOneKg = cost;
            CostPrice = price;
            ExpensesMonth = expences;
        }
    }

    public interface ICalculator
    {
        //событие, которое ген. после выполнения вычислений
        event CalculationDelegate CalculationPerformed;

        void Calculate(int cost, int weight, int fixMarkup, int daily);
    }

    public class Calculator : ICalculator
    {
        public event CalculationDelegate CalculationPerformed;

        

        public void Calculate(int cost, int weight, int fixMarkap, int daily)
        {
            if (weight == 0)
                throw new DivideByZeroException("Вес не может равняться нулю!");

            int costOneKg = cost / weight;
            int costPrice = costOneKg - fixMarkap;
            int expensesMonth = costPrice * daily * 30;

            //ген. событие с результатами
            OnCalculationPerformed(new CalculationEventArgs(costOneKg, costPrice, expensesMonth));
            
        }

        //метод для безопасного вызова события (проверка на наличие подписчиков)
        protected virtual void OnCalculationPerformed(CalculationEventArgs e)
        {
            CalculationPerformed?.Invoke(this, e);
        }
    }

    
}
