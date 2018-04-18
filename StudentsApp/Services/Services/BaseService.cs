using Core.Database;
using Core.Repositories;
using Services.IServices;
using System;
using System.Data.Entity;

namespace Services.Services
{
    public class BaseService : IBaseService
    {
        public bool IsError { get; set; }
        public DbContext Context { get; set; }

        public BaseService()
        {
            Context = new DataContext();
        }

        public Repository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(Context);
        }

        public int Complete()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (Exception)
            {
                IsError = true;
                return -1;
            }

        }
    }
}
