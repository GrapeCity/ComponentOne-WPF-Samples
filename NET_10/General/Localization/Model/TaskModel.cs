using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Model
{
    /// <summary>
    /// Model for task list
    /// </summary>
    public class TaskModel
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        required public string Name { get; set; }
        /// <summary>
        /// Gets or sets the start date and time for the event or operation.
        /// </summary>
        required public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the duration of the event or operation.
        /// </summary>
        required public int Duration { get; set; }
        /// <summary>
        /// Gets or sets the percentage of completion for the current operation.
        /// </summary>
        required public double PercentComplete { get; set; }

        /// <summary>
        /// Gets or sets the deadline for completing the task.
        /// </summary>
        public DateTime? Deadline { get; set; }

    }
}
