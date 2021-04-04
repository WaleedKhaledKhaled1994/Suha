using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class ErrorLogsServices : IErrorLogsServices
    {
        private readonly IRepository<WebApiLogs> _entityRepository;

        public ErrorLogsServices(IRepository<WebApiLogs> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<List<WebApiLogs>> Get()
        {
            var query = _entityRepository.TableNoTracking;
            var entities = await query.OrderByDescending(x => x.Id).ToListAsync();
            return entities;
        }

        public async Task<WebApiLogs> Get(long id)
        {
            var entity = await _entityRepository.GetByIdAsync(id);
            return entity;
        }

        //public async Task<long> Post(WebApiLogs entity)
        //{
        //    var newEntity = await _entityRepository.InsertAsync(entity);
        //    return newEntity.Id;
        //}

        //public async Task Put(WebApiLogs entity)
        //{
        //    var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
        //    entityDB = _mapper.Map(entity, entityDB);
        //    await _entityRepository.UpdateAsync(entityDB);
        //}

        public async Task Delete(long id)
        {
            var entity = await _entityRepository.GetByIdAsync(id);
            await _entityRepository.DeleteAsync(entity);
        }
    }
}
