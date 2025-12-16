using MediAgenda.Domain.Core;
using MediAgenda.Domain.Entities;
using MediAgenda.Domain.Entities.Relations;
using MediAgenda.Infraestructure.Context;
using MediAgenda.Infraestructure.Core;
using MediAgenda.Infraestructure.Interfaces;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.Models.Relations;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAgenda.Infraestructure.Repositories
{
    public class PrescriptionRepository : BaseRepositoryIdInt<PrescriptionModel>, IPrescriptionRepository
    {
        public PrescriptionRepository(MediContext context) : base(context)
        {
        }

        #region Analisis
        public async Task<List<PrescriptionAnalysisModel>> GetAnalysisInPrescription(int PrescriptionId)
        {
            var ids = await _context.Set<PrescriptionAnalysisModel>()
                .Where(x => x.PrescriptionId == PrescriptionId)
                .Select(x => x.AnalysisId)
                .ToListAsync();

            var analyses = await _context.Set<PrescriptionAnalysisModel>()
                .Where(x => ids.Contains(x.AnalysisId))
                .Include(x => x.Analysis)
                .ToListAsync();

            return analyses;
        }

        public async Task<PrescriptionAnalysisModel> AddAnalysisInPrescription(PrescriptionAnalysisModel entity)
        {
            _context.Set<PrescriptionAnalysisModel>()
                .Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<PrescriptionAnalysisModel> UpdateAnalysisInPrescription(PrescriptionAnalysisModel entity)
        {
            _context.Set<PrescriptionAnalysisModel>()
                .Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAnalysisInPrescription(int AnalysisId, int PrescriptionId)
        {
            var entity = _context.Set<PrescriptionAnalysisModel>()
                .FirstOrDefaultAsync(x => x.AnalysisId == AnalysisId && x.PrescriptionId == PrescriptionId);

            _context.Remove(entity);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Medicinas
        public async Task<List<PrescriptionMedicineModel>> GetMedicineInPrescription(int PrescriptionId)
        {
            var ids = await _context.Set<PrescriptionMedicineModel>()
                .Where(x => x.PrescriptionId == PrescriptionId)
                .Select(x => x.MedicineId)
                .ToListAsync();

            var medicines = await _context.Set<PrescriptionMedicineModel>()
                .Where(x => ids.Contains(x.MedicineId))
                .Include(x => x.Medicine)
                .ToListAsync();

            return medicines;
        }

        public async Task<PrescriptionMedicineModel> AddMedicineInPrescription(PrescriptionMedicineModel entity)
        {
            _context.Set<PrescriptionMedicineModel>()
                .Add(entity);

            int consultationId = await _context.Set<PrescriptionModel>()
                .Where(x => x.Id == entity.PrescriptionId)
                .Select(x => x.ConsultationId)
                .FirstOrDefaultAsync();

            var patientId = await _context.Set<ConsultationModel>()
                .Where(x => x.Id == consultationId)
                .Select(x => x.PatientId)
                .FirstOrDefaultAsync();

            var cm = new HistoryMedicamentsModel
            {
                PatientId = patientId,
                MedicineId = entity.MedicineId,
                StartMedication = entity.StartDosage,
                EndMedication = entity.EndDosage
            };

            await _context.HystoryMedicaments.AddAsync(cm);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PrescriptionMedicineModel> UpdateMedicineInPrescription(PrescriptionMedicineModel entity)
        {
            var model = await _context.Set<PrescriptionMedicineModel>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MedicineId == entity.MedicineId && x.PrescriptionId == entity.PrescriptionId);

            var medicineHistory = await _context.Set<HistoryMedicamentsModel>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    entity.MedicineId == x.MedicineId
                    && x.StartMedication == entity.StartDosage
                    && x.EndMedication == entity.EndDosage
                );

            if (medicineHistory != null)
            {
                medicineHistory.StartMedication = entity.StartDosage;
                medicineHistory.EndMedication = entity.EndDosage;
                _context.Set<HistoryMedicamentsModel>()
                    .Update(medicineHistory);
            }

            _context.Set<PrescriptionMedicineModel>()
                .Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteMedicineInPrescription(int MedicineId, int PrescriptionId)
        {
            var entity = await _context.Set<PrescriptionMedicineModel>()
                .FirstOrDefaultAsync(x => x.MedicineId == MedicineId && x.PrescriptionId == PrescriptionId);


            var medicineHistory = await _context.Set<HistoryMedicamentsModel>()
                .FirstOrDefaultAsync(x => 
                    entity.MedicineId == x.MedicineId 
                    && x.StartMedication == entity.StartDosage
                    && x.EndMedication == entity.EndDosage
                );

            if (medicineHistory != null)
            {
                _context.Remove(medicineHistory);
            }

            _context.Remove(entity);

            await _context.SaveChangesAsync();
        }

        #endregion

        #region Permisos
        public async Task<List<PrescriptionPermissionModel>> GetPermissionInPrescription(int PrescriptionId)
        {
            var ids = await _context.Set<PrescriptionPermissionModel>()
                .Where(x => x.PrescriptionId == PrescriptionId)
                .Select(x => x.PermissionId)
                .ToListAsync();

            var permissions = await _context.Set<PrescriptionPermissionModel>()
                .Where(x => ids.Contains(x.PrescriptionId))
                .Include(x => x.Permission)
                .ToListAsync();

            return permissions;
        }

        public async Task<PrescriptionPermissionModel> AddPermissionInPrescription(PrescriptionPermissionModel entity)
        {
            _context.Set<PrescriptionPermissionModel>()
                .Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<PrescriptionPermissionModel> UpdatePermissionInPrescription(PrescriptionPermissionModel entity)
        {
            _context.Set<PrescriptionPermissionModel>()
                .Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeletePermissionInPrescription(int PermissionId, int PrescriptionId)
        {
            var entity = _context.Set<PrescriptionPermissionModel>()
                .FirstOrDefaultAsync(x => x.PermissionId == PermissionId && x.PrescriptionId == PrescriptionId);

            _context.Remove(entity);

            await _context.SaveChangesAsync();
        }
        #endregion

        public async Task<(List<PrescriptionModel>, int)> GetAllAsync(PrescriptionRequest request)
        {
            IQueryable<PrescriptionModel> query = _context.Set<PrescriptionModel>()
                .Include(x => x.PrescriptionAnalysis)
                    .ThenInclude(pa => pa.Analysis)
                .Include(x => x.PrescriptionMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .Include(x => x.PrescriptionPermissions)
                    .ThenInclude(pp => pp.Permission);


            if (request.PatientId is not null)
            {
                query = query.Where(x => x.Consultation.PatientId == request.PatientId);
            }

            if (request.CreatedFrom is not null)
            {
                query = query.Where(x => x.CreatedAt >= request.CreatedFrom);
            }

            if (request.CreatedTo is not null)
            {
                query = query.Where(x => x.CreatedAt <= request.CreatedTo);
            }

            if (request.LastPrintFrom is not null)
            {
                query = query.Where(x => x.LastPrint >= request.LastPrintFrom);
            }

            if (request.LastPrintTo is not null)
            {
                query = query.Where(x => x.LastPrint <= request.LastPrintTo);
            }

            if (request.IncludeConsultation is true)
            {
                query = query.Include(x => x.Consultation);
            }

            return await query.PaginateAsync(request);
        }

        public override Task<PrescriptionModel> GetByIdAsync(int id)
        {
            return _context.Set<PrescriptionModel>()
                .Include(x => x.Consultation)
                .Include(x => x.PrescriptionAnalysis)
                .ThenInclude(pa => pa.Analysis)
                .Include(x => x.PrescriptionMedicines)
                .ThenInclude(pm => pm.Medicine)
                .Include(x => x.PrescriptionPermissions)
                .ThenInclude(pp => pp.Permission)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<PrescriptionModel> AddAsync(PrescriptionModel entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            return await base.AddAsync(entity);
        }

        public async Task PrintPrescription(int id)
        {
            var entity = await _context.Set<PrescriptionModel>().AsTracking().FirstOrDefaultAsync(x => x.Id == id);
            entity.LastPrint = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}