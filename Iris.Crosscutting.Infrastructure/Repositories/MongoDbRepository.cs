using Iris.Crosscutting.Common.Data;
using Iris.Crosscutting.Domain.Interfaces.Repositories;
using Iris.Crosscutting.Domain.Model;
using Iris.Crosscutting.Infrastructure.Contexts.MongoDb;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Iris.Crosscutting.Infrastructure.Repositories
{
    public abstract class MongoRepository<TEntity> : IMongoDbRepository<TEntity> where TEntity : Entity
    {
        protected readonly IMongoDbContext _context;
        protected readonly IMongoCollection<TEntity> _collection;

        protected MongoRepository(string collectionName, IMongoDbContext context)
        {
            _context = context;
            _collection = _context.GetCollection<TEntity>(collectionName);
        }

        public virtual async Task<PagedResult<TEntity>> GetAllPagedAsync(int page, int size,
            Expression<Func<TEntity, bool>> filter = null,
            Func<Expression<Func<TEntity, bool>>, PagedResult<TEntity>> whenNoExists = null,
            ProjectionDefinition<TEntity, TEntity> projection = null)
        {
            var pagedResult = new PagedResult<TEntity>();

            pagedResult.CurrentPage = page;
            pagedResult.PageSize = size;
            pagedResult.TotalRecords = _collection.Find(t => true).CountDocuments();

            var pageCount = (double)pagedResult.TotalRecords / size;
            pagedResult.PageCount = (int)Math.Ceiling(pageCount);

            int skip = (page - 1) * size;

            var cursor = filter == null ? _collection.Find(t => true).Skip(skip).Limit(size)
                : _collection.Find(filter).Skip(skip).Limit(size);

            if (projection != null)
                cursor = cursor.Project(projection);

            pagedResult.Results = await cursor.ToListAsync();

            if (!pagedResult.Results.Any() && whenNoExists != null) return whenNoExists(filter);

            return pagedResult;
        }

        public virtual async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<Expression<Func<TEntity, bool>>, List<TEntity>> whenNoExists = null,
            ProjectionDefinition<TEntity, TEntity> projection = null)
        {
            var cursor = filter == null ? _collection.Find(t => true) : _collection.Find(filter);

            if (projection != null)
            {
                cursor = cursor.Project(projection);
            }

            var result = await cursor.ToListAsync();

            if (!result.Any() && whenNoExists != null) return whenNoExists(filter);

            return result;
        }

        public async Task<TEntity> GetOneAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<Expression<Func<TEntity, bool>>, TEntity> whenNoExists = null)
        {
            var cursor = await _collection.FindAsync(filter);

            var result = await cursor.FirstOrDefaultAsync();

            if (result == null && whenNoExists != null) return whenNoExists(filter);

            return result;
        }

        public virtual Task AddAsync(TEntity newEntity)
        {
            return _collection
                .InsertOneAsync(newEntity);
        }

        public virtual Task UpdateAsync(string key, TEntity newEntity) => UpdateAsync(Guid.Parse(key), newEntity);

        public virtual Task UpdateAsync(Guid key, TEntity newEntity)
        {
            return _collection
                .ReplaceOneAsync(
                    t => t.Id == key,
                    newEntity,
                    new UpdateOptions { IsUpsert = true }
                );
        }

        public virtual Task RemoveAsync(string key) => RemoveAsync(Guid.Parse(key));

        public virtual Task RemoveAsync(Guid key)
        {
            return _collection
                .DeleteOneAsync(t => t.Id == key);
        }

        public virtual Task<TEntity> GetByIdAsync(Guid id) => GetOneAsync(t => t.Id == id);

        public virtual async Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            var reg = await GetOneAsync(expression);

            return reg != null;
        }

        public virtual Task<IClientSessionHandle> StartTransactionAsync()
        {
            return _context.Client.StartSessionAsync();
        }
    }
}