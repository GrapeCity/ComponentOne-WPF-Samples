using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataAttributes
{
    public class Customer: IDataErrorInfo
    {
        //Display
        //  Name - The full name to use as a label for the field, such as "Social Security Number".
        //  ** ShortName - A compressed or abbreviated version of the name to use as a label for the field, such as "SSN". This value is used by the DataGrid control.
        //  Description - The text that appears in the user interface to provide more information about the field. The text can be used as a tool tip.
        //  Prompt - The text that is intended as a water mark for the field to suggest a user action.
        //  GroupName - A name that is used when grouping data.
        //  Order - A number that indicates the position of a user-interface element that represents the field, relative to other fields. This value is used by the DataGrid control to establish the column ordering.
        //  AutoGenerateField - A value that indicates whether the field is included in the automatic generation of user-interface elements such as columns. This value is used by the DataGrid control.
        //  AutoGenerateFilter - A value that indicates whether a user-interface element that is automatically generated for the field should include filtering capabilities.
        //  ResourceType - Where to get the text attributes
        //
        //DisplayFormat
        //  ApplyFormatInEditMode	Gets or sets a value that indicates whether the formatting string that is specified in the DataFormatString property is applied to the member when in edit mode.
        //  ConvertEmptyStringToNull	Gets or sets a value that indicates whether empty string values ("") are automatically converted to null.
        //  DataFormatString	Gets or sets the string value that specifies how to display values for the member.
        //  NullDisplayText	Gets or sets the text to display for a member when the value of the member is null.
        //  
        //Editable
        //  AllowEdit	Gets a value that indicates whether a client application should allow users to change the value of the property.
        //  AllowInitialValue	Gets or sets a value that indicates whether users should be able to set a value for the property when adding a new record in the data set.
        //
       
        [Display(Order = 4, Name = "Email Address Name", ShortName = "wazoo!", Description = "An email address is needed to provide notifications about the order.")]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "Email address must be between 3 and 8 characters long.")]
        public string EmailAddress { get; set; }

        [Display(Order = 3, ResourceType = typeof(Resource1), Name = "LName", Description = "LNameDescription")]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 8 characters long.")]
        public string LastName { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        
        [Display(Order = 2, Name="ListPrice", Description = "This is the description for List Price!")]
        [Editable(false)]
        public Decimal ListPrice { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Not specified.")]
        [Display(Order = 1, Name="Size", AutoGenerateField = false)]
        public string Size { get; set; }

        [Editable(false)]
        [Display(Order = 0, Name = "The Thing!")]
        public string Something { get; set; }

        [Editable(false)]
        [Display(Name = "OrOther", AutoGenerateField = false)]
        public string OrOther { get; set; }

        [Editable(false)]
        [Display(Order = 123, Name = "Order123")]
        public string Order123 { get; set; }

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "EmailAddress":
                        return ValidateProperty(EmailAddress, columnName);
                    case "LastName":
                        return ValidateProperty(LastName, columnName);
                }
                return null;
            }
        }

        protected string ValidateProperty(object value, string propertyName)
        {
            var info = GetType().GetProperty(propertyName);
            IEnumerable<string> errorInfos =
                  (from va in info.GetCustomAttributes(true).OfType<ValidationAttribute>()
                   where !va.IsValid(value)
                   select va.FormatErrorMessage(string.Empty)).ToList();

            if (errorInfos.Count() > 0)
            {
                return errorInfos.FirstOrDefault<string>();
            }
            return null;
        }
    }
}
