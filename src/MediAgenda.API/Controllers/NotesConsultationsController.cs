using Mapster;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
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
    [Route("api/NotesConsultations")]
    [ApiController]
    [Authorize]
    public class NotesConsultationsController : ControllerBase
    {
        private readonly INotesConsultationService _service;

        public NotesConsultationsController(INotesConsultationService service)
        {
            _service = service;
        }


        // GET: api/NoteConsultations
        [SwaggerOperation(Summary = "Obtiene las notas de consultas.", Description = "Este endpoint se creo para obtener las notas de consultas en general, solo accesible para Doctores y Administradores.")]
        [SwaggerResponse(200, "Te devuelve las notas de consultas en un JSON de paginacion.", typeof(APIResponse<NoteConsultationDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<APIResponse<NoteConsultationDTO>>> Get([FromQuery] NoteConsultationRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/NoteConsultations/5
        [SwaggerOperation(Summary = "Obtiene la nota de consulta en especifico.", Description = "Este endpoint trae una nota de consulta con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve una nota de consulta.", typeof(NoteConsultationDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Nota de consulta no encontrada.")]
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<NoteConsultationDTO>> Get(int id)
        {

            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            NoteConsultationDTO dto = entity.Adapt<NoteConsultationDTO>();
            return Ok(dto);
        }

        // POST api/NoteConsultations
        [SwaggerOperation(Summary = "Agrega una nota de consulta al sistema.", Description = "Este endpoint crea una nota de consulta solo si eres Doctor o Admin.")]
        [SwaggerResponse(201, "Te devuelve la nota de consulta creada.", typeof(NoteConsultationDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<NoteConsultationDTO>> PostAsync([FromBody] NoteConsultationCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/NoteConsultations/5
        [SwaggerOperation(Summary = "Actualiza una nota de consulta.", Description = "Este endpoint actualiza una nota de consulta solo si eres Admin.")]
        [SwaggerResponse(204, "Nota de consulta actualizada.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] NoteConsultationUpdateDTO dtou)
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

        // DELETE api/NoteConsultations/5
        [SwaggerOperation(Summary = "Elimina una nota de consulta.", Description = "Este endpoint elimina una nota de consulta solo si eres Doctor o Admin.")]
        [SwaggerResponse(204, "Nota de consulta eliminada.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Nota de consulta no encontrada.")]
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<NoteConsultationModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }

    }
}
