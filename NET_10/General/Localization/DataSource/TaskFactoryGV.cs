using Localization.Model;
using System;
using System.Collections.Generic;
//using System.Threading.Tasks;
using C1.GanttView;


namespace Localization.DataSource
{
    /// <summary>
    /// Provides functionality for generating predefined tasks for project management purposes.
    /// </summary>
    /// <remarks>The <see cref="TaskFactoryGV"/> class is designed to create sample task data that can be used
    /// in Gantt chart visualizations  or other project tracking tools. It includes methods for generating collections
    /// of tasks with predefined attributes such as  name, start date, duration, percent completion, and optional
    /// deadlines.</remarks>
    public static class TaskFactoryGV
    {
        /// <summary>
        /// Generates a predefined list of default tasks for project management purposes.
        /// </summary>
        /// <remarks>This method returns a collection of <see cref="TaskModel"/> objects, each
        /// representing a task with  predefined attributes such as name, start date, duration, percent completion, and
        /// optional deadline. The returned tasks are intended to serve as sample data for use in Gantt chart
        /// visualizations or  other project tracking tools.</remarks>
        /// <returns>A <see cref="List{T}"/> of <see cref="TaskModel"/> objects containing the default tasks.</returns>
        public static List<Task> GetDefaultTasks()
        {
            var taskModels = new List<TaskModel>
            {
                new TaskModel { Name = "Requirement Gathering", Start = new DateTime(2022, 4, 5), Duration = 5, PercentComplete = 0.5},
                new TaskModel { Name = "Analysis", Start = new DateTime(2022, 3, 20), Duration = 8, PercentComplete = 0, Deadline = new DateTime(2022, 4, 22) },
                new TaskModel { Name = "Design", Start = new DateTime(2022, 3, 8), Duration = 4, PercentComplete = 0.6 },
                new TaskModel { Name = "Development", Start = new DateTime(2022, 5, 1), Duration = 10, PercentComplete = 0.2 },
                new TaskModel { Name = "Quality Assurance", Start = new DateTime(2022, 4, 16), Duration = 7, PercentComplete = 0.2 },
                new TaskModel { Name = "Deployment and Release", Start = new DateTime(2022, 4, 29), Duration = 1, PercentComplete = 0.5 },
                new TaskModel { Name = "Maintenance", Start = new DateTime(2022, 5, 3), Duration = 4, PercentComplete = 1 },
                new TaskModel { Name = "Enhancements", Start = new DateTime(2022, 5, 11), Duration = 2, PercentComplete = 0.8 },
                new TaskModel { Name = "User Acceptance Testing", Start = new DateTime(2022, 5, 15), Duration = 5, PercentComplete = 0.4 },
                new TaskModel { Name = "Code Review", Start = new DateTime(2022, 4, 25), Duration = 3, PercentComplete = 0.9 },
                new TaskModel { Name = "Security Audit", Start = new DateTime(2022, 5, 20), Duration = 6, PercentComplete = 0.1 },
                new TaskModel { Name = "Performance Optimization", Start = new DateTime(2022, 5, 10), Duration = 4, PercentComplete = 0.3 },
                new TaskModel { Name = "Documentation", Start = new DateTime(2022, 5, 5), Duration = 7, PercentComplete = 0.7 },
                new TaskModel { Name = "Integration Testing", Start = new DateTime(2022, 3, 18), Duration = 6, PercentComplete = 0.6 },
                new TaskModel { Name = "Stakeholder Review", Start = new DateTime(2022, 5, 18), Duration = 2, PercentComplete = 0 },
                new TaskModel { Name = "Bug Fixing Sprint", Start = new DateTime(2022, 5, 25), Duration = 5, PercentComplete = 0.2 },
                new TaskModel { Name = "Production Setup", Start = new DateTime(2022, 5, 30), Duration = 3, PercentComplete = 0 },
                new TaskModel { Name = "Post-Release Monitoring", Start = new DateTime(2022, 6, 5), Duration = 7, PercentComplete = 0 },

            };

            return ConvertToGanttTasks(taskModels);
        }
        private static List<Task> ConvertToGanttTasks(IEnumerable<TaskModel> taskModels)
        {
            var ganttTasks = new List<Task>();
            foreach (var model in taskModels)
            {
                ganttTasks.Add(new Task
                {
                    Name = model.Name,
                    Start = model.Start,
                    Duration = model.Duration,
                    PercentComplete = model.PercentComplete,
                    Deadline = model.Deadline
                });
            }
            return ganttTasks;
        }


    }
}
