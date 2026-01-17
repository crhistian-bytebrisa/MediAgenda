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
    [Route("api/NotesPatients")]
    [ApiController]
    [Authorize]
    public class NotesPatientsController : ControllerBase
    {
        private readonly INotesPatientsService _service;

        public NotesPatientsController(INotesPatientsService service)
        {
            _service = service;
        }

        // GET: api/NotesPatients
        [SwaggerOperation(Summary = "Obtiene las notas de pacientes.", Description = "Este endpoint se creo para obtener las notas de pacientes en general, solo accesible para Doctores y Administradores.")]
        [SwaggerResponse(200, "Te devuelve las notas de pacientes en un JSON de paginacion.", typeof(APIResponse<NotePatientDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<APIResponse<NotePatientDTO>>> Get([FromQuery] NotePatientRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/NotesPatients/5
        [SwaggerOperation(Summary = "Obtiene la nota de paciente en especifico.", Description = "Este endpoint trae una nota de paciente con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve una nota de paciente.", typeof(NotePatientDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Nota de paciente no encontrada.")]
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<NotePatientDTO>> Get(int id)
        {

            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            NotePatientDTO dto = entity.Adapt<NotePatientDTO>();
            return Ok(dto);
        }

        // POST api/NotesPatients
        [SwaggerOperation(Summary = "Agrega una nota de paciente al sistema.", Description = "Este endpoint crea una nota de paciente solo si eres Doctor o Admin.")]
        [SwaggerResponse(201, "Te devuelve la nota de paciente creada.", typeof(NotePatientDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<NotePatientDTO>> PostAsync([FromBody] NotePatientCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/NotesPatients/5
        [SwaggerOperation(Summary = "Actualiza una nota de paciente.", Description = "Este endpoint actualiza una nota de paciente solo si eres Admin.")]
        [SwaggerResponse(204, "Nota de paciente actualizada.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] NotePatientUpdateDTO dtou)
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

        [SwaggerOperation(Summary = "Actualiza parcialmente una nota de paciente.", Description = "Este endpoint actualiza parcialmente una nota de paciente.")]
        [SwaggerResponse(501, "No implementado.")]
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<DoctorDTO>> PatchAsync(int id)
        {
            return null;
        }

        // DELETE api/NotesPatients/5
        [SwaggerOperation(Summary = "Elimina una nota de paciente.", Description = "Este endpoint elimina una nota de paciente solo si eres Doctor o Admin.")]
        [SwaggerResponse(204, "Nota de paciente eliminada.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Nota de paciente no encontrada.")]
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<NotePatientModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }
    }
}