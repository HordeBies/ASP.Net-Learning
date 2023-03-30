using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewsExample.Models
{
    public class Person : IValidatableObject
    {
        [Display(Name= "Person Name")]
        [Required(ErrorMessage ="{0} can't be null or empty")]
        [StringLength(40,MinimumLength = 3,ErrorMessage ="{0} should be between {2} and {1} characters")]
        [RegularExpression("^[A-Za-z .]+$",ErrorMessage ="{0} should contain only alphabets, space and dot (.)")]
        public string? Name { get; set; }

        //[RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Not a valid email adress")]
        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email adress")]
        public string? Email { get; set; }
        
        [Phone(ErrorMessage = "Not a valid phone number")]
        public string? Phone { get; set; }
        
        [Required]
        public string? Password { get; set; }
        
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        
        [Range(0,999.99,ErrorMessage ="{0} should be between ${1} and ${2}")]
        public double? Price { get; set; }
        
        [Url]
        public string? WebPage { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [ValidateNever]
        public bool neverValidated { get; set; }
        
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [BindNever] //[bindnever] doesnt work with [frombody]
        public int? Age { get; set; }

        public List<string?> Tags { get; set; } = new();

        public override string ToString()
        {
            return $"Person object - Name: {Name}, Email: {Email}, Phone: {Phone}, Password: {Password}, Confirm Password: {ConfirmPassword}, Price: ${Price}, WebPage: {WebPage}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DateOfBirth.HasValue == false && Age.HasValue == false)
            {
                yield return new("Either of Date of Birth or Age must be supplied", new[] { nameof(Age) });
            }
        }
    }
}
