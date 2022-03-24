using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Themes
{
    public class Employee
    {
        public string HeaderText = "Employees";

        [Browsable(false)]
        public int EmployeeID { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [Browsable(false)]
        public string TitleOfCourtesy { get; set; }
        [DisplayName("Date of Birth")]
        [Range(typeof(DateTime), "09/19/1957", "01/27/1986")]
        public DateTime BirthDate { get; set; }
        [DisplayName("Employment date")]
        [Range(typeof(DateTime), "04/01/2012", "11/15/2014")]
        public DateTime HireDate { get; set; }
        [DisplayName("Employment ratio")]
        [Range(typeof(int), "0", "1000")]
        public int Ratio { get; set; }
        [Browsable(false)]
        public string Address { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [Browsable(false)]
        public string Region { get; set; }
        [Browsable(false)]
        public string PostalCode { get; set; }
        [DisplayName("Country")]
        public string Country { get; set; }
        [Browsable(false)]
        public string HomePhone { get; set; }
        [Browsable(false)]
        public string Extension { get; set; }
        [Browsable(false)]
        public byte[] Photo { get; set; }
        [Browsable(false)]
        public string Notes { get; set; }
        [Browsable(false)]
        public int? ReportsTo { get; set; }

        [Browsable(false)]
        public string Color { get; set; }

        [Browsable(false)]
        public bool Sex { get; set; }
    }
}