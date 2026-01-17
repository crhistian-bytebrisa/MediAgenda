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
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Obtiene los doctores.", Description = "Este endpoint se creo para obtener los doctores en general, solo accesible para Doctores y Administradores.")]
        [SwaggerResponse(200, "Te devuelve los doctores en un JSON de paginacion.", typeof(APIResponse<DoctorDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<APIResponse<DoctorDTO>>> Get([FromQuery] DoctorRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Doctors/5
        [SwaggerOperation(Summary = "Obtiene el doctor en especifico.", Description = "Este endpoint trae un doctor con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve un doctor.", typeof(DoctorDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Doctor no encontrado.")]
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
        [SwaggerOperation(Summary = "Agrega un doctor al sistema.", Description = "Este endpoint crea un doctor solo si eres Admin.")]
        [SwaggerResponse(201, "Te devuelve el doctor creado.", typeof(DoctorDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DoctorDTO>> PostAsync([FromBody] DoctorCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Doctors/5
        [SwaggerOperation(Summary = "Actualiza un doctor.", Description = "Este endpoint actualiza un doctor solo si eres Admin.")]
        [SwaggerResponse(204, "Doctor actualizado.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
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

        [SwaggerOperation(Summary = "Actualiza parcialmente un doctor.", Description = "Este endpoint actualiza parcialmente un doctor.")]
        [SwaggerResponse(501, "No implementado.")]
        [HttpPatch("{id:int}")]
        [AuthorizeSameUserOrRoles("userId", "Admin")]
        public async Task<ActionResult<DoctorDTO>> PatchAsync(int id)
        {
            return null;
        }

        // DELETE api/Doctors/5
        [SwaggerOperation(Summary = "Elimina un doctor.", Description = "Este endpoint elimina un doctor solo si eres Admin.")]
        [SwaggerResponse(204, "Doctor eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [SwaggerResponse(404, "Doctor no encontrado.")]
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
