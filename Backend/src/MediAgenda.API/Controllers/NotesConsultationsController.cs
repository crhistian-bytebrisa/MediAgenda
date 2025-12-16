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
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<APIResponse<NoteConsultationDTO>>> Get([FromQuery] NoteConsultationRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/NoteConsultations/5
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
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<NoteConsultationDTO>> PostAsync([FromBody] NoteConsultationCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/NoteConsultations/5
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


        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<DoctorDTO>> PatchAsync(int id)
        {
            return null;
        }

        // DELETE api/NoteConsultations/5
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
