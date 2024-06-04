using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GapAnalysis
{
    /// <summary>
    /// Fields on the form. 
    /// The Value property may be set directly or it may be calculated
    /// by a formula.
    /// </summary>
    public class FormField : BaseEditableObject
    {
        FormModel _model;
        string _section, _id, _description, _formula;
        bool _isString;
        object _value;

        public FormField(FormModel model, string id, string section, string description, string formula)
        {
            _model = model;
            _id = id.TrimStart('*');
            _isString = id.StartsWith("*");
            _section = section;
            _description = description;
            if (!string.IsNullOrEmpty(formula))
            {
                if (formula.StartsWith("{{") && formula.EndsWith("}}"))
                {
                    _formula = formula.Trim('{', '}');
                }
                else
                {
                    _value = _isString ? (object)formula : (object)double.Parse(formula);
                }
            }
        }
        public string Section
        {
            get { return _section; }
        }
        public string ID
        {
            get { return _id; }
        }
        public bool IsString
        {
            get { return _isString; }
        }
        public string Description
        {
            get { return _description; }
        }
        public string Formula
        {
            get { return _formula; }
        }
        public object Value
        {
            get { return _value; }
            set
            {
                // convert values to double
                if (value != null)
                {
                    var text = value.ToString();
                    if (!this.IsString && text.Length > 0)
                    {
                        double dbl;
                        if (!double.TryParse(text, out dbl))
                        {
                            throw new Exception("Please enter a numeric value.");
                        }
                        if (dbl < 0)
                        {
                            throw new Exception("Please enter a positive value.");
                        }
                        value = dbl;
                    }
                }

                // set the value
                if (SetValue(ref _value, value, "Value"))
                {
                    _model.UpdateFieldValues();
                }
            }
        }
    }
}
