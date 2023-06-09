﻿using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO
{
    public class RegisterRequest
    {
        [Required(ErrorMessage ="Name can't be empty")]
        public string PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [Remote(action:"IsEmailNotRegistered", controller:"Account",ErrorMessage = "Email is already taken")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone can't be empty")]
        [Phone(ErrorMessage = "Phone number should be in a proper phone number format")]
        [RegularExpression(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$",ErrorMessage = "Phone number should be in a proper phone number format")] //Phone Data Annotation didn't work with client side validation so I had to use RegularExpression Data Annotation instead with a regular expression I found on the Microsoft docs about Phone Data Annotation (https://referencesource.microsoft.com/#System.ComponentModel.DataAnnotations/DataAnnotations/PhoneAttribute.cs)
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Password can't be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm your password")]
        [Compare(nameof(Password),ErrorMessage ="Passwords don't match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
