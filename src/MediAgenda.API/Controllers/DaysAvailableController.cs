using Mapster;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Application.Validations;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/DaysAvailable")]
    [ApiController]
    [Authorize]
    [Authorize(Roles = "Doctor,Admin,User")]
    public class DaysAvailableController : ControllerBase
    {
        private readonly IDayAvailablesService _service;

        public DaysAvailableController(IDayAvailablesService service)
        {
            _service = service;
        }

        // GET: api/DaysAvailable
        [SwaggerOperation(Summary = "Obtiene los dias disponibles.", Description = "Este endpoint se creo para obtener los dias disponibles en general.")]
        [SwaggerResponse(200, "Te devuelve los dias disponibles en un JSON de paginacion.", typeof(APIResponse<DayAvailableDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpGet]
        public async Task<ActionResult<APIResponse<DayAvailableDTO>>> Get([FromQuery] DayAvailableRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/DaysAvailable/5
        [SwaggerOperation(Summary = "Obtiene el dia disponible en especifico.", Description = "Este endpoint trae un dia disponible con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve un dia disponible.", typeof(DayAvailableDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Dia disponible no encontrado.")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DayAvailableDTO>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            DayAvailableDTO dto = entity.Adapt<DayAvailableDTO>();
            return Ok(dto);
        }

        // POST api/DaysAvailable
        [SwaggerOperation(Summary = "Agrega un dia disponible al sistema.", Description = "Este endpoint crea un dia disponible.")]
        [SwaggerResponse(201, "Te devuelve el dia disponible creado.", typeof(DayAvailableDTO))]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpPost]
        public async Task<ActionResult<DayAvailableDTO>> PostAsync([FromBody] DayAvailableCreateDTO dtoc)
        {
            if (dtoc.ClinicId <= 0)
            {
                ModelState.AddModelError("ClinicId", "El ClinicId es invalido.");
                return ValidationProblem();
            }

            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/DaysAvailable/5
        [SwaggerOperation(Summary = "Actualiza un dia disponible.", Description = "Este endpoint actualiza un dia disponible.")]
        [SwaggerResponse(204, "Dia disponible actualizado.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] DayAvailableUpdateDTO dtou)
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


        // DELETE api/DaysAvailable/5
        [SwaggerOperation(Summary = "Elimina un dia disponible.", Description = "Este endpoint elimina un dia disponible.")]
        [SwaggerResponse(204, "Dia disponible eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Dia disponible no encontrado.")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<DayAvailableModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }

    }
}