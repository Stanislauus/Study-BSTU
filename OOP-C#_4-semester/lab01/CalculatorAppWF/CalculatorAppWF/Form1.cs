using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorAppWF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = textBox1.Text;
            string b = textBox2.Text;
            int a_1;
            int b_1;
            int ab;
            try
            {
                a_1 = Convert.ToInt32(a);
                b_1 = Convert.ToInt32(b);

                ab = a_1 + b_1;
                textBox3.Text = ab.ToString();
            }
            catch(Exception err)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                Console.WriteLine("{0} Eception caught.", err);
            }

           

            
        }
    }
}
