using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Services
{
    public interface IRepository<TEntity>
    {
        TEntity GetById(int id);
        IList<TEntity> GetAll();
        TEntity SaveOrUpdate(TEntity entity);
        IEnumerable<TEntity> SaveOrUpdateList(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
    }
}
