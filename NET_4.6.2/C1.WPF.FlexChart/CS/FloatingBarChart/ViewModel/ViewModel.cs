using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace FloatingBarChart
{
    public class ViewModel
    {
        List<SubjectScoreRange> scores;
        static List<Phase> phases;
        static List<Task> _tasks;

        public List<SubjectScoreRange> SubjectScoreRanges
        {
            get
            {
                if (scores == null)
                {
                    scores = new List<SubjectScoreRange>();
                    scores.Add(new SubjectScoreRange() { Name = "English", ClassAHigh = 99, ClassALow = 56, ClassBHigh = 96, ClassBLow = 67 });
                    scores.Add(new SubjectScoreRange() { Name = "Mathematics", ClassAHigh = 100, ClassALow = 75, ClassBHigh = 98, ClassBLow = 65 });
                    scores.Add(new SubjectScoreRange() { Name = "Reading", ClassAHigh = 91, ClassALow = 32, ClassBHigh = 96, ClassBLow = 67 });
                    scores.Add(new SubjectScoreRange() { Name = "Science", ClassAHigh = 85, ClassALow = 21, ClassBHigh = 92, ClassBLow = 51 });
                    scores.Add(new SubjectScoreRange() { Name = "Writing", ClassAHigh = 92, ClassALow = 47, ClassBHigh = 95, ClassBLow = 61 });
                }
                return scores;
            }
        }

        public static List<Phase> ReleasePhases
        {
            get
            {
                if (phases == null)
                {
                    phases = new List<Phase>();
                    phases.Add(new Phase() { Name = "Distribute", End = 11, Start = 10, Color = Color.FromArgb(255, 210, 192, 135) });
                    phases.Add(new Phase() { Name = "Test, Accept", End = 10, Start = 8, Color = Color.FromArgb(255, 248, 195, 217) });
                    phases.Add(new Phase() { Name = "Development", End = 8, Start = 2, Color = Color.FromArgb(255, 252, 201, 137) });
                    phases.Add(new Phase() { Name = "Design", End = 2, Start = 1, Color = Color.FromArgb(255, 177, 220, 182) });
                    phases.Add(new Phase() { Name = "Plan", End = 1, Start = 0, Color = Color.FromArgb(255, 171, 208, 237) });
                }
                return phases;
            }
        }

        public static List<Task> ChartData
        {
            get
            {
                if (_tasks == null)
                {
                    var year = DateTime.Now.Year;
                    _tasks = new List<Task>() {
                        new Task("Task1", new DateTime(year, 1, 1), new DateTime(year, 3, 31), null, 100),
                        new Task("Task2", new DateTime(year, 4, 1), new DateTime(year, 4, 30), "Task1", percent: 100),
                        new Task("Task3", new DateTime(year, 5, 1), new DateTime(year, 7, 31), "Task2", percent: 75),
                        new Task("Task4", new DateTime(year, 4, 1), new DateTime(year, 7, 31), "Task1", percent: 33),
                        new Task("Task5", new DateTime(year, 8, 1), new DateTime(year, 9, 30), "Task3,Task4", percent: 0),
                        new Task("Task6", new DateTime(year, 10, 1), new DateTime(year, 12, 31), "Task1,Task5", percent: 0),
                        new Task("Task7", new DateTime(year, 1, 1), new DateTime(year, 12, 31), null, percent: 50)
                    };
                }
                return _tasks;
            }
        }
    }
}
