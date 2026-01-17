using Mapster;
using MediAgenda.API.Filters;
using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.DTOs.Relations;
using MediAgenda.Application.Interfaces;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/Patients")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsService _service;

        public PatientsController(IPatientsService service)
        {
            _service = service;
        }
        // GET: api/Patients
        [SwaggerOperation(Summary = "Obtiene los pacientes.", Description = "Este endpoint se creo para obtener los pacientes en general, solo accesible para Doctores y Administradores.")]
        [SwaggerResponse(200, "Te devuelve los pacientes en un JSON de paginacion.", typeof(APIResponse<PatientDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [HttpGet]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<APIResponse<PatientDTO>>> Get([FromQuery] PatientRequest request)
        {
            var APIR = await _service.GetAllAsync(request);
            return Ok(APIR);
        }

        // GET api/Patients/5
        [SwaggerOperation(Summary = "Obtiene el paciente en especifico.", Description = "Este endpoint trae un paciente con todos los datos relacionados al mismo.")]
        [SwaggerResponse(200, "Te devuelve un paciente.", typeof(PatientDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No tienes permisos.")]
        [SwaggerResponse(404, "Paciente no encontrado.")]
        [HttpGet("{id:int}")]
        [AuthorizeSamePatientIdOrRoles("id","Doctor","Admin")]
        public async Task<ActionResult<PatientDTO>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            PatientDTO dto = entity.Adapt<PatientDTO>();
            return Ok(dto);
        }

        // GET api/Patients/5/Notes
        [SwaggerOperation(Summary = "Obtiene las notas de un paciente.", Description = "Este endpoint trae las notas de un paciente especifico.")]
        [SwaggerResponse(200, "Te devuelve las notas en un JSON de paginacion.", typeof(APIResponse<NotePatientDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin o Doctor")]
        [SwaggerResponse(404, "Paciente no encontrado.")]
        [HttpGet("{id:int}/Notes")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<APIResponse<NotePatientDTO>>> GetNotes(int id, [FromQuery] PatientNoteRequest request)
        {
            var dto = await _service.GetAllNotes(id, request);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        // GET api/Patients/5/Consultations
        [SwaggerOperation(Summary = "Obtiene las consultas de un paciente.", Description = "Este endpoint trae las consultas de un paciente especifico.")]
        [SwaggerResponse(200, "Te devuelve las consultas en un JSON de paginacion.", typeof(APIResponse<ConsultationDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No tienes permisos.")]
        [SwaggerResponse(404, "Paciente no encontrado.")]
        [HttpGet("{id:int}/Consultations")]
        [AuthorizeSamePatientIdOrRoles("id", "Doctor", "Admin")]
        public async Task<ActionResult<APIResponse<ConsultationDTO>>> GetConsultations(int id, [FromQuery] PatientConsultationRequest request)
        {
            var dto = await _service.GetAllConsultations(id, request);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        // GET api/Patients/5/Documents
        [SwaggerOperation(Summary = "Obtiene los documentos de un paciente.", Description = "Este endpoint trae los documentos medicos de un paciente especifico.")]
        [SwaggerResponse(200, "Te devuelve los documentos en un JSON de paginacion.", typeof(APIResponse<MedicalDocumentDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No tienes permisos.")]
        [SwaggerResponse(404, "Paciente no encontrado.")]
        [HttpGet("{id:int}/Documents")]
        [AuthorizeSamePatientIdOrRoles("id", "Doctor", "Admin")]
        public async Task<ActionResult<APIResponse<MedicalDocumentDTO>>> GetDocuments(int id, [FromQuery] PatientMedicalDocumentRequest request)
        {
            var dto = await _service.GetAllMedicalDocuments(id, request);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        // GET api/Patients/5/Medicines
        [SwaggerOperation(Summary = "Obtiene los medicamentos de un paciente.", Description = "Este endpoint trae los medicamentos de un paciente especifico.")]
        [SwaggerResponse(200, "Te devuelve los medicamentos en un JSON de paginacion.", typeof(APIResponse<PrescriptionMedicineDTO>))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No tienes permisos.")]
        [SwaggerResponse(404, "Paciente no encontrado.")]
        [HttpGet("{id:int}/Medicines")]
        [AuthorizeSamePatientIdOrRoles("id", "Doctor", "Admin")]
        public async Task<ActionResult<APIResponse<PrescriptionMedicineDTO>>> GetMedicines(int id, [FromQuery] PatientMedicamentRequest request)
        {
            var dto = await _service.GetAllMedicaments(id, request);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        // POST api/Patients
        [SwaggerOperation(Summary = "Agrega un paciente al sistema.", Description = "Este endpoint crea un paciente solo si eres Admin.")]
        [SwaggerResponse(201, "Te devuelve el paciente creado.", typeof(PatientDTO))]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PatientDTO>> PostAsync([FromBody] PatientCreateDTO dtoc)
        {
            var dto = await _service.AddAsync(dtoc);
            return CreatedAtAction(actionName: nameof(Get), routeValues: new { id = dto.Id }, value: dto);
        }

        // PUT api/Patients/5
        [SwaggerOperation(Summary = "Actualiza un paciente.", Description = "Este endpoint actualiza un paciente solo si eres Admin.")]
        [SwaggerResponse(204, "Paciente actualizado.")]
        [SwaggerResponse(400, "Datos invalidos.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] PatientUpdateDTO dtou)
        {

            if (ModelState.ErrorCount > 0)
            {
                return ValidationProblem();
            }

            await _service.UpdateAsync(dtou);
            return NoContent();
        }

        // DELETE api/Patients/5
        [SwaggerOperation(Summary = "Elimina un paciente.", Description = "Este endpoint elimina un paciente solo si eres Admin.")]
        [SwaggerResponse(204, "Paciente eliminado.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [SwaggerResponse(403, "No eres Admin")]
        [SwaggerResponse(404, "Paciente no encontrado.")]
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            var model = dto.Adapt<PatientModel>();
            await _service.DeleteAsync(model);
            return NoContent();
        }

    }
}
