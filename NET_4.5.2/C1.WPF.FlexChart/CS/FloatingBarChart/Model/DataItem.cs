using System;
using System.Windows.Media;

namespace FloatingBarChart
{
    public class Phase
    {
        public string Name { get; set; }
        public int End { get; set; }
        public int Start { get; set; }
        public Color Color { get; set; }
    }

    public class SubjectScoreRange
    {
        public string Name { get; set; }
        public int ClassAHigh { get; set; }
        public int ClassALow { get; set; }
        public int ClassBHigh { get; set; }
        public int ClassBLow { get; set; }
        public string ClassName { get; set; }
    }

    public class Task
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string Parent { get; set; }
        public double Percent { get; set; }

        public Task(string name, DateTime start, DateTime end, string parent, double percent)
        {
            Name = name;
            Start = start;
            End = end;
            Parent = parent;
            Percent = percent;
        }
    }
}
