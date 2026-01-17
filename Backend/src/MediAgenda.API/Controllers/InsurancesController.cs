using Mapster;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/Insurances")]
    [ApiController]
    [Authorize]
    public class InsurancesController : ControllerBase
    {
        private readonly IInsurancesService _service;

        public InsurancesController(IInsurancesService service)
        {
            _service = service;
        }

        [SwaggerOperation(Summary = "Obtiene los nombres de los seguros.", Description = "Este endpoint se creo para llenar los textbox del front, todos los usuarios tienen acceso.")]
        [SwaggerResponse(200, "Te devuelve los nombres de los seguros.", typeof(List<string>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpGet("Names")]
        [Authorize(Roles = "Doctor,Admin,User")]
        public async Task<List<string>> GetNames()
        {
            return await _service.GetAllNames();
        }

        // GET: api/Insurances
        [SwaggerOperation(Summary = "Obtiene los seguros.", Description = "Este endpoint se creo para obtener los seguros en general.")]
        [SwaggerResponse(200, "Te devuelve los seguros en un JSON de paginacion.", typeof(APIResponse<InsuranceDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpGet]
        public async Task<ActionResult<APIResponse<InsuranceDTO>>> Get([FromQuery] InsuranceRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Insurances/5
        [SwaggerOperation(Summary = "Obtiene el seguro en especifico.", Description = "Este endpoint trae un seguro con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve un seguro.", typeof(InsuranceDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Seguro no encontrado.")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InsuranceDTO>> Get(int id)
        {

            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            InsuranceDTO dto = entity.Adapt<InsuranceDTO>();
            return Ok(dto);
        }

        // POST api/Insurances
        [SwaggerOperation(Summary = "Agrega un seguro al sistema.", Description = "Este endpoint crea un seguro.")]
        [SwaggerResponse(201, "Te devuelve el seguro creado.", typeof(InsuranceDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpPost]
        public async Task<ActionResult<InsuranceDTO>> PostAsync([FromBody] InsuranceCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Insurances/5
        [SwaggerOperation(Summary = "Actualiza un seguro.", Description = "Este endpoint actualiza un seguro solo si eres Admin.")]
        [SwaggerResponse(204, "Seguro actualizado.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] InsuranceUpdateDTO dtou)
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

        [SwaggerOperation(Summary = "Actualiza parcialmente un seguro.", Description = "Este endpoint actualiza parcialmente un seguro.")]
        [SwaggerResponse(501, "No implementado.")]
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<DoctorDTO>> PatchAsync(int id)
        {
            return null;
        }

        // DELETE api/Insurances/5
        [SwaggerOperation(Summary = "Elimina un seguro.", Description = "Este endpoint elimina un seguro.")]
        [SwaggerResponse(204, "Seguro eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Seguro no encontrado.")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<InsuranceModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }

    }
}
