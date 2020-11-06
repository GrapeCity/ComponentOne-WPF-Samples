using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Validation
{
    // custom validator can be used to provide item-level validation
    // or anything that is not covered by the standard validators
    public static class CustomProductValidator
    {
        public static ValidationResult CheckPriceCost(ProductBase product)
        {
            if (product.Price < product.Cost)
            {
                List<string> properties = new List<string>() { "Price", "Cost" };
                return new ValidationResult("Price must be >= Cost.", properties);
            }
            return ValidationResult.Success;
        }
    }
}
