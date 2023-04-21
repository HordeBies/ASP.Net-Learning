using Assignment_ModelBindingAndValidations.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Assignment_ModelBindingAndValidations.CustomValidators
{
    public class SumAttribute : ValidationAttribute
    {
        public string OtherProperty { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="otherProperty">InvoicePrice</param>
        public SumAttribute(string otherProperty) : base(errorMessage: "Sum of {0} should be equal to {1}")
        {
            OtherProperty = otherProperty;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var products = value as List<Product>;
                var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
                if (otherPropertyInfo != null)
                {
                    var invoicePrice = Convert.ToInt32(otherPropertyInfo.GetValue(validationContext.ObjectInstance));
                    if(products == null || invoicePrice != products.Sum(x => x.Price * x.Quantity))
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new string[] { validationContext.MemberName });
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            return null;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherProperty);
        }
    }
}
