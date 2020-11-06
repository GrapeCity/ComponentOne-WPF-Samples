using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace InputPanelSample
{
    public class CustomValidator
    {
        public static ValidationResult ValidatePhoneNumber(string PhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                return new ValidationResult("Phone number cannot be none.", new List<string>() { "Phone" });
            }
            else if (PhoneNumber.Length > 12 || PhoneNumber.Length < 9)
            {
                return new ValidationResult("Phone number should be more tan 8 digit and less than 12 digit.", new List<string>() { "Phone" });
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
