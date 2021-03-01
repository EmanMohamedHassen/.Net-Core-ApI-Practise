using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    [TitleDiffFromDescription(ErrorMessage = "the provided description should be different from the title")]
    // make class abstract to serve as a base class 
    public abstract class CourseForManipulationDto 
    {
        [Required(ErrorMessage = "you should fill out a title")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The Description shouldn't have more than 1500 characters.")]
        public virtual string Description { get; set; } // make property abstract  so can be implemented in child class
    }
}
