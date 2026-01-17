using MediAgenda.Domain.Core;
using MediAgenda.Infraestructure.Core;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediAgenda.Infraestructure.Interfaces
{
    public interface IReasonRepository : IBaseRepositoryIdInt<ReasonModel>
    {
        Task<List<ListItem>> GetAllNames();
        Task<(List<ReasonModel>, int)> GetAllAsync(ReasonRequest request);
    }
}