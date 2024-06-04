using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Validation
{
    /// <summary>
    /// Class that implements INotifyDataErrorInfo and raises validation errors when 
    /// users set Price, Cost, or Weight properties to negative values.
    /// </summary>
    /// <remarks>
    /// This implementation is based on the one found here:
    /// http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifydataerrorinfo(v=vs.95).aspx
    /// </remarks>
    public class ProductNotifyDataErrorInfo :
        ProductBase,
        INotifyDataErrorInfo
    {

        // validate before setting the value
        protected override void SetValue(string p, object value)
        {
            // set property value
            base.SetValue(p, value);

            // validate
            var valid = IsValid(p, value);

            // item-level validation
            if (Price <= Cost)
            {
                AddError(string.Empty, "Price must be > Cost");
            }
            else
            {
                RemoveError(string.Empty);
            }
        }
        bool IsValid(string propName, object value)
        {
            // assume all is OK
            bool valid = true;
            RemoveError(propName);

            // property-level validation
            switch (propName)
            {
                case "Price":
                    {
                        var p = value as double?;
                        if (p <= 0)
                        {
                            AddError(propName, "Price must be > 0");
                            valid = false;
                        }
                    }
                    break;
                case "Cost":
                    {
                        var c = value as double?;
                        if (c <= 0)
                        {
                            AddError(propName, "Cost must be > 0");
                            valid = false;
                        }
                    }
                    break;
                case "Weight":
                    {
                        var w = value as double?;
                        if (w <= 0)
                        {
                            AddError(propName, "Weight must be > 0");
                            valid = false;
                        }
                    }
                    break;
                case "Rating":
                    {
                        var r = value as int?;
                        if (r < 0 || r > 5)
                        {
                            AddError(propName, "Rating must be between 0 and 5");
                            valid = false;
                        }
                    }
                    break;
            }
            return valid;
        }

        // error dictionary
        Dictionary<String, List<String>> _errors = new Dictionary<string, List<string>>();
        void AddError(string propName, string error)
        {
            if (!_errors.ContainsKey(propName))
            {
                _errors[propName] = new List<string>();
            }

            if (!_errors[propName].Contains(error))
            {
                _errors[propName].Insert(0, error);
                RaiseErrorsChanged(propName);
            }
        }
        void RemoveError(string propName)
        {
            if (_errors.ContainsKey(propName))
            {
                _errors.Remove(propName);
                RaiseErrorsChanged(propName);
            }
        }
        void RaiseErrorsChanged(string propName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propName));
            }
        }

        #region ** INotifyDataErrorInfo
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        IEnumerable INotifyDataErrorInfo.GetErrors(string propName)
        {
            if (propName == null)
            {
                propName = string.Empty;
            }
            if (!_errors.ContainsKey(propName))
            {
                return null;
            }
            return _errors[propName];
        }
        bool INotifyDataErrorInfo.HasErrors
        {
            get { return _errors.Count > 0; }
        }
        #endregion
    }
}
