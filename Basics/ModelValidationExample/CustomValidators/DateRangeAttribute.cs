using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace ModelValidationExample.CustomValidators
{
    /// <summary>
    /// Compares the date with another date
    /// </summary>
    public class DateRangeAttribute : ValidationAttribute
    {
        public string OtherProperty { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="otherProperty">FromDate</param>
        public DateRangeAttribute(string otherProperty) : base(errorMessage:"{0} should be later than {1}")
        {
            OtherProperty = otherProperty;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var toDate = Convert.ToDateTime(value);
                var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
                if(otherPropertyInfo != null)
                {
                    var fromDate = Convert.ToDateTime(otherPropertyInfo.GetValue(validationContext.ObjectInstance));
                    if (fromDate > toDate)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new string[] { OtherProperty, validationContext.MemberName });
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
