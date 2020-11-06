using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation
{
    /// <summary>
    /// Class that implements validation using Data Annotation attributes.
    /// </summary>
    /// <remarks>
    /// This class also implements the IDataErrorInfo interface. 
    /// This would not be required if the sample used RIA services, since
    /// those automatically provide the functionality implemented here.
    /// What this class does is use a Validator to enforce the criteria 
    /// specified as attributes.
    /// </remarks>
    [CustomValidation(typeof(CustomProductValidator), "CheckPriceCost")]
    public class ProductValidationAttributes :
        ProductBase,
        IDataErrorInfo
    {
        [Required()]
        [Display(Name = "Line")]
        public override string Line
        {
            get { return base.Line; }
            set { base.Line = value; }
        }

        [Required()]
        [Display(Name = "Color")]
        public override string Color
        {
            get { return base.Color; }
            set { base.Color = value; }
        }

        [Required()]
        [Display(Name = "Name")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        [Range(0, double.MaxValue)]
        [Display(Name = "Price")]
        public override double? Price
        {
            get { return base.Price; }
            set { base.Price = value; }
        }

        [Range(0, double.MaxValue)]
        [Display(Name = "Cost")]
        public override double? Cost
        {
            get { return base.Cost; }
            set { base.Cost = value; }
        }

        [Range(0, double.MaxValue)]
        [Display(Name = "Weight")]
        public override double? Weight
        {
            get { return base.Weight; }
            set { base.Weight = value; }
        }

        [Range(0, double.MaxValue)]
        [Display(Name = "Volume")]
        public override double? Volume
        {
            get { return base.Volume; }
            set { base.Volume = value; }
        }

        [Display(Name = "Discontinued")]
        public override bool? Discontinued
        {
            get { return base.Discontinued; }
            set { base.Discontinued = value; }
        }

        [Range(0, 5)]
        [Display(Name = "Rating")]
        public override int? Rating
        {
            get { return base.Rating; }
            set { base.Rating = value; }
        }

        #region ** IDataErrorInfo
        string IDataErrorInfo.Error
        {
            get
            {
                // get item-level errors
                return GetError(null);
            }
        }
        string IDataErrorInfo.this[string propName]
        {
            get
            {
                // get property-level errors
                return GetError(propName);
            }
        }
        string GetError(string propName)
        {
            // get validation context
            var ctx = new ValidationContext(this, null, null);

            // validate
            try
            {
                // get property name and value
                if (string.IsNullOrEmpty(propName))
                {
                    Validator.ValidateObject(this, ctx);
                }
                else
                {
                    ctx.MemberName = propName;
                    var value = GetValue(propName);
                    Validator.ValidateProperty(value, ctx);
                }
            }
            catch (Exception x)
            {
                return x.Message;
            }

            // no errors
            return null;
        }
        #endregion
    }

  
}
