using Mapster;
using MediAgenda.API.Filters;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MediAgenda.API.Controllers
{
    [Route("api/Doctors")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorsService _service;

        public DoctorsController(IDoctorsService service)
        {
            _service = service;
        }


        // GET: api/Doctors
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<APIResponse<DoctorDTO>>> Get([FromQuery] DoctorRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Doctors/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<DoctorDTO>> Get(int id)
        {

            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            DoctorDTO dto = entity.Adapt<DoctorDTO>();
            return Ok(dto);
        }

        // POST api/Doctors
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DoctorDTO>> PostAsync([FromBody] DoctorCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Doctors/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] DoctorUpdateDTO dtou)
        {
            if (id != dtou.Id)
            {
                ModelState.AddModelError("Id", "Deben tener el mismo Id.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return ValidationProblem();
            }

            await _service.UpdateAsync(dtou);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [AuthorizeSameUserOrRoles("userId", "Admin")]
        public async Task<ActionResult<DoctorDTO>> PatchAsync(int id)
        {
            return null;
        }

        // DELETE api/Doctors/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<DoctorModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }
    }
}
