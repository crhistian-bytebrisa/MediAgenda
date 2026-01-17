using Mapster;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Application.Services;
using MediAgenda.Application.Validations;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/Clinics")]
    [ApiController]
    [Authorize]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicsService _service;

        public ClinicsController(IClinicsService service)
        {
            _service = service;
        }

        // GET: api/Clinics/Names        
        [SwaggerOperation(Summary = "Obtiene los nombres de las clinicas.", Description = "Este endpoint se creo para llenar los textbox del front, todos los usuarios tienen acceso.")]
        [SwaggerResponse(200, "Te devuelve los nombres de las clinicas.", typeof(List<ClinicsListItem>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpGet("Names")]
        [Authorize(Roles = "Doctor,Admin,User")]
        public async Task<List<ClinicsListItem>> GetNames()
        {
            return await _service.GetAllNames();
        }

        // GET: api/Clinics
        [SwaggerOperation(Summary = "Obtiene las clinicas sin los datos relacionados a no ser que pidas que se incluyan.", Description = "Este endpoint se creo para optener las clinicas en general mas alla del nombre, esto tiene acceso todos los usuarios.")]
        [SwaggerResponse(200, "Te devuelve las clinicas en un JSON de paginacion.", typeof(APIResponse<ClinicDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpGet]
        [Authorize(Roles = "Doctor,Admin,User")]
        public async Task<ActionResult<APIResponse<ClinicDTO>>> Get([FromQuery] ClinicRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Clinics/5
        [SwaggerOperation(Summary = "Obtiene la clinica en especifico.", Description = "Este endpoint trae una clinica con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve una clinica.", typeof(ClinicDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Clinica no encontrada.")]
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Doctor,Admin,User")]
        public async Task<ActionResult<ClinicDTO>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            ClinicDTO dto = entity.Adapt<ClinicDTO>();
            return Ok(dto);
        }

        // GET api/Clinics/5/Days
        [SwaggerOperation(Summary = "Obtiene los dias disponibles de una clinica.", Description = "Este endpoint trae los dias disponibles de una clinica especifica.")]
        [SwaggerResponse(200, "Te devuelve los dias disponibles.", typeof(List<DayAvailableDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Clinica no encontrada.")]
        [HttpGet("{id:int}/Days")]
        [Authorize(Roles = "Doctor,Admin,User")]
        public async Task<ActionResult<List<DayAvailableDTO>>> GetClinicDays(int id,[FromQuery] ClinicDaysAvailableRequest request)
        {
            var dto = await _service.GetAllDaysAvailableById(id, request);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }


        // POST api/Clinics
        [SwaggerOperation(Summary = "Agrega una clinica al sistema.", Description = "Este endpoint crea una clinica solo si eres Doctor o Admin.")]
        [SwaggerResponse(201, "Te devuelve la clinica creada.", typeof(ClinicDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<ClinicDTO>> PostAsync([FromBody] ClinicCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Clinics/5
        [SwaggerOperation(Summary = "Actualiza una clinica.", Description = "Este endpoint actualiza una clinica solo si eres Admin.")]
        [SwaggerResponse(204, "Clinica actualizada.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] ClinicUpdateDTO dtou)
        {

            if (ModelState.ErrorCount > 0)
            {
                return ValidationProblem();
            }

            await _service.UpdateAsync(dtou);
            return NoContent();
        }

        // DELETE api/Clinics/5
        [SwaggerOperation(Summary = "Elimina una clinica.", Description = "Este endpoint elimina una clinica solo si eres Doctor o Admin.")]
        [SwaggerResponse(204, "Clinica eliminada.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Clinica no encontrada.")]
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<ClinicModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }

    }
}
