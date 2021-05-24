using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<Language> GetCurrentLanguageAsync();
    }
}