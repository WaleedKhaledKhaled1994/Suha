using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class CityServices : ICityServices
    {
        private readonly IRepository<City> _entityRepository;

        public CityServices(IRepository<City> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<ApiResponse> Get()
        {
            try
            {
                var query = _entityRepository.TableNoTracking.Where(x => x.Status == Status.Online).Include(x => x.Country);
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
                var entity = await _entityRepository.TableNoTracking.Include(x => x.Country).SingleOrDefaultAsync(x => x.Id == id);
                return ApiResponse.Create(HttpStatusCode.OK, entity);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Post(AddCityVMRequest entity)
        {
            try
            {
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                City city = new()
                {
                    Name_ar = entity.Name_ar,
                    Name_en = entity.Name_en,
                    Name_fr = entity.Name_fr,
                    Name_tr = entity.Name_tr,
                    Name_ru = entity.Name_ru,
                    CountryId = entity.CountryId,
                    Code = $"City_{max + 1}"
                };
                var newEntity = await _entityRepository.InsertAsync(city);
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Put(EditCityVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
                entityDB.Name_tr = entity.Name_tr;
                entityDB.Name_ru = entity.Name_ru;
                entityDB.Name_fr = entity.Name_fr;
                entityDB.Name_en = entity.Name_en;
                entityDB.Name_ar = entity.Name_ar;
                entityDB.CountryId = entity.CountryId;
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
