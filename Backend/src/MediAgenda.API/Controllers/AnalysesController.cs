using Mapster;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Application.Validations;
using MediAgenda.Domain.Core;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.Repositories;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using NuGet.Protocol;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace MediAgenda.API.Controllers
{

    [Route("api/Analyses")]
    [ApiController]
    [Authorize]
    
    public class AnalysesController : ControllerBase
    {
        private readonly IAnalysesService _service;

        public AnalysesController(IAnalysesService service)
        {
            _service = service;
        }
        
        // GET: api/Analyses/Names
        [HttpGet("Names")]
        [SwaggerResponse(200, "Te devuelve los nombres de los analisis.",typeof(List<AnalysesListItem>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerOperation(
            Summary = "Obtiene los nombres de los analisis.",
            Description = "Este endpoint se creo para llenar los textbox del front, todos los usuarios tienen acceso."
            )]
        [Authorize(Roles = "Doctor,Admin,User")]
        public async Task<List<AnalysesListItem>> GetNames()
        {
            return await _service.GetAllNames();
        }


        // GET: api/Analyses
        [HttpGet]
        [Authorize(Roles = "Doctor,Admin")]
        [SwaggerResponse(200, "Te devuelve los analisis en un JSON de paginacion.", typeof(APIResponse<AnalysisDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerOperation(
            Summary = "Obtiene los analisis sin los datos relacionados a no ser que pidas que se incluyan.",
            Description = "Este endpoint se creo para optener los analisis en general mas alla del nombre, esto solo tiene acceso los doctores y administradores."
            )]
        public async Task<ActionResult<APIResponse<AnalysisDTO>>> Get([FromQuery] AnalysisRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }



        // GET api/Analyses/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        [SwaggerResponse(200, "Te devuelve un analisis.", typeof(AnalysisDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerOperation(
            Summary = "Obtiene el analisis en especifico.",
            Description = "Este endpoint trae un analisis con todos los datos relacionados al mismo."
            )]
        public async Task<ActionResult<AnalysisDTO>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            AnalysisDTO dto = entity.Adapt<AnalysisDTO>();
            return Ok(dto);
        }


        // POST api/Analyses
        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        [SwaggerResponse(200, "Te devuelve el analisis creado.", typeof(AnalysisDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerOperation(
            Summary = "Agrega un analisis al sistema.",
            Description = "Este endpoint crea un analisis solo si eres Doctor o Admin."
            )]
        public async Task<ActionResult<AnalysisDTO>> PostAsync([FromBody] AnalysisCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Analyses/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] AnalysisUpdateDTO dtou)
        {

            if(ModelState.ErrorCount > 0)
            {
                return ValidationProblem();
            }

            await _service.UpdateAsync(dtou);
            return NoContent();
        }

        // DELETE api/Analyses/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<AnalysisModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }
        
    }
}
