using Mapster;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.DTOs.Relations;
using MediAgenda.Application.Interfaces;
using MediAgenda.Application.Services;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/Prescriptions")]
    [ApiController]
    [Authorize]
    [Authorize(Roles = "Admin,Doctor")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionsService _service;

        public PrescriptionsController(IPrescriptionsService service)
        {
            _service = service;
        }

        // GET: api/Prescriptions
        [SwaggerOperation(Summary = "Obtiene las prescripciones.", Description = "Este endpoint se creo para obtener las prescripciones en general.")]
        [SwaggerResponse(200, "Te devuelve las prescripciones en un JSON de paginacion.", typeof(APIResponse<PrescriptionDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet]
        public async Task<ActionResult<APIResponse<PrescriptionDTO>>> Get([FromQuery] PrescriptionRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Prescriptions/5
        [SwaggerOperation(Summary = "Obtiene la prescripcion en especifico.", Description = "Este endpoint trae una prescripcion con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve una prescripcion.", typeof(PrescriptionDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Prescripcion no encontrada.")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PrescriptionDTO>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            PrescriptionDTO dto = entity.Adapt<PrescriptionDTO>();
            return Ok(dto);
        }

        // POST api/Prescriptions
        [SwaggerOperation(Summary = "Agrega una prescripcion al sistema.", Description = "Este endpoint crea una prescripcion.")]
        [SwaggerResponse(201, "Te devuelve la prescripcion creada.", typeof(PrescriptionDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost]
        public async Task<ActionResult<PrescriptionDTO>> PostAsync([FromBody] PrescriptionCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        [SwaggerOperation(Summary = "Actualiza parcialmente una prescripcion.", Description = "Este endpoint actualiza parcialmente una prescripcion.")]
        [SwaggerResponse(501, "No implementado.")]
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<DoctorDTO>> PatchAsync(int id)
        {
            return null;
        }

        // DELETE api/Prescriptions/5
        [SwaggerOperation(Summary = "Elimina una prescripcion.", Description = "Este endpoint elimina una prescripcion.")]
        [SwaggerResponse(204, "Prescripcion eliminada.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Prescripcion no encontrada.")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<PrescriptionModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }

        #region Medicinas

        [SwaggerOperation(Summary = "Obtiene los medicamentos de una prescripcion.", Description = "Este endpoint trae los medicamentos de una prescripcion especifica.")]
        [SwaggerResponse(200, "Te devuelve los medicamentos.", typeof(List<PrescriptionMedicineDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet("{id:int}/Medicines")]
        //GET api/Prescriptions/5/Medicines
        public async Task<ActionResult<List<PrescriptionMedicineDTO>>> GetMedicineInPrescription(int id)
        {
            return await _service.GetAllMedicine(id);
        }


        // POST api/Prescriptions/5/Medicines
        [SwaggerOperation(Summary = "Agrega un medicamento a una prescripcion.", Description = "Este endpoint agrega un medicamento a una prescripcion.")]
        [SwaggerResponse(200, "Medicamento agregado.", typeof(PrescriptionMedicineDTO))]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost("{id:int}/Medicines")]
        public async Task<ActionResult<PrescriptionMedicineDTO>> AddMedicineInPrescription(int id, PrescriptionMedicineCreateDTO dtocu)
        {
            if (dtocu.PrescriptionId != id)
            {
                return BadRequest("Deberias de poner el id igual.");
            }

            var entity = await _service.AddMedicineAsync(dtocu);

            return Ok(entity);
        }

        // PUT api/Prescription/5/Medicines/1
        [SwaggerOperation(Summary = "Actualiza un medicamento en una prescripcion.", Description = "Este endpoint actualiza un medicamento en una prescripcion.")]
        [SwaggerResponse(200, "Medicamento actualizado.", typeof(PrescriptionMedicineDTO))]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPut("{id:int}/Medicines/{idmedi:int}")]
        public async Task<ActionResult<PrescriptionMedicineDTO>> UpdateMedicineInPrescription(int id, int idmedi, PrescriptionMedicineUpdateDTO dtocu)
        {
            if (dtocu.PrescriptionId != id || dtocu.MedicineId == idmedi)
            {
                return BadRequest("Deberias de poner el id igual.");
            }

            var entity = await _service.UpdateMedicineAsync(dtocu);

            return Ok(entity);
        }



        // DELETE api/Prescription/5/Medicines/1
        [SwaggerOperation(Summary = "Elimina un medicamento de una prescripcion.", Description = "Este endpoint elimina un medicamento de una prescripcion.")]
        [SwaggerResponse(204, "Medicamento eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpDelete("{id:int}/Medicines/{idmedi:int}")]
        public async Task<ActionResult> DeleteMedicineInPrescription(int id, int idmedi)
        {
            await _service.DeleteMedicineAsync(idmedi, id);

            return NoContent();
        }

        #endregion

        #region Analisis

        [SwaggerOperation(Summary = "Obtiene los analisis de una prescripcion.", Description = "Este endpoint trae los analisis de una prescripcion especifica.")]
        [SwaggerResponse(200, "Te devuelve los analisis.", typeof(List<PrescriptionAnalysisDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet("{id:int}/Analysis")]
        //GET api/Prescriptions/5/Analysis
        public async Task<ActionResult<List<PrescriptionAnalysisDTO>>> GetAnalysisInPrescription(int id)
        {
            return await _service.GetAllAnalysis(id);
        }

        // POST api/Prescriptions/5/Analysis
        [SwaggerOperation(Summary = "Agrega un analisis a una prescripcion.", Description = "Este endpoint agrega un analisis a una prescripcion.")]
        [SwaggerResponse(200, "Analisis agregado.", typeof(PrescriptionAnalysisDTO))]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost("{id:int}/Analysis")]
        public async Task<ActionResult<PrescriptionAnalysisDTO>> AddAnalysisInPrescription(int id, [FromBody] PrescriptionAnalysisCreateDTO dtocu)
        {
            if (dtocu.PrescriptionId != id)
            {
                return BadRequest("Deberias de poner el id igual.");
            }

            var entity = await _service.AddAnalysisAsync(dtocu);
            return Ok(entity);
        }

        // PUT api/Prescriptions/5/Analysis/1
        [SwaggerOperation(Summary = "Actualiza un analisis en una prescripcion.", Description = "Este endpoint actualiza un analisis en una prescripcion.")]
        [SwaggerResponse(200, "Analisis actualizado.", typeof(PrescriptionAnalysisDTO))]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPut("{id:int}/Analysis/{idanalysis:int}")]
        public async Task<ActionResult<PrescriptionAnalysisDTO>> UpdateAnalysisInPrescription(int id, int idanalysis, [FromBody] PrescriptionAnalysisUpdateDTO dtocu)
        {
            if (dtocu.PrescriptionId != id || dtocu.AnalysisId != idanalysis)
            {
                return BadRequest("Deberias de poner el id igual.");
            }

            var entity = await _service.UpdateAnalysisAsync(dtocu);
            return Ok(entity);
        }

        // DELETE api/Prescriptions/5/Analysis/1
        [SwaggerOperation(Summary = "Elimina un analisis de una prescripcion.", Description = "Este endpoint elimina un analisis de una prescripcion.")]
        [SwaggerResponse(204, "Analisis eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpDelete("{id:int}/Analysis/{idanalysis:int}")]
        public async Task<ActionResult> DeleteAnalysisInPrescription(int id, int idanalysis)
        {
            await _service.DeleteAnalysisAsync(idanalysis, id);
            return NoContent();
        }


        #endregion

        #region Permisos

        [SwaggerOperation(Summary = "Obtiene los permisos de una prescripcion.", Description = "Este endpoint trae los permisos de una prescripcion especifica.")]
        [SwaggerResponse(200, "Te devuelve los permisos.", typeof(List<PrescriptionPermissionDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet("{id:int}/Permissions")]
        //GET api/Prescriptions/5/Permissions
        public async Task<ActionResult<List<PrescriptionPermissionDTO>>> GetPermissionsInPrescription(int id)
        {
            return await _service.GetAllPermission(id);
        }

        // POST api/Prescriptions/5/Permissions
        [SwaggerOperation(Summary = "Agrega un permiso a una prescripcion.", Description = "Este endpoint agrega un permiso a una prescripcion.")]
        [SwaggerResponse(200, "Permiso agregado.", typeof(PrescriptionPermissionDTO))]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPost("{id:int}/Permissions")]
        public async Task<ActionResult<PrescriptionPermissionDTO>> AddPermissionInPrescription(int id, [FromBody] PrescriptionPermissionCreateDTO dtocu)
        {
            if (dtocu.PrescriptionId != id)
            {
                return BadRequest("Deberias de poner el id igual.");
            }

            var entity = await _service.AddPermissionAsync(dtocu);
            return Ok(entity);
        }

        // PUT api/Prescriptions/5/Permissions/1
        [SwaggerOperation(Summary = "Actualiza un permiso en una prescripcion.", Description = "Este endpoint actualiza un permiso en una prescripcion.")]
        [SwaggerResponse(200, "Permiso actualizado.", typeof(PrescriptionPermissionDTO))]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpPut("{id:int}/Permissions/{idpermission:int}")]
        public async Task<ActionResult<PrescriptionPermissionDTO>> UpdatePermissionInPrescription(int id, int idpermission, [FromBody] PrescriptionPermissionUpdateDTO dtocu)
        {
            if (dtocu.PrescriptionId != id || dtocu.PermissionId != idpermission)
            {
                return BadRequest("Deberias de poner el id igual.");
            }

            var entity = await _service.UpdatePermissionAsync(dtocu);
            return Ok(entity);
        }

        // DELETE api/Prescriptions/5/Permissions/1
        [SwaggerOperation(Summary = "Elimina un permiso de una prescripcion.", Description = "Este endpoint elimina un permiso de una prescripcion.")]
        [SwaggerResponse(204, "Permiso eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpDelete("{id:int}/Permissions/{idpermission:int}")]
        public async Task<ActionResult> DeletePermissionInPrescription(int id, int idpermission)
        {
            await _service.DeletePermissionAsync(idpermission, id);
            return NoContent();
        }
        #endregion
    }
}