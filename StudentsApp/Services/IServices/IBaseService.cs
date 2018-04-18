using Core.Repositories;
using System.Data.Entity;

namespace Services.IServices
{
    public interface IBaseService
    {
        DbContext Context { get; set; }
        Repository<TEntity> GetRepository<TEntity>() where TEntity : class;
        bool IsError { get; set; }

        int Complete();

    }
}
