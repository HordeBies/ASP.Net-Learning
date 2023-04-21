using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingExample.Models
{
    public class Book
    {
        //[FromRoute] //_ModelClass
        public int? BookId { get; set; }
        //[FromQuery] //_ModelClass
        public string? Author { get; set; }
        public override string ToString()
        {
            return $"Book object - Book id: {BookId}, Author: {Author}";
        }
    }
}
