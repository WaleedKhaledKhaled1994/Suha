using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AD.Server.Data;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AD.Server.Repositories
{
    public partial interface IRepository<T> where T : class, IBaseEntity, new()
    {
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        T Insert(T entity);

        Task<T> InsertAsync(T entity);

        void Insert(IEnumerable<T> entities);

        Task<IEnumerable<T>> InsertAsync(IEnumerable<T> entities);
        T Update(T entity);

        Task<T> UpdateAsync(T entity);

        void Update(IEnumerable<T> entities);

        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities);
        void Delete(T entity);

        Task<T> DeleteAsync(T entity);

        void Delete(IEnumerable<T> entities);

        Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities);

        bool Any();

        bool Any(Expression<Func<T, bool>> where);

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<T, bool>> where);

        long Count();

        long Count(Expression<Func<T, bool>> where);

        Task<long> CountAsync();

        Task<long> CountAsync(Expression<Func<T, bool>> where);

        EntityEntry Attach(object entity);

        void Attach(IEnumerable<object> entities);
        List<Target> UpdateOneToManyRelation<Target>(IEnumerable<long> entities, IEnumerable<Target> entitiesToUpdate) ;

        public List<Target> UpdateManyToManyRelation<Target>(IEnumerable<long> entities, IEnumerable<Target> entitiesToUpdate,
            Func<long, Target, bool> targetComparer,
            Func<long, Target> select) where Target : class, new();

        Task AttachAndSaveAsync(T entity);

        void AttachAndSave(T entity);

        Task<int> SaveChangesAsync();

        int SaveChanges();

        List<Target> InsertOneToManyRelation<Target>(IEnumerable<long> entities) where Target : class, IBaseEntity, new();

        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
    }
}
