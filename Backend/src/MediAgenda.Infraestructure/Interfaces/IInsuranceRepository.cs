using MediAgenda.Domain.Core;
using MediAgenda.Infraestructure.Core;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediAgenda.Infraestructure.Interfaces
{
    public interface IInsuranceRepository : IBaseRepositoryIdInt<InsuranceModel>
    {
        Task<List<ListItem>> GetAllNames();
        Task<(List<InsuranceModel>, int)> GetAllAsync(InsuranceRequest request);
    }
}