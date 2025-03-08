using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleAppWF
{
    public class Course
    {
        public string CourseName { get; set; }
        public int AudienceAge { get; set; }
        public string Difficulty { get; set; }
        public int Lectures { get; set; }
        public int Labs { get; set; }
        public string ControlType { get; set; }
        public DateTime StartDate { get; set; }
        public Teacher Teacher { get; set; }
        public List<Literature> LiteratureList { get; set; }
    }
    
    public class Teacher
    {
        public string FullName { get; set; }
        public string Department { get; set; }
    }

    public class Literature
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public override string ToString()
        {
            return $"{Title} / {Author} ({Year})";
        }
    }
}
