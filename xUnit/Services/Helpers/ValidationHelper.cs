using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        public static void ModelValidation(object obj)
        {
            ValidationContext context = new(obj);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(obj, context, results, true)) throw new ArgumentException(results.FirstOrDefault()?.ErrorMessage, results.FirstOrDefault()?.MemberNames.FirstOrDefault());
        }
    }
}
