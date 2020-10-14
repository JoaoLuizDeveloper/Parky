using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version:apiversion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenApiSpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailrepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailrepo, IMapper mapper)
        {
            _trailrepo = trailrepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get List of Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type= typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = _trailrepo.GetTrails();

            var objDTo = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDTo.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDTo);
        }

        /// <summary>
        /// Get Individual Trail
        /// </summary>
        /// <param name="id">The id of the Trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var obj = _trailrepo.GetTrail(id);
            if (obj == null)
            {
                return NotFound();
            }

            var objDTO = _mapper.Map<TrailDto>(obj);
            return Ok(objDTO);
        }

        /// <summary>
        /// Get Individual Trail in national Park
        /// </summary>
        /// <param name="nationalParkid">The id of the NationalParkId</param>
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkid:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkid)
        {
            var objList = _trailrepo.GetTrailsInNationalPark(nationalParkid);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<TrailDto>();
            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailsDto)
        {
            if (trailsDto == null)
            {
                return BadRequest(ModelState);
            }

            if(_trailrepo.TrailExists(trailsDto.Name))
            {
                ModelState.AddModelError("", "Trail Exist");
                return StatusCode(404, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trailsObj = _mapper.Map<Trail>(trailsDto);

            if (!_trailrepo.CreateTrail(trailsObj))
            {
                ModelState.AddModelError("",$"Something went wrong when you trying to save {trailsObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { id= trailsObj.Id }, trailsObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailsDto)
        {
            if (trailsDto == null || id != trailsDto.Id)
            {
                return BadRequest(ModelState);
            }

            var trailsObj = _mapper.Map<Trail>(trailsDto);

            if (!_trailrepo.UpdateTrail(trailsObj))
            {
                ModelState.AddModelError("", $"Something went wrong when you trying to updating {trailsObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_trailrepo.TrailExists(id))
            {
                return NotFound();
            }

            var trailsObj = _trailrepo.GetTrail(id);

            if (!_trailrepo.DeleteTrail(trailsObj))
            {
                ModelState.AddModelError("", $"Something went wrong when you trying to deleting {trailsObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
