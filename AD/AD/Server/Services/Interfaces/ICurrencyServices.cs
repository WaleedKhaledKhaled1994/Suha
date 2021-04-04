using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface ICurrencyServices
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> Post(Currency input);
        Task<ApiResponse> Put(Currency input);
        Task<ApiResponse> Delete(long id);
    }
}
