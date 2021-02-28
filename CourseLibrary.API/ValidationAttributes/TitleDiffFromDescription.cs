using CourseLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.ValidationAttributes
{
    public class TitleDiffFromDescription : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForCreationDto)validationContext.ObjectInstance;
            if(course.Title == course.Description)
            {
                return new ValidationResult("the provided description should be different from the title", new[] { "CourseForCreationDto" });
            }
            return ValidationResult.Success;
        }
    }
}
