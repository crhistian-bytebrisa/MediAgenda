using Mapster;
using MediAgenda.API.Filters;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Application.Validations;
using MediAgenda.Domain.Entities;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/MedicalDocuments")]
    [ApiController]
    [Authorize]
    [Authorize(Roles = "User,Doctor,Admin")]
    public class MedicalDocumentsController : ControllerBase
    {
        private readonly IMedicalDocumentsService _service;
        private readonly MedicalDocumentValidation _validation;

        public MedicalDocumentsController(IMedicalDocumentsService service, IValidationService validation)
        {
            _service = service;
            _validation = new MedicalDocumentValidation(validation);
        }


        // GET: api/MedicalDocuments
        [SwaggerOperation(Summary = "Obtiene los documentos medicos.", Description = "Este endpoint se creo para obtener los documentos medicos en general.")]
        [SwaggerResponse(200, "Te devuelve los documentos medicos en un JSON de paginacion.", typeof(APIResponse<MedicalDocumentDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpGet]
        public async Task<ActionResult<APIResponse<MedicalDocumentDTO>>> Get([FromQuery] MedicalDocumentRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/MedicalDocuments/5
        [SwaggerOperation(Summary = "Obtiene el documento medico en especifico.", Description = "Este endpoint trae un documento medico con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve un documento medico.", typeof(MedicalDocumentDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Documento medico no encontrado.")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedicalDocumentDTO>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            MedicalDocumentDTO dto = entity.Adapt<MedicalDocumentDTO>();
            return Ok(dto);
        }

        // GET api/MedicalDocuments/5/Download
        [SwaggerOperation(Summary = "Descarga un documento medico.", Description = "Este endpoint permite descargar un documento medico.")]
        [SwaggerResponse(200, "Archivo descargado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Documento medico no encontrado.")]
        [HttpGet("{id:int}/Download")]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _service.GetFileByIdAsync(id);

            return File(file.Item1, file.Item2, file.Item3);
        }

        // GET api/MedicalDocuments/5/Open
        [SwaggerOperation(Summary = "Abre un documento medico.", Description = "Este endpoint permite abrir un documento medico en el navegador.")]
        [SwaggerResponse(200, "Archivo abierto.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Documento medico no encontrado.")]
        [HttpGet("{id:int}/Open")]
        public async Task<IActionResult> Open(int id)
        {
            var file = await _service.GetFileByIdAsync(id);
            return File(file.Item1, file.Item2);
        }

        // POST api/MedicalDocuments/Upload
        [SwaggerOperation(Summary = "Sube un documento medico.", Description = "Este endpoint permite subir un documento medico.")]
        [SwaggerResponse(201, "Te devuelve el documento medico subido.", typeof(MedicalDocumentDTO))]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpPost("Upload")]
        [RequestSizeLimit(10485760)]
        public async Task<ActionResult<MedicalDocumentDTO>> PostAsync(MedicalDocumentCreateDTO dtoc)
        {
            var result = await _validation.ValidateAsync(dtoc);

            if (!result.IsValid)
            {
                result.Errors.ForEach(x => ModelState.AddModelError($"{x.PropertyName}", $"{x.ErrorMessage}"));
                return ValidationProblem();
            }

            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // DELETE api/MedicalDocuments/5
        [SwaggerOperation(Summary = "Elimina un documento medico.", Description = "Este endpoint elimina un documento medico.")]
        [SwaggerResponse(204, "Documento medico eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(404, "Documento medico no encontrado.")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<MedicalDocumentModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }
    }
}