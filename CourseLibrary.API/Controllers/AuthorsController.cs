using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseLibrary.API.Services;
using CourseLibrary.API.Models;
using CourseLibrary.API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using CourseLibrary.API.ReasourceParameters;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
  //  [Route("api/[controller] ")] not recommended if controller resource changed  after while will affect resource of api 
    public class AuthorsController : ControllerBase
    {
        private ICourseLibraryRepository _courseLibraryRepository;
        private IMapper _mapper;
        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorsResourceParameters parameters) // get data of parameters from url query
        {
            var authors = _courseLibraryRepository.GetAuthors(parameters);

            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authors));

        }
        [HttpGet("{authorId:guid}",Name ="GetAuthor")] // use guid type if we have multiple end points with different id type to distinguise it 
        public IActionResult GetAuthor(Guid authorId)
        {
            
        var author = _courseLibraryRepository.GetAuthor(authorId);
            if (author == null) return NotFound();
            return Ok(_mapper.Map<AuthorDto>(author));
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            var authorEntity = _mapper.Map<Entities.Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();
            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor",
                new { authorId = authorToReturn.Id },
                authorToReturn
                );
        }
        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }


        [HttpDelete("{authorId:guid}")]
        public IActionResult DelteteAuthor(Guid authorId)
        {
            var author = _courseLibraryRepository.GetAuthor(authorId);
            if (author == null) return NotFound();
            _courseLibraryRepository.DeleteAuthor(author);
            _courseLibraryRepository.Save();
            return NoContent();
        }
    }
}
