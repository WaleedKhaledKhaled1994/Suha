using Microsoft.EntityFrameworkCore;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels.MeasruingUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class MeasruingUnitServices : IMeasruingUnitServices
    {
        private readonly IRepository<MeasruingUnit> _entityRepository;

        public MeasruingUnitServices(IRepository<MeasruingUnit> entityRepository)
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

        public async Task<ApiResponse> Post(AddMeasruingUnitVMRequest entity)
        {
            try
            {
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                MeasruingUnit measruingUnit = new()
                {
                    Name_ar = entity.Name_ar,
                    Name_en = entity.Name_en,
                    Name_fr = entity.Name_fr,
                    Name_tr = entity.Name_tr,
                    Name_ru = entity.Name_ru,
                    Symbol = entity.Symbol,
                    Code = $"MU_{max + 1}"
                };
                var newEntity = await _entityRepository.InsertAsync(measruingUnit);
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Put(EditMeasruingUnitVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
                entityDB.Name_ar = entity.Name_ar;
                entityDB.Name_en = entity.Name_en;
                entityDB.Name_fr = entity.Name_fr;
                entityDB.Name_ru = entity.Name_ru;
                entityDB.Name_tr = entity.Name_tr;
                entityDB.Symbol = entity.Symbol;
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
