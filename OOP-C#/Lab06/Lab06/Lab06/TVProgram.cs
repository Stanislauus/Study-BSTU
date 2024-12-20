using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06
{
    public abstract partial class TVProgram : IShowInfo
    {
        public string Title { get; set; }
        public int Duration { get; set; }

        //private Director _director; 
        public Director Director { get; set; }//композиция




        //------------------------------1)---------------------------
        public Schedule Schedule { get; set; }//композиция
        public Type Type { get; set; }
        //-----------------------------------------------------------




        public TVProgram(string title, int duration, Director director, Schedule schedule, Type type)
        {
            Title = title;
            Duration = duration;

            Director = director;//композиция
            Schedule = schedule;
            Type = type;
        }
    }
}
