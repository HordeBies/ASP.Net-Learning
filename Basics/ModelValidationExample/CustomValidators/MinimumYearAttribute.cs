using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ModelValidationExample.CustomValidators
{
    public class MinimumYearAttribute:ValidationAttribute
    {
        private readonly int year;
        public MinimumYearAttribute(int YearInclusive) : base(errorMessage:"Minimum Year allowed is {1}")
        {
            this.year = YearInclusive;
        }
        public override bool IsValid(object? value)
        {
            if(value != null)
            {
                DateTime date = (DateTime) value;
                if(date.Year >= year)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, year);
        }
    }
}
