using MediAgenda.Infraestructure.Core;
using MediAgenda.Infraestructure.Models;
using MediAgenda.Infraestructure.RequestRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediAgenda.Infraestructure.Interfaces
{
    public interface IAnalysisRepository : IBaseRepositoryIdInt<AnalysisModel>
    {
        Task<(List<AnalysisModel>, int)> GetAllAsync(AnalysisRequest request);
        Task<List<string>> GetAllNames();
    }
}