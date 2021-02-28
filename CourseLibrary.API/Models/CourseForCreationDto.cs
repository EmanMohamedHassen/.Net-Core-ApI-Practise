using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    [TitleDiffFromDescription(ErrorMessage = "the provided description should be different from the title")]
    public class CourseForCreationDto //:IValidatableObject
    {
        [Required(ErrorMessage ="you should fill out a title")]
        [MaxLength(100,ErrorMessage ="The title shouldn't have more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The Description shouldn't have more than 1500 characters.")]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //   if(Title == Description)
        //    {
        //        // yield return make sure the validation result are immediatily returned after which code execution will continue
        //        yield return new ValidationResult("the provided description should be different from the title", new[] { "CourseForCreationDto" });// use class name as it class level validation 
        //    }
        //}
    }
}
