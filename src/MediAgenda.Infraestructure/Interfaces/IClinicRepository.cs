using MediAgenda.Domain.Core;
using MediAgenda.Infraestructure.Core;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediAgenda.Infraestructure.Interfaces
{
    public interface IClinicRepository : IBaseRepositoryIdInt<ClinicModel>
    {
        Task<List<ListItem>> GetAllNames();
        Task<(List<ClinicModel>, int)> GetAllAsync(ClinicRequest request);
        Task<(List<DayAvailableModel>, int)> GetAllDaysAvailableById(int id, ClinicDaysAvailableRequest request);
    }
}