using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : ControllerBase
    {
        private ICourseLibraryRepository _courseLibraryRepository;
        private IMapper _mapper;
        public AuthorCollectionsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        [HttpGet("({ids})",Name ="GetAuthorCollections")]
        public IActionResult GetAuthorCollection([FromRoute] [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null) return BadRequest();
            var authorsEntities = _courseLibraryRepository.GetAuthors(ids);
            if (ids.Count() != authorsEntities.Count()) return NotFound();
            var authors = _mapper.Map<IEnumerable<AuthorDto>>(authorsEntities);
            return Ok(authors);
        }
        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorCollections)
        {
            var authorEntities = _mapper.Map<IEnumerable<Entities.Author>>(authorCollections);
            foreach (var author in authorEntities)
            {
                _courseLibraryRepository.AddAuthor(author);

            }
            _courseLibraryRepository.Save();
            var authors = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var ids = string.Join(",", authors.Select(x => x.Id));
            return CreatedAtRoute("GetAuthorCollections", new { ids = ids }, authors);
        }
    }
}
