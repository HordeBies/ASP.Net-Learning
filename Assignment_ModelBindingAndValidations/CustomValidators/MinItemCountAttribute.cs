using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Assignment_ModelBindingAndValidations.CustomValidators
{
    public class MinItemCountAttribute : ValidationAttribute
    {
        private readonly int count;
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="Count">at least amount of items</param>
        public MinItemCountAttribute(int Count) : base(errorMessage: "List {0} should contain at least {1} item.")
        {
            this.count = Count;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var products = value as List<object>;
                if (products?.Count < count)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new string[] {validationContext.MemberName });
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return null;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name,count);
        }
    }
}
