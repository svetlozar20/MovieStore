using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.Responses;
using MovieStore.Models.View;

namespace MovieStore.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly ILogger<BusinessController> _logger;
        private readonly IBusinessService _businessService;
        private readonly IMapper _mapper;

        public BusinessController(ILogger<BusinessController> logger, IBusinessService businessService, IMapper mapper)
        {
            _logger = logger;
            _businessService = businessService;
            _mapper = mapper;
        }

        [HttpGet("GetAllDetailedMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDetailedMovieInfo()
        {
            var result = _businessService.GetDetailedMovies();
            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("AddActorToMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddActorToMovie(string actorId, string movieId)
        {
            try
            {
                _businessService.AddActor(movieId, actorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            

            return Ok();
        }
        
    }
}