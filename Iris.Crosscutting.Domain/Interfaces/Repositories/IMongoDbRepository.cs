using Iris.Crosscutting.Common.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Iris.Crosscutting.Domain.Interfaces.Repositories
{
    public interface IMongoDbRepository<TEntity> where TEntity : class
    {
        Task<PagedResult<TEntity>> GetAllPagedAsync(int page, int size,
            Expression<Func<TEntity, bool>> filter = null,
            Func<Expression<Func<TEntity, bool>>, PagedResult<TEntity>> whenNoExists = null,
            ProjectionDefinition<TEntity, TEntity> projection = null);

        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<Expression<Func<TEntity, bool>>, List<TEntity>> whenNoExists = null,
            ProjectionDefinition<TEntity, TEntity> projection = null);

        Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<Expression<Func<TEntity, bool>>, TEntity> whenNoExists = null);

        Task AddAsync(TEntity newEntity);

        Task UpdateAsync(string key, TEntity newEntity);

        Task UpdateAsync(Guid key, TEntity newEntity);

        Task RemoveAsync(string key);

        Task RemoveAsync(Guid key);

        Task<TEntity> GetByIdAsync(Guid id);

        Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression);

        Task<IClientSessionHandle> StartTransactionAsync();
    }
}