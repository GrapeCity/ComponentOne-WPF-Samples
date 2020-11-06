using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation
{
    /// <summary>
    /// Class that throws exception when users set Price, Cost, or Weight properties
    /// to negative values.
    /// </summary>
    public class ProductThrow :
        ProductBase
    {
        protected override void SetValue(string p, object value)
        {
            if (p == "Price" && (double?)value <= 0)
            {
                throw new Exception("Price must be > 0.");
            }
            if (p == "Cost" && (double?)value <= 0)
            {
                throw new Exception("Cost must be > 0.");
            }
            if (p == "Weight" && (double?)value <= 0)
            {
                throw new Exception("Weight must be > 0.");
            }
            if (p == "Rating" && ((int?)value < 0 || (int?)value > 5))
            {
                throw new Exception("Rating must be between 0 and 5.");
            }
            base.SetValue(p, value);
        }
    }
}
