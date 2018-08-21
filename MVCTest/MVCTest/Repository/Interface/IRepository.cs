using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCTest.Repository
{
    public interface IRepository<TDBContext, TEntity>
        where TDBContext :DbContext
        where TEntity : Entity
    {
        IEnumerable<TEntity> FindAll();
        int Insert(TEntity entity);
        int Update(TEntity entity);
        int Delete(TEntity entity);
        TEntity GetByKey(object key);
    }
}