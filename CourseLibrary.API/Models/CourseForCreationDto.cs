using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    [TitleDiffFromDescription]
    public class CourseForCreationDto //:IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
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
