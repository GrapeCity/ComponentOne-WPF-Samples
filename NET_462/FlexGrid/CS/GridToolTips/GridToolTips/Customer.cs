using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GridToolTips
{
    /// <summary>
    /// Simple data class.
    /// </summary>
    public class Customer
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
