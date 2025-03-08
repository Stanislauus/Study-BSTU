using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExampleAppWF
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string selectedControlType = "Экзамен"; //Используется в button2_Clic
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //Экзамен
            if (((RadioButton)sender).Checked)
                selectedControlType = "Экзамен";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //Зачет
            if (((RadioButton)sender).Checked)
                selectedControlType = "Зачет";
        }

        // Обработчик для добавления литературы в список (listBox1)
        private void button1_Click_1(object sender, EventArgs e)
        {
            string title = Prompt.ShowDialogWindow("Введите название литературы:", "Добавить литературу");
            if (string.IsNullOrWhiteSpace(title)) return;

            string author = Prompt.ShowDialogWindow("Введите автора:", "Добавить литературу");
            if (string.IsNullOrWhiteSpace(author)) return;

            string yearInput = Prompt.ShowDialogWindow("Введите год издания:", "Добавить литературу");
            if (!int.TryParse(yearInput, out int year))
            {
                MessageBox.Show("Некорректный год.");
                return;
            }

            Literature literature = new Literature
            {
                Title = title,
                Author = author,
                Year = year
            };

            listBox1.Items.Add(literature); //вызывает метод ToString() у добавленного объекта
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Для сохранения в JSON
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите название курса.");
                return;
            }

            Course course = new Course
            {
                CourseName = textBox1.Text,
                AudienceAge = trackBar1.Value,
                Difficulty = comboBox1.SelectedItem?.ToString() ?? "",
                Lectures = (int)numericUpDown1.Value,
                Labs =  (int)numericUpDown2.Value,
                ControlType = selectedControlType,
                StartDate = dateTimePicker1.Value,
                Teacher = new Teacher
                {
                    FullName = textBox2.Text,
                    Department = textBox3.Text
                },
                LiteratureList = new List<Literature>()
            };

            foreach (var item in listBox1.Items)
            {
                if (item is Literature line)
                    course.LiteratureList.Add(line);               
            }

            try
            {
                JsonSerialize.Serialize(course, "course.json");
                MessageBox.Show("Данные сохранены в course.json");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Для загрузки из JSON
            try
            {
                Course course = JsonSerialize.Deserialize<Course>("course.json");

                textBox1.Text = course.CourseName;
                trackBar1.Value = course.AudienceAge;

                if (comboBox1.Items.Contains(course.Difficulty))
                    comboBox1.SelectedItem = course.Difficulty;

                numericUpDown1.Value = course.Lectures;
                numericUpDown2.Value = course.Labs;

                if (course.ControlType == "Экзамен")
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;

                dateTimePicker1.Value = course.StartDate;
                textBox2.Text = course.Teacher.FullName;
                textBox3.Text = course.Teacher.Department;

                listBox1.Items.Clear();
                if (course.LiteratureList != null)
                {
                    foreach (var item in course.LiteratureList)
                        listBox1.Items.Add(item);
                }

                MessageBox.Show("Данные загружены из course.json");      
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Для рассчета бюджета
            int lectures = (int)numericUpDown1.Value;
            int labs = (int)numericUpDown2.Value;
            int age = trackBar1.Value;
            int budget = lectures * 100 + labs * 50;

            textBox4.Text = budget.ToString();
            MessageBox.Show($"Бюджет рассчитан: {budget}");

        }

        private void label1_Click(object sender, EventArgs e) { /* Название курса */ }
        private void textBox1_TextChanged(object sender, EventArgs e) { /* Ввод название курса */ }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { /* Выбор сложности курса */ }
        private void label4_Click(object sender, EventArgs e) { /* Сложность курса */ }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { /* Выбор количества лекций */ }
        private void groupBox1_Enter(object sender, EventArgs e) { /* Вид контроля */ }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { /* Календарь */ }
        private void trackBar1_Scroll(object sender, EventArgs e) { /* Выбор возраста */ }
        private void label8_Click(object sender, EventArgs e) { /* Возраст аудитории */ }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { /* Выбор литературы */ }
        private void label2_Click(object sender, EventArgs e) { /* ФИО преподавателя */ }
        private void textBox2_TextChanged(object sender, EventArgs e) { /* Ввод ФИО */ }
        private void textBox3_TextChanged(object sender, EventArgs e) { /* Ввод кафедры */ }
        private void label3_Click(object sender, EventArgs e) { /* Кафедра */ }
        private void label5_Click(object sender, EventArgs e) { /* Количество лекций */ }
        private void label6_Click(object sender, EventArgs e) { /* Количество лабораторных */ }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e) { /* Ввод количества лабораторных */ }
        private void label7_Click(object sender, EventArgs e) { /* Дата начала курса */ }
        private void textBox4_TextChanged(object sender, EventArgs e) { /* Вывод рассчета бюджета */ }
    }


}
