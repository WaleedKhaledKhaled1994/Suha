using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> Get();
        Task<PaginatedResponse<List<Tag>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<Tag> Get(long id);
        Task<long> Create(AddTagVMRequest entity);
        Task Edit(EditTagVMRequest entity);
        Task Delete(long id);
    }
}
