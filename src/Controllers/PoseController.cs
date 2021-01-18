using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoseDatabaseWebApi.Models;

using AutoMapper;
using PoseDatabaseWebApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace PoseDatabaseWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PosesController : ControllerBase
    {
        private readonly IPoseRepository _poseRepository;
        private readonly IMapper _mapper;

        public PosesController(IPoseRepository poseRepository, IMapper mapper)
        {
            _poseRepository = poseRepository;
            _mapper = mapper;
        }

        // GET: api/Poses
        [HttpGet]
        public ActionResult<IEnumerable<PoseReadDto>> GetPoses()
        {
            var result = _poseRepository.GetPoses();
            return Ok(_mapper.Map<IEnumerable<PoseReadDto>>(result));
        }

        // GET: api/Poses/5
        [HttpGet("{id}", Name = "GetPose")]
        public ActionResult<PoseReadDto> GetPose(int id)
        {
            var pose = _poseRepository.GetPose(id);
            if (pose == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PoseReadDto>(pose));
        }

        [HttpGet("search/{input}")]
        public ActionResult<IEnumerable<Pose>> Search(string input)
        {
            try
            {
                var result = _poseRepository.Search(input);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/Poses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPatch("{id}")]
        public ActionResult PartialPoseUpdate(int id, JsonPatchDocument<PoseUpdateDto> patchDoc)
        {
            var poseModelFromRepo = _poseRepository.GetPose(id);
            if (poseModelFromRepo == null)
            {
                return NotFound();
            }

            var poseToPatch = _mapper.Map<PoseUpdateDto>(poseModelFromRepo);
            patchDoc.ApplyTo(poseToPatch, ModelState);

            if (!TryValidateModel(poseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(poseToPatch, poseModelFromRepo);

            _poseRepository.PatchPoseDetails(poseModelFromRepo);

            _poseRepository.SaveChanges();

            return NoContent();
        }

        //POST: api/Poses
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<PoseReadDto> AddPoseToDb(PoseCreateDto poseCreate)
        {
            var poseModel = _mapper.Map<Pose>(poseCreate);


            _poseRepository.AddPoseToDb(poseModel);


            _poseRepository.SaveChanges();

            var poseReadDto = _mapper.Map<PoseReadDto>(poseModel);

            return CreatedAtRoute(nameof(GetPose), new { Id = poseReadDto.Id }, poseReadDto);
            //CreatedAtRoute(RouteWhereResourceResides, Id of resource(used to generate route, actualReponseContent)
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePose(int id, PoseUpdateDto poseUpdateDto)
        {
            var poseModelFromRepo = _poseRepository.GetPose(id);
            if (poseModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(poseUpdateDto, poseModelFromRepo);

            _poseRepository.PatchPoseDetails(poseModelFromRepo);
            _poseRepository.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Poses/5
        [HttpDelete("{id}")]
        public ActionResult DeletePose(int id)
        {
            var poseModelFromRepo = _poseRepository.GetPose(id);
            if (poseModelFromRepo == null)
            {
                return NotFound();
            }

            _poseRepository.DeletePose(poseModelFromRepo);
            _poseRepository.SaveChanges();

            return NoContent();
        }
    }
}
