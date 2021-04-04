using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class CountryServices : ICountryServices
    {
        private readonly IRepository<Country> _entityRepository;
        private readonly IFileStorageService _fileService;
        private const string path = "countries";
        public CountryServices(IRepository<Country> countryServices, IFileStorageService fileService)
        {
            _entityRepository = countryServices;
            _fileService = fileService;
        }

        public async Task<ApiResponse> Get()
        {
            try
            {
                var query = _entityRepository.TableNoTracking.Where(x => x.Status == Status.Online).OrderByDescending(x => x.Id);
                var entities = await query.ToListAsync();
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
        public async Task<ApiResponse> Post(AddCountryVMRequest entity)
        {
            try
            {
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                Country country = new()
                {
                    Name_ar = entity.Name_ar,
                    Name_en = entity.Name_en,
                    Name_fr = entity.Name_fr,
                    Name_tr = entity.Name_tr,
                    Name_ru = entity.Name_ru,
                    Image = entity.Image,
                    Code = $"Country_{max + 1}"
                };
                if (!string.IsNullOrWhiteSpace(entity.Image))
                {
                    var entityImage = Convert.FromBase64String(entity.Image);
                    country.Image = await _fileService.SaveFile(entityImage, "jpg", path, country.Code);
                }
                var newEntity = await _entityRepository.InsertAsync(country);
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Put(EditCountryVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);

                if (!string.IsNullOrWhiteSpace(entity.Image))
                {
                    if (entityDB.Image != null)
                        await _fileService.DeleteFile(entityDB.Image, path);

                    var entityImage = Convert.FromBase64String(entity.Image);
                    entityDB.Image = await _fileService.EditFile(entityImage, "jpg", path, entityDB.Image, entityDB.Code);
                }
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

                if (!string.IsNullOrWhiteSpace(entity.Image))
                    await _fileService.DeleteFile(entity.Image, path);

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

