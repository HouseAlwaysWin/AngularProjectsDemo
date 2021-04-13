using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<Language> GetCurrentLanguageAsync();
    }
}