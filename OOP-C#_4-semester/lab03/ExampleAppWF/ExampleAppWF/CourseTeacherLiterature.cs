using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExampleAppWF.ValidationAttributes;

namespace ExampleAppWF
{
    public class Course
    {
        [Required(ErrorMessage = "Название курса обязательно.")]
        [StringLength(100, ErrorMessage = "Название курса не должно превышать 100 символов.")]
        public string CourseName { get; set; }

        [Range(0, 100, ErrorMessage = "Возраст адитории должен быть от 1 до 100.")]
        public int AudienceAge { get; set; }
        public string Difficulty { get; set; }

        [Range(10, 50, ErrorMessage = "Количество лекций должно быть от 10 до 50.")]
        public int Lectures { get; set; }

        [Range(10, 50, ErrorMessage = "Количество лабораторных должно быть от 10 до 50.")]
        public int Labs { get; set; }
        public string ControlType { get; set; }

        [ValidationCourseDate]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public Teacher Teacher { get; set; }
        public List<Literature> LiteratureList { get; set; }
    }
    
    public class Teacher
    {
        [Required(ErrorMessage = "ФИО преподавателя обязательно.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "ФИО преподавателя не должно превышать 100 символов.")]
        public string FullName { get; set; }
        public string Department { get; set; }
    }

    public class Literature
    {
        [Required(ErrorMessage = "Название литературы обязательно.")]
        [StringLength(100, ErrorMessage = "Название литературы не должно превышать 100 символов.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Автор обязателен.")]
        [StringLength(100, ErrorMessage = "Имя автора не должно превышать 100 символов.")]
        public string Author { get; set; }

        [Range(1800, 2025, ErrorMessage = "Год издания должен быть между 1800 и 2025.")]
        public int Year { get; set; }
        public override string ToString()
        {
            return $"{Title} / {Author} ({Year})";
        }
    }
}
