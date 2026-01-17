using Mapster;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Application.Validations;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/Reasons")]
    [ApiController]
    [Authorize]    
    public class ReasonsController : ControllerBase
    {
        private readonly IReasonsService _service;

        public ReasonsController(IReasonsService service)
        {
            _service = service;
        }

        
        // GET: api/Reasons
        [SwaggerOperation(Summary = "Obtiene las razones.", Description = "Este endpoint se creo para obtener las razones en general.")]
        [SwaggerResponse(200, "Te devuelve las razones en un JSON de paginacion.", typeof(APIResponse<ReasonDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,User")]
        public async Task<ActionResult<APIResponse<ReasonDTO>>> Get([FromQuery] ReasonRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Reasons/5
        [SwaggerOperation(Summary = "Obtiene la razon en especifico.", Description = "Este endpoint trae una razon con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve una razon.", typeof(ReasonDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Razon no encontrada.")]
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,User")]
        public async Task<ActionResult<ReasonDTO>> Get(int id)
        {

            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            ReasonDTO dto = entity.Adapt<ReasonDTO>();
            return Ok(dto);
        }

        // POST api/Reasons
        [SwaggerOperation(Summary = "Agrega una razon al sistema.", Description = "Este endpoint crea una razon solo si eres Doctor o Admin.")]
        [SwaggerResponse(201, "Te devuelve la razon creada.", typeof(ReasonDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<ReasonDTO>> PostAsync([FromBody] ReasonCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Reasons/5
        [SwaggerOperation(Summary = "Actualiza una razon.", Description = "Este endpoint actualiza una razon solo si eres Doctor o Admin.")]
        [SwaggerResponse(204, "Razon actualizada.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] ReasonUpdateDTO dtou)
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

        // PATCH api/Reasons/5
        [SwaggerOperation(Summary = "Actualiza parcialmente una razon.", Description = "Este endpoint actualiza parcialmente una razon solo si eres Doctor o Admin.")]
        [SwaggerResponse(204, "Razon actualizada.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<ReasonPatchDTO> dtop)
        {
            //Busca el modelo por el id ingresado
            var model = await _service.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            //Le manda el modelo y el JsonPatchDocument al servicio para que haga el patch
            var validate = await _service.PatchAsync(model, dtop);

            //Si hay errores de validacion los agrega al ModelState y retorna ValidationProblem
            if (!validate.IsValid)
            {
                foreach (var error in validate.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return ValidationProblem();
            }
            //Si todo sale bien retorna NoContent
            return NoContent();
            
        }

        // DELETE api/Reasons/5
        [SwaggerOperation(Summary = "Elimina una razon.", Description = "Este endpoint elimina una razon solo si eres Doctor o Admin.")]
        [SwaggerResponse(204, "Razon eliminada.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Razon no encontrada.")]
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<ReasonModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }
    }
}
