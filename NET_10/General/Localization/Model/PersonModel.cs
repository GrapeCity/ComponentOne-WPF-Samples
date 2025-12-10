using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Model
{
    /// <summary>
    /// Represents a person with basic identifying information, including name, age, address, and birth details.
    /// </summary>
    /// <remarks>This model is typically used to store and transfer information about an individual.  The <see
    /// cref="BirthDate"/> property may be excluded from display in certain contexts,  as indicated by its
    /// metadata.</remarks>
    public class PersonModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string BloodType { get; set; }
        public string MaritalStatus { get; set; }
        public string NationalID { get; set; }
        public string Education { get; set; }
        public string Nationality { get; set; }
        public string EmailAddress { get; set; }
        public string Gender { get; set; }
        public int ContactNumber { get; set; }
        public string Address { get; set; }
        public string Residence { get; set; }
        public string Born { get; set; }
        //[Display(AutoGenerateField=false)]
        public DateTime BirthDate { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
    }
}
