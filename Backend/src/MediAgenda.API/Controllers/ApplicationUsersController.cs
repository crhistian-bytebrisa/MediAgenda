using Mapster;
using MediAgenda.API.Filters;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/ApplicationUsers")]
    [ApiController]
    [Authorize]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly IApplicationUsersService _service;

        public ApplicationUsersController(IApplicationUsersService service)
        {
            _service = service;
        }
        // GET: api/ApplicationUsers
        [SwaggerOperation(
            Summary = "Obtiene los usuarios de aplicacion.",
            Description = "Este endpoint se creo para obtener los usuarios de aplicacion en general, solo accesible para Doctores y Administradores."
        )]
        [SwaggerResponse(200, "Te devuelve los usuarios en un JSON de paginacion.", typeof(APIResponse<ApplicationUserDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<APIResponse<ApplicationUserDTO>>> Get([FromQuery] ApplicationUserRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/ApplicationUsers/dato-ramd-ommm
        [SwaggerOperation(
            Summary = "Obtiene el usuario de aplicacion en especifico.",
            Description = "Este endpoint trae un usuario de aplicacion con todos los datos relacionados al mismo."
        )]
        [SwaggerResponse(200, "Te devuelve un usuario de aplicacion.", typeof(ApplicationUserDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Usuario no encontrado.")]
        [HttpGet("{id}")]
        [AuthorizeSameUserOrRoles("id","Admin")]
        public async Task<ActionResult<ApplicationUserDTO>> Get(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            ApplicationUserDTO dto = entity.Adapt<ApplicationUserDTO>();
            return Ok(dto);
        }

        // POST api/ApplicationUsers
        [SwaggerOperation(
            Summary = "Agrega un usuario de aplicacion al sistema.",
            Description = "Este endpoint crea un usuario de aplicacion solo si eres Admin."
        )]
        [SwaggerResponse(201, "Te devuelve el usuario creado.", typeof(ApplicationUserDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationUserDTO>> PostAsync([FromBody] ApplicationUserCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/ApplicationUsers/dato-ramd-ommm
        [SwaggerOperation(
            Summary = "Actualiza un usuario de aplicacion.",
            Description = "Este endpoint actualiza un usuario de aplicacion solo si eres Admin."
        )]
        [SwaggerResponse(204, "Usuario actualizado.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{ids}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(Guid id, [FromBody] ApplicationUserUpdateDTO dtou)
        {
            if(id != dtou.Id)
            {
                ModelState.AddModelError("Id", "El Id en la URL no coincide con el Id en el cuerpo de la solicitud.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return ValidationProblem();
            }
            await _service.UpdateAsync(dtou);
            return NoContent();
        }

        // DELETE api/ApplicationUsers/dato-ramd-ommm
        [SwaggerOperation(
            Summary = "Elimina un usuario de aplicacion.",
            Description = "Este endpoint elimina un usuario de aplicacion solo si eres Admin."
        )]
        [SwaggerResponse(204, "Usuario eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [SwaggerResponse(404, "Usuario no encontrado.")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<ApplicationUserModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }
    }
}
