using Mapster;
using MediAgenda.API.Filters;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.DTOs.Relations;
using MediAgenda.Application.Interfaces;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/Consultations")]
    [ApiController]
    [Authorize]
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationsService _service;

        public ConsultationsController(IConsultationsService service)
        {
            _service = service;
        }
        // GET: api/Consultations
        [SwaggerOperation(Summary = "Obtiene las consultas.", Description = "Este endpoint se creo para obtener las consultas en general.")]
        [SwaggerResponse(200, "Te devuelve las consultas en un JSON de paginacion.", typeof(APIResponse<ConsultationDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No tienes permisos.")]
        [HttpGet]
        [AuthorizeSamePatientIdOrRoles("PatientId", "Doctor", "Admin")]
        public async Task<ActionResult<APIResponse<ConsultationDTO>>> Get([FromQuery] ConsultationRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Consultations/5
        [SwaggerOperation(Summary = "Obtiene la consulta en especifico.", Description = "Este endpoint trae una consulta con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve una consulta.", typeof(ConsultationDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No tienes permisos.")]
        [SwaggerResponse(404, "Consulta no encontrada.")]
        [HttpGet("{id:int}")]
        [AuthorizeSamePatientIdOrRoles("PatientId", "Doctor", "Admin")]
        public async Task<ActionResult<ConsultationDTO>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            ConsultationDTO dto = entity.Adapt<ConsultationDTO>();
            return Ok(dto);
        }

        // GET api/Consultations/5/Notes
        [SwaggerOperation(Summary = "Obtiene las notas de una consulta.", Description = "Este endpoint trae las notas de una consulta especifica.")]
        [SwaggerResponse(200, "Te devuelve las notas en un JSON de paginacion.", typeof(APIResponse<NoteConsultationDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Consulta no encontrada.")]
        [HttpGet("{id:int}/Notes")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<APIResponse<NoteConsultationDTO>>> GetNotes(int id, [FromQuery] ConsultationNoteRequest request)
        {
            var dto = await _service.GetAllNotes(id, request);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        // POST api/Consultations
        [SwaggerOperation(Summary = "Agrega una consulta al sistema.", Description = "Este endpoint crea una consulta.")]
        [SwaggerResponse(201, "Te devuelve la consulta creada.", typeof(ConsultationDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No tienes permisos.")]
        [HttpPost]
        [AuthorizeSamePatientIdOrRoles("PatientId", "Doctor", "Admin")]
        public async Task<ActionResult<ConsultationDTO>> PostAsync([FromBody] ConsultationCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Consultations/5
        [SwaggerOperation(Summary = "Actualiza una consulta.", Description = "Este endpoint actualiza una consulta solo si eres Admin.")]
        [SwaggerResponse(204, "Consulta actualizada.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] ConsultationUpdateDTO dtou)
        {
            if (ModelState.ErrorCount > 0)
            {
                return ValidationProblem();
            }

            await _service.UpdateAsync(dtou);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Actualiza parcialmente una consulta.", Description = "Este endpoint actualiza parcialmente una consulta.")]
        [SwaggerResponse(501, "No implementado.")]
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,User")]
        public async Task<ActionResult<ConsultationDTO>> PatchAsync(int id)
        {
            return null;
        }


        // DELETE api/Consultations/5
        [SwaggerOperation(Summary = "Elimina una consulta.", Description = "Este endpoint elimina una consulta.")]
        [SwaggerResponse(204, "Consulta eliminada.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No tienes permisos.")]
        [SwaggerResponse(404, "Consulta no encontrada.")]
        [HttpDelete("{id:int}")]
        [AuthorizeSamePatientIdOrRoles("PatientId", "Doctor", "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<ConsultationModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }


    }
}