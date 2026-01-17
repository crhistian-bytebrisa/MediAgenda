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
    [Route("api/Medicines")]
    [ApiController]
    [Authorize]    
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicinesService _service;

        public MedicinesController(IMedicinesService service)
        {
            _service = service;
        }


        // GET: api/Medicines
        [SwaggerOperation(Summary = "Obtiene los medicamentos.", Description = "Este endpoint se creo para obtener los medicamentos en general, solo accesible para Doctores y Administradores.")]
        [SwaggerResponse(200, "Te devuelve los medicamentos en un JSON de paginacion.", typeof(APIResponse<MedicineDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<APIResponse<MedicineDTO>>> Get([FromQuery] MedicineRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Medicines/5
        [SwaggerOperation(Summary = "Obtiene el medicamento en especifico.", Description = "Este endpoint trae un medicamento con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve un medicamento.", typeof(MedicineDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Medicamento no encontrado.")]
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<MedicineDTO>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            MedicineDTO dto = entity.Adapt<MedicineDTO>();
            return Ok(dto);
        }

        // POST api/Medicines
        [SwaggerOperation(Summary = "Agrega un medicamento al sistema.", Description = "Este endpoint crea un medicamento solo si eres Doctor o Admin.")]
        [SwaggerResponse(201, "Te devuelve el medicamento creado.", typeof(MedicineDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<MedicineDTO>> PostAsync([FromBody] MedicineCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Medicines/5
        [SwaggerOperation(Summary = "Actualiza un medicamento.", Description = "Este endpoint actualiza un medicamento solo si eres Admin.")]
        [SwaggerResponse(204, "Medicamento actualizado.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] MedicineUpdateDTO dtou)
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


        // DELETE api/Medicines/5
        [SwaggerOperation(Summary = "Elimina un medicamento.", Description = "Este endpoint elimina un medicamento solo si eres Doctor.")]
        [SwaggerResponse(204, "Medicamento eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Doctor")]
        [SwaggerResponse(404, "Medicamento no encontrado.")]
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<MedicineModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }

    }
}