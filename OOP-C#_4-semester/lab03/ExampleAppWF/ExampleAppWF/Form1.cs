using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ExampleAppWF
{
    
    public partial class Form1: Form
    {
        private Panel toolStripTriggerPanel;
        private MenuStrip menuStrip;
        private ToolStrip toolStrip;

        private FormState currentState; // Добавляем переменную для текущего состояния

        private StatusStrip statusStrip;         // Контейнер для строки состояния
        private ToolStripStatusLabel statusLabel; // Текстовая метка в строке состояния
        private Timer updateTimer;               // Таймер для обновления времени
        private string lastAction = "Программа запущена"; // Хранит последнее действие

        //для работы с историей
        private Stack<FormState> historyBack = new Stack<FormState>();
        private Stack<FormState> historyForward = new Stack<FormState>();
        private bool isToolbarPinned = false;
        private bool isRestoringState = false;


        private const int TOOLSTRIP_HEIGHT = 25;
        private bool isMouseOverToolStrip = false;

        public Form1()
        {
            InitializeComponent();
            InitializeStatusStrip();
            InitializeMenu();      // Сначала инициализируем меню
            InitializeToolStrip();     // Затем menuStrip
            InitializeTimer();

            this.Load += Form1_Load;

            // Инициализируем текущее состояние
            currentState = GetCurrentState();

            textBox1.TextChanged += (s, e) => SaveState();
            textBox2.TextChanged += (s, e) => SaveState();
            textBox3.TextChanged += (s, e) => SaveState();
            textBox4.TextChanged += (s, e) => SaveState();
            numericUpDown1.ValueChanged += (s, e) => SaveState();
            numericUpDown2.ValueChanged += (s, e) => SaveState();
            trackBar1.Scroll += (s, e) => SaveState();
            dateTimePicker1.ValueChanged += (s, e) => SaveState();
            comboBox1.SelectedIndexChanged += (s, e) => SaveState();
            listBox1.SelectedIndexChanged += (s, e) => SaveState(); // Для изменений в списке литературы
            //groupBox1
            radioButton1.CheckedChanged += (s, e) => SaveState();
            radioButton2.CheckedChanged += (s, e) => SaveState();

            

        }

        //--------------------------------3lab-------------------------

        private void Form1_Load(object sender, EventArgs e)
        {
            menuStrip.Location = new Point(0, toolStripTriggerPanel.Height);
            menuStrip.Height = 24;        
            SaveState();

            UpdateStatus("Очистка списка литературы");
        }

        private void InitializeToolStrip()
        {
            toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                Visible = isToolbarPinned,
                Height = TOOLSTRIP_HEIGHT,
                GripStyle = ToolStripGripStyle.Hidden
            };

            // Кнопка "Очистить"
            ToolStripButton clearButton = new ToolStripButton("Очистить")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            clearButton.Click += ClearButton_Click;

            // Кнопка "Удалить"
            ToolStripButton deleteButton = new ToolStripButton("Удалить список литературы")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            deleteButton.Click += DeleteButton_Click;

            // Кнопки навигации
            ToolStripButton backButton = new ToolStripButton("Назад")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            backButton.Click += BackButton_Click;

            ToolStripButton forwardButton = new ToolStripButton("Вперед")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            forwardButton.Click += ForwardButton_Click;

            // Кнопка закрепления
            ToolStripButton pinButton = new ToolStripButton("Закрепить")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                CheckOnClick = true
            };
            pinButton.CheckedChanged += PinButton_CheckedChanged;

            toolStrip.Items.AddRange(new ToolStripItem[]
            {
                clearButton,
                new ToolStripSeparator(),
                deleteButton,
                new ToolStripSeparator(),
                backButton,
                forwardButton,
                new ToolStripSeparator(),
                pinButton
            });

            this.Controls.Add(toolStrip);
            toolStrip.Dock = DockStyle.Top;


            toolStrip.MouseEnter += (s, e) =>
            {
                isMouseOverToolStrip = true;
                if (!isToolbarPinned)
                {
                    toolStrip.Visible = true;
                    toolStrip.BringToFront();
                }
            };

            toolStrip.MouseLeave += (s, e) =>
            {
                isMouseOverToolStrip = false;
                if (!isToolbarPinned)
                {
                    HideToolStrip();
                }
            };


            toolStrip.Visible = isToolbarPinned; // Убедитесь, что состояние синхронизировано
            toolStrip.BringToFront(); // Для правильного отображения поверх других элементов


            // Добавляем триггерную панель
            toolStripTriggerPanel = new Panel
            {
                Height = 5,
                Dock = DockStyle.Top,
                BackColor = Color.LightGray, // Цвет подсветки
                Cursor = Cursors.Hand,
                Visible = !isToolbarPinned // Показываем только когда панель не закреплена
            };

            this.Controls.Add(toolStripTriggerPanel);
            toolStripTriggerPanel.BringToFront();

            // Обработчики событий для всей области
            toolStripTriggerPanel.MouseEnter += (s, e) => ShowToolStrip();
            menuStrip.MouseEnter += (s, e) => ShowToolStrip();

            toolStripTriggerPanel.MouseLeave += (s, e) => HideToolStrip();
            menuStrip.MouseLeave += (s, e) => HideToolStrip();

            // Обновляем позиционирование
            //UpdateLayout();
        }



        private void ShowToolStrip()
        {
            if (!isToolbarPinned)
            {
                toolStrip.Visible = true;
                toolStrip.BringToFront();
                toolStripTriggerPanel.Visible = false;
            }
        }

        private void HideToolStrip()
        {
            if (!isToolbarPinned && !toolStrip.ClientRectangle.Contains(toolStrip.PointToClient(Cursor.Position)))
            {
                toolStrip.Visible = false;
                toolStripTriggerPanel.Visible = true;
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            SaveState();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            trackBar1.Value = 18;
            dateTimePicker1.Value = DateTime.Now;

            UpdateStatus("Очистка всех полей");
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            SaveState();
            listBox1.Items.Clear(); // Очищаем только список литературы

            UpdateStatus("Очистка списка литературы");
        }

        //----------------------------------------------------------------------------------------------------------------

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (historyBack.Count > 0)
            {
                // Сохраняли текущее (новое) состояние
                //historyForward.Push(GetCurrentState());
                historyForward.Push(currentState);
                RestoreState(historyBack.Pop());

                UpdateStatus("Возвращение на одно действие назад");
            }
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            if (historyForward.Count > 0)
            {
                //historyBack.Push(GetCurrentState());
                historyBack.Push(currentState);// Сохраняем текущее состояние перед восстановлением старого
                RestoreState(historyForward.Pop());

                UpdateStatus("Возвращение на одно действие вперед");
            }
        }

        private void PinButton_CheckedChanged(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            isToolbarPinned = button.Checked;
            button.Text = isToolbarPinned ? "Открепить" : "Закрепить";
            button.ToolTipText = button.Text;

            //toolStripTriggerPanel.Visible = !isToolbarPinned;
            toolStrip.Visible = true;
            //UpdateLayout();
        }

        // Методы для работы с историей
        private FormState GetCurrentState()
        {
            return new FormState
            {
                CourseName = textBox1.Text,
                TeacherName = textBox2.Text,
                Department = textBox3.Text,
                Budget = textBox4.Text,
                LiteratureItems = listBox1.Items.Cast<object>().ToList(),
                SearchResults = listBox2.Items.Cast<object>().ToList(),
                Lectures = (int)numericUpDown1.Value,
                Labs = (int)numericUpDown2.Value,
                AudienceAge = trackBar1.Value,
                Date = dateTimePicker1.Value,
                ControlType = selectedControlType
            };
        }



        private void SaveState()
        {
            if (isRestoringState) return; // Не сохранять, если идет восстановление

            //var currentState = GetCurrentState();
            //historyBack.Push(GetCurrentState());  // Сохраняли текущее состояние
            //historyForward.Clear();

            // Сохраняем текущее состояние как предыдущее
            historyBack.Push(currentState);
            // Обновляем текущее состояние
            currentState = GetCurrentState();
            historyForward.Clear();
        }

        private void RestoreState(FormState state)
        {
            isRestoringState = true; // Включаем флаг
            textBox1.Text = state.CourseName;
            textBox2.Text = state.TeacherName;
            textBox3.Text = state.Department;
            textBox4.Text = state.Budget;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(state.LiteratureItems.ToArray());
            listBox2.Items.Clear();
            listBox2.Items.AddRange(state.SearchResults.ToArray());
            numericUpDown1.Value = state.Lectures;
            numericUpDown2.Value = state.Labs;
            trackBar1.Value = state.AudienceAge;
            dateTimePicker1.Value = state.Date;

            if (state.ControlType == "Экзамен")
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;

            currentState = state; // Обновляем текущее состояние

            isRestoringState = false; // Выключаем флаг
        }

        //--------------------------------------------------------------------------------------------------------------

        private void InitializeMenu()
        {
            if (menuStrip == null)
            {
                menuStrip = new MenuStrip();
            }


            ToolStripMenuItem searchMenu = new ToolStripMenuItem("Поиск");

            ToolStripMenuItem fullSearchItem = new ToolStripMenuItem("По полному соответствию");
            fullSearchItem.Click += FullSearchItem_Click;
            searchMenu.DropDownItems.Add(fullSearchItem);

            ToolStripMenuItem regexSearchItem = new ToolStripMenuItem("По регулярному выражению");
            regexSearchItem.Click += RegexSearchItem_Click;
            searchMenu.DropDownItems.Add(regexSearchItem);

            menuStrip.Items.Add(searchMenu);


            ToolStripMenuItem sortMenu = new ToolStripMenuItem("Сортировка по");
            
            ToolStripMenuItem sortByName = new ToolStripMenuItem("Названию курса");
            sortByName.Click += SortByName_Click;
            sortMenu.DropDownItems.Add(sortByName);

            ToolStripMenuItem sortByAge = new ToolStripMenuItem("Возрасту аудитории");
            sortByAge.Click += SortByAge_Click;
            sortMenu.DropDownItems.Add(sortByAge);

            ToolStripMenuItem sortByFullName = new ToolStripMenuItem("ФИО преподавателя");
            sortByFullName.Click += SortByFullName_Click;
            sortMenu.DropDownItems.Add(sortByFullName);

            menuStrip.Items.Add(sortMenu);


            ToolStripMenuItem saveMenu = new ToolStripMenuItem("Сохранить результаты поиска/сортировки");
            saveMenu.Click += SaveMenu_Click;

            menuStrip.Items.Add(saveMenu);


            ToolStripMenuItem aboutMenu = new ToolStripMenuItem("О программе");
            aboutMenu.Click += AboutMenu_CLick;

            menuStrip.Items.Add(aboutMenu);


            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

        }
              
        private void FullSearchItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Запуск поиска по полному соответствию");

            string searchTerm = Prompt.ShowDialogWindow("Введите поисковый запрос: ", "Поиск по полному соответствию");

            if (string.IsNullOrWhiteSpace(searchTerm)) return;

            listBox2.Items.Clear();

            foreach (var course in coursesList)
            {
                if (course.CourseName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    listBox2.Items.Add($"Курс: {course.CourseName}, Возраст: {course.AudienceAge}, Преподаватель: {course.Teacher.FullName}");
                }
            }

            UpdateStatus("Поиск по полному соответствию");
        }

        private void RegexSearchItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Запуск поиска с использованием регулярных выражений");

            string searchRegex = Prompt.ShowDialogWindow("Введите регулярное выражение", "Поиск по регулярным выражениям");

            if (string.IsNullOrWhiteSpace(searchRegex)) return;

            listBox2.Items.Clear();

            foreach (var course in coursesList)
            {
                if (Regex.IsMatch(course.CourseName, searchRegex))
                {
                    listBox2.Items.Add($"Курс: {course.CourseName}, Возраст: {course.AudienceAge}, Преподаватель: {course.Teacher.FullName}");
                }
            }

            UpdateStatus("Поиск по регулярному выражению");
        }


        /// <summary>
        /// 
        /// </summary>
        /// 
        //private List<Teacher> teachersList = new List<Teacher>();
        private void SortByFullName_Click(object obj, EventArgs e)
        {
            if (coursesList.Count == 0)
            {
                MessageBox.Show("Список курсов пуст. Загрузите данные или добавьте курс");
                return;
            }

            // var sortedCourses = teachersList.OrderBy(name => name.FullName).ToList();
            var sortedCourses = coursesList.OrderBy(course => course.Teacher.FullName).ToList();

            listBox2.Items.Clear();

            foreach (var course in sortedCourses)
            {
                listBox2.Items.Add($"Курс: {course.CourseName}, Возраст: {course.AudienceAge}, Преподаватель: {course.Teacher.FullName}");
            }

            UpdateStatus("Сортировка по фио");
        }



        private void SortByName_Click(object obj, EventArgs e)
        {
            if (coursesList.Count == 0)
            {
                MessageBox.Show("Список курсов пуст. Загрузите данные или добавьте новый курс.");
                return;
                
            }

            //MessageBox.Show("Сортировка по названию курса");
            //LINQ
            var sortedCourses = coursesList.OrderBy(course => course.CourseName).ToList();


            listBox2.Items.Clear();

            foreach (var course in sortedCourses)
            {
                listBox2.Items.Add($"Курс: {course.CourseName}, Возраст: {course.AudienceAge}, Преподаватель: {course.Teacher.FullName}");
            }

            UpdateStatus("Сортировка по названию");
        }

        private void SortByAge_Click(object obj, EventArgs e)
        {
            if (coursesList.Count == 0)
            {
                MessageBox.Show("Список курсов пуст. Загрузите данные или добавьте новый курс.");
                return;

            }

            //MessageBox.Show("Сортировка по возрасту аудитории");
            //LINQ
            var sortedCourses = coursesList.OrderBy(course => course.AudienceAge).ToList();

            listBox2.Items.Clear();

            foreach (var course in sortedCourses)
            {
                listBox2.Items.Add($"Курс: {course.CourseName}, Возраст: {course.AudienceAge}, Преподаватель: {course.Teacher.FullName}");
            }

            UpdateStatus("Сортировка по возрасту");
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Сохранение результатов поиска/сортировки"); 
            if (listBox2.Items.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения.");
                return;
            }

            List<Course> searchResults = new List<Course>();

            foreach (var item in listBox2.Items)
            {
                string[] parts = item.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Course course = new Course
                {
                    CourseName = parts[0].Replace("Курс: ", "").Trim(),
                    AudienceAge = int.Parse(parts[1].Replace("Возраст: ", "").Trim()),
                    Teacher = new Teacher
                    {
                        FullName = parts[2].Replace("Преподаватель: ", "").Trim()
                    }
                };

                searchResults.Add(course);
            }
            try
            {
                JsonSerialize.Serialize(searchResults, "search_results.json");
                MessageBox.Show("Результаты поиска/сортировки сохранены в search_results.json");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }

        }

        private void AboutMenu_CLick(object sender, EventArgs e)
        {
            MessageBox.Show("Приложение \"Выбор курса программирования\" \nРазработчик: Серебряный Станислав");

            UpdateStatus("Вывод информации о программе");
        }

        //Для хранения списка курсов
        private List<Course> coursesList = new List<Course>();


        //-------------------------------------------------------------


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

            SaveState();

            UpdateStatus("Добавлена литература в список");
        }
        //Сохранить
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

            //------------------3lab--------------

            //Validation
            var validationResults = Validation.Validate(course);

            if (validationResults.Count > 0)
            {
                string errorMessage = "Ошибки валидации:\n";
                foreach (var result in validationResults)
                {
                    errorMessage += $"- {result.ErrorMessage}\n";
                }
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            coursesList.Add(course);

            UpdateStatus("Курс сохранен");
            //------------------------------------

            try
            {
                JsonSerialize.Serialize(coursesList, "course.json");
                MessageBox.Show("Данные сохранены в course.json");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }
        //Загрузить
        private void button3_Click(object sender, EventArgs e)
        {
            //Для загрузки из JSON
            try
            {
                coursesList = JsonSerialize.Deserialize<List<Course>>("course.json");

                if (coursesList.Count > 0 )
                {
                    //----------------------------3lab---------------------
                    Course course = coursesList.Last();
                    //-----------------------------------------------------

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

                    UpdateStatus("Загрузка данных из файла");
                }
                else
                {
                    MessageBox.Show("Файл course.json пуст.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }
        //Для рассчета бюджета
        private void button4_Click(object sender, EventArgs e)
        {
            //Для рассчета бюджета
            int lectures = (int)numericUpDown1.Value;
            int labs = (int)numericUpDown2.Value;
            int age = trackBar1.Value;
            int budget = lectures * 100 + labs * 50;

            textBox4.Text = budget.ToString();
            MessageBox.Show($"Бюджет рассчитан: {budget}");

            UpdateStatus("Рассчет бюджета выполнен");

        }



        private void UpdateStatus(string action = null)
        {
            if (!string.IsNullOrEmpty(action))
            {
                lastAction = action;
            }

            statusLabel.Text = $"Курсов: {coursesList.Count} | " +
                              $"Последнее действие: {lastAction} | " +
                              $"Дата и время: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
        }
        private void InitializeStatusStrip()
        {
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
            statusStrip.Dock = DockStyle.Bottom;
        }
        private void InitializeTimer()
        {
            updateTimer = new Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += (s, e) => UpdateStatus();
            updateTimer.Start();
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
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) { /*Для вывода результата поиска и сортировки*/}
        private void Form1_Load_1(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
    }


    public class FormState
    {
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string Department { get; set; }
        public string Budget { get; set; }
        public List<object> LiteratureItems { get; set; }
        public List<object> SearchResults { get; set; }
        public int Lectures { get; set; }
        public int Labs { get; set; }
        public int AudienceAge { get; set; }
        public DateTime Date { get; set; }
        public string ControlType { get; set; } //Для RadioButton в groupBox1
    }



}
