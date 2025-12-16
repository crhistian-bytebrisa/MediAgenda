using MediAgenda.Infraestructure.Core;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediAgenda.Infraestructure.Interfaces
{
    public interface IMedicineRepository : IBaseRepositoryIdInt<MedicineModel>
    {
        Task<List<string>> GetAllNames();
        Task<(List<MedicineModel>, int)> GetAllAsync(MedicineRequest request);
    }
}