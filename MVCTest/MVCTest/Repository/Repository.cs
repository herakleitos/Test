using MVCTest.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCTest.Repository
{
    public abstract class Repository<TDBContext,TEntity> : IRepository<TDBContext,TEntity> 
        where TDBContext:DbContext
        where TEntity : Entity
    {
        TDBContext _dbContext;
        IDbSet<TEntity> _dataSet;
        public Repository(TDBContext dbContext)
        {
            _dataSet = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }
        public IEnumerable<TEntity> FindAll()
        {
            return _dataSet;
        }
        public int Insert(TEntity entity)
        {
            entity = _dataSet.Add(entity);
            return _dbContext.SaveChanges();
        }
        public int Update(TEntity entity)
        {
            var entry = _dbContext.Entry<TEntity>(entity);
            if (entry.State == EntityState.Detached)
            {
                TEntity orignalEntity = _dataSet.Local.SingleOrDefault(s=>s.Id==entity.Id);
                if (orignalEntity != null)
                {
                    var orignalEntry = _dbContext.Entry(orignalEntity);
                    orignalEntry.CurrentValues.SetValues(orignalEntity);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
            return _dbContext.SaveChanges();
        }
        public int Delete(TEntity entity)
        {
            var entry = _dbContext.Entry<TEntity>(entity);
            if (entry.State == EntityState.Detached)
            {
                TEntity orignalEntity = _dataSet.Local.SingleOrDefault(s => s.Id == entity.Id);
                if (orignalEntity != null)
                {
                    _dataSet.Remove(orignalEntity);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
            return _dbContext.SaveChanges();
        }
        public TEntity GetByKey(object key)
        {
            return _dataSet.Find(key);
        }
    }
}