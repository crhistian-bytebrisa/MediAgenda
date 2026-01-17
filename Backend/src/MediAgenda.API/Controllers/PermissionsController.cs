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
    [Route("api/Permissions")]
    [ApiController]
    [Authorize]    
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsService _service;

        public PermissionsController(IPermissionsService service)
        {
            _service = service;
        }

        // GET: api/Permissions
        [SwaggerOperation(Summary = "Obtiene los permisos.", Description = "Este endpoint se creo para obtener los permisos en general, solo accesible para Doctores y Administradores.")]
        [SwaggerResponse(200, "Te devuelve los permisos en un JSON de paginacion.", typeof(APIResponse<PermissionDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<APIResponse<PermissionDTO>>> Get([FromQuery] PermissionRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Permissions/5
        [SwaggerOperation(Summary = "Obtiene el permiso en especifico.", Description = "Este endpoint trae un permiso con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve un permiso.", typeof(PermissionDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Permiso no encontrado.")]
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<PermissionDTO>> Get(int id)
        {

            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            PermissionDTO dto = entity.Adapt<PermissionDTO>();
            return Ok(dto);
        }

        // POST api/Permissions
        [SwaggerOperation(Summary = "Agrega un permiso al sistema.", Description = "Este endpoint crea un permiso solo si eres Doctor o Admin.")]
        [SwaggerResponse(201, "Te devuelve el permiso creado.", typeof(PermissionDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<PermissionDTO>> PostAsync([FromBody] PermissionCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Permissions/5
        [SwaggerOperation(Summary = "Actualiza un permiso.", Description = "Este endpoint actualiza un permiso solo si eres Admin.")]
        [SwaggerResponse(204, "Permiso actualizado.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] PermissionUpdateDTO dtou)
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

        // DELETE api/Permissions/5
        [SwaggerOperation(Summary = "Elimina un permiso.", Description = "Este endpoint elimina un permiso solo si eres Doctor o Admin.")]
        [SwaggerResponse(204, "Permiso eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Permiso no encontrado.")]
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<PermissionModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }
    }
}