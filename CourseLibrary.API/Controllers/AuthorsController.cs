using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseLibrary.API.Services;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
  //  [Route("api/[controller] ")] not recommended if controller resource changed  after while will affect resource of api 
    public class AuthorsController : ControllerBase
    {
        private ICourseLibraryRepository _courseLibraryRepository;
        public AuthorsController(ICourseLibraryRepository courseLibraryRepository)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

        }
        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authors = _courseLibraryRepository.GetAuthors();
            return Ok(authors);

        }
        [HttpGet("{authorId:guid}")] // use guid type if we have multiple end points with different id type to distinguise it 
        public IActionResult GetAuthor(Guid authorId)
        {
            
        var author = _courseLibraryRepository.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }
    }
}
