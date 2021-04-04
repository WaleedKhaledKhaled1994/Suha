using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class AuditServices : IAuditServices
    {
        private readonly IRepository<Audit> _entityRepository;

        public AuditServices(IRepository<Audit> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<List<Audit>> Get()
        {
            var query = _entityRepository.TableNoTracking;
            var entities = await query.OrderByDescending(x => x.Id).ToListAsync();
            //var entities = await query.ToListAsync();
            return entities;
        }

        public async Task<Audit> Get(long id)
        {
            var entity = await _entityRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<long> Post(Audit entity)
        {
            var newEntity = await _entityRepository.InsertAsync(entity);
            return newEntity.Id;
        }

        public async Task Put(Audit entity)
        {
            var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
            entityDB.UserId = entity.UserId;
            entityDB.Type = entity.Type;
            entityDB.TableName = entity.TableName;
            entityDB.DateTime = entity.DateTime;
            entityDB.OldValues = entity.OldValues;
            entityDB.NewValues = entity.NewValues;
            entityDB.AffectedColumns = entity.AffectedColumns;
            entityDB.PrimaryKey = entity.PrimaryKey;

            await _entityRepository.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await _entityRepository.GetByIdAsync(id);
            await _entityRepository.DeleteAsync(entity);
        }
    }
}
