﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        public bool IsError { get; private set; }
        private bool _disposed = false;


        public Repository(DbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> GetQueryable
            (
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                int? skip = null,
                int? take = null,
                params Expression<Func<TEntity, object>>[] includes
            )
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        #region Read Only Operations


        public IEnumerable<TEntity> GetAll
        (
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                int? skip = default(int?),
                int? take = default(int?),
                params Expression<Func<TEntity, object>>[] includes
        )
        {
            return GetQueryable
            (
                orderBy: orderBy,
                skip: skip,
                take: take,
                includes: includes
            ).ToList();
        }

        public IEnumerable<TEntity> Get
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = default(int?),
            int? take = default(int?),
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            return GetQueryable
            (
                filter: filter,
                orderBy: orderBy,
                skip: skip,
                take: take,
                includes: includes
            ).ToList();
        }

        public TEntity GetById(object Id)
        {
            return Context.Set<TEntity>().Find(Id);
        }

        public TEntity GetFirst
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            return GetQueryable
            (
                filter: filter,
                orderBy: orderBy,
                includes: includes
            ).FirstOrDefault();
        }

        public TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetQueryable
            (
                filter: filter,
                includes: includes
            ).SingleOrDefault();
        }

        public int? GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable
            (
                filter: filter
            ).Count();
        }

        public decimal? GetSum(Expression<Func<TEntity, decimal?>> sumProperty, Expression<Func<TEntity, bool>> filter = null)
        {

            return GetQueryable
            (
                filter: filter
            ).Sum(sumProperty);
        }

        public bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            return GetQueryable(filter: filter).Any();
        }

        #endregion

        #region CRUD Operations

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        #endregion


        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
