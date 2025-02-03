using System.Net;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Request;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMovieService movieService, IMapper mapper, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            var result = _movieService.GetAllMovies();

            if (result.Count == 0)
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add(AddMovieRequest movieRequest)
        {
            var movieDto = _mapper.Map<Movie>(movieRequest);

            try
            {
                if (movieDto == null)
                {
                    return BadRequest("Could not add movie to the database");
                }

                _movieService.AddMovie(movieDto);
                return Ok("Movie added successfully.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Could not add movie to the database");
                return BadRequest(ex.Message);
            }
            
            
        }

        [HttpGet("GetMovieById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult? GetById(string id)
        {
            _logger.LogInformation($"Getting movie by id: {id}");
            if (id.Length == 0)
            {
                _logger.LogError($"Getting movie by id: {id}");
                return BadRequest("Id is invalid, must be greater than zero.");
            }
            var result = _movieService.GetMovieById(id);

            if (result == null)
            {
                return NotFound($"Movie with {id} not found.");
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(string id)
        {
            if (id.Length == 0)
            {
                return BadRequest("Id is invalid, must be greater than zero.");
            }
            _movieService.DeleteMovie(id);
            return Ok("Movie deleted.");
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(UpdateMovieRequest movieRequest)
        {
            var movieDto = _mapper.Map<Movie>(movieRequest);
            try
            {
                if (movieDto == null)
                {
                    return BadRequest("Could not update movie to the database");
                }

                _movieService.UpdateMovie(movieDto);
                return Ok("Movie updated successfully.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Could not update movie to the database");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetActorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetActorById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id is invalid, must be greater than zero.");
            }
            var result = _movieService.GetActorById(id);

            if (result == null)
            {
                return NotFound($"Movie with {id} not found.");
            }
            return Ok(result);
        }
    }
}
