using MediAgenda.Infraestructure.Core;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediAgenda.Infraestructure.Interfaces
{
    public interface IPermissionRepository : IBaseRepositoryIdInt<PermissionModel>
    {
        Task<List<string>> GetAllNames();
        Task<(List<PermissionModel>, int)> GetAllAsync(PermissionRequest request);
    }
}