using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Validation
{
    /// <summary>
    /// Class that implements IDataErrorInfo and raises validation errors when 
    /// users set Price, Cost, or Weight properties to negative values.
    /// </summary>
    public class ProductDataErrorInfo :
        ProductBase,
        IDataErrorInfo
    {
        #region ** IDataErrorInfo

        string IDataErrorInfo.Error
        {
            get
            {
                // perform item-level validation: Price must be > Cost
                if (Price <= Cost)
                {
                    return string.Format("Price must be > Cost ({0:c2})", Cost);
                }
                return null;
            }
        }
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case "Price":
                        if (Price <= 0)
                        {
                            error = "Price must be > 0";
                        }
                        else if (Price <= Cost)
                        {
                            error = string.Format("Price must be > Cost ({0:c2})", Cost);
                        }
                        break;
                    case "Cost":
                        if (Cost <= 0)
                        {
                            error = "Cost must be > 0";
                        }
                        if (Price <= Cost)
                        {
                            error = string.Format("Cost must be < Price ({0:c2})", Price);
                        }
                        break;
                    case "Volume":
                        if (Volume <= 0)
                        {
                            error = "Volume must be > 0";
                        }
                        break;
                    case "Rating":
                        if (Rating < 0 || Rating > 5)
                        {
                            error = "Rating must be between 0 and 5";
                        }
                        break;
                }
                return error;
            }
        }

        #endregion
    }
}
