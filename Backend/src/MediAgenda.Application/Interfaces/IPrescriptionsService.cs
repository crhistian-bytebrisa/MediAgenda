using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.DTOs.Relations;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;

namespace MediAgenda.Application.Services
{
    public interface IPrescriptionsService
    {
        Task<PrescriptionAnalysisDTO> AddAnalysisAsync(PrescriptionAnalysisCreateDTO dtocu);
        Task<PrescriptionDTO> AddAsync(PrescriptionCreateDTO dtoc);
        Task<PrescriptionMedicineDTO> AddMedicineAsync(PrescriptionMedicineCreateDTO dtocu);
        Task<PrescriptionPermissionDTO> AddPermissionAsync(PrescriptionPermissionCreateDTO dtocu);
        Task DeleteAnalysisAsync(int analysisid, int prescriptionid);
        Task DeleteMedicineAsync(int medicineid, int prescriptionid);
        Task DeletePermissionAsync(int permissionid, int prescriptionid);
        Task<List<PrescriptionAnalysisDTO>> GetAllAnalysis(int prescriptionid);
        Task<APIResponse<PrescriptionDTO>> GetAllAsync(PrescriptionRequest request);
        Task<List<PrescriptionMedicineDTO>> GetAllMedicine(int prescriptionid);
        Task<List<PrescriptionPermissionDTO>> GetAllPermission(int prescriptionid);
        Task<PrescriptionModel> GetByIdAsync(int id);
        Task<PrescriptionAnalysisDTO> UpdateAnalysisAsync(PrescriptionAnalysisUpdateDTO dtocu);
        Task<PrescriptionMedicineDTO> UpdateMedicineAsync(PrescriptionMedicineUpdateDTO dtocu);
        Task<PrescriptionPermissionDTO> UpdatePermissionAsync(PrescriptionPermissionUpdateDTO dtocu);
        Task DeleteAsync(PrescriptionModel model);
    }
}