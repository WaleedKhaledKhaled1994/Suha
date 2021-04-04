using AD.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AD.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AD.Shared.ViewModels.Tag;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.Base;
using System.Net;

namespace AD.Server.Services.Interfaces
{
    public class TagServices : ITagServices
    {
        private readonly IRepository<Tag> _entityRepository;
        public TagServices(IRepository<Tag> entityRepository)
        {
            _entityRepository = entityRepository;
        }
        public async Task<ApiResponse> Get()
        {
            try
            {
                var query = _entityRepository.TableNoTracking.Where(x => x.Status == Status.Online);
                var entities = await query.OrderByDescending(x => x.Id).ToListAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entities);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Get(long id)
        {
            try
            {
                var entity = await _entityRepository.GetByIdAsync(id);
                return ApiResponse.Create(HttpStatusCode.OK, entity);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Post(AddTagVMRequest entity)
        {
            try
            {
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                Tag tag = new()
                {
                    Name_ar = entity.Name_ar,
                    Name_en = entity.Name_en,
                    Name_fr = entity.Name_fr,
                    Name_tr = entity.Name_tr,
                    Name_ru = entity.Name_ru,
                    Code = $"Tag_{max + 1}"
                };
                var newEntity = await _entityRepository.InsertAsync(tag);
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }

        }
        public async Task<ApiResponse> Put(EditTagVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
                entityDB.Name_ar = entity.Name_ar;
                entityDB.Name_en = entity.Name_en;
                entityDB.Name_fr = entity.Name_fr;
                entityDB.Name_ru = entity.Name_ru;
                entityDB.Name_tr = entity.Name_tr;
                entityDB.Status = entity.Status;
                await _entityRepository.UpdateAsync(entityDB);

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Delete(long id)
        {
            try
            {
                var entity = await _entityRepository.GetByIdAsync(id);
                await _entityRepository.DeleteAsync(entity);

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
    }
}