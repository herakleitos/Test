using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo.UOW
{
    public class Repository<TContext, T> : IRepository<T>
           where TContext : DbContext
           where T : class
    {
        protected TContext context;
        protected DbSet<T> dbSet;
        //protected bool IsCommit;//是否自动提交
        protected T entity;

        public Repository(TContext dbcontext)
        {
            context = dbcontext;
            dbSet = dbcontext.Set<T>();
        }

        /// <summary>
        /// 获取所有实体对象
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> All()
        {
            //context.Set<T>().AsQueryable();
            return dbSet.AsQueryable();
        }

        /// <summary>
        /// 根据Lamda表达式来查询实体对象
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return dbSet.Where(whereLambda);
        }

        /// <summary>
        /// 获取所有实体数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return dbSet.Count();
        }

        /// <summary>
        /// 根据表达式获取实体数量
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public int Count(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return dbSet.Where(whereLambda).Count();
        }

        /// <summary>
        /// 实体对象新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(T model, bool IsCommit = false)
        {
            dbSet.Add(model);
            int i_flag = IsCommit ? context.SaveChanges() : 0;
            return i_flag;
        }

        /// <summary>
        /// 实体对象修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(T model, bool IsCommit = false)
        {
            var entry = context.Entry<T>(model);
            entry.State = EntityState.Modified;
            dbSet.Attach(model);
            int i_flag = IsCommit ? context.SaveChanges() : 0;
            return i_flag;
        }

        /// <summary>
        /// 实体对象根据字段修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="proName"></param>
        /// <returns></returns>
        public int Update(T model, bool IsCommit = false, params string[] proName)
        {
            var entry = context.Entry<T>(model);
            entry.State = EntityState.Unchanged;
            foreach (string s in proName)
            {
                entry.Property(s).IsModified = true;
            }
            int i_flag = IsCommit ? context.SaveChanges() : 0;
            return i_flag;
        }

        /// <summary>
        /// 实体对象删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns
        public int Delete(T model, bool IsCommit = false)
        {
            //var entry = context.Entry<T>(model);
            //entry.State = EntityState.Deleted;
            dbSet.Attach(model);
            dbSet.Remove(model);
            int i_flag = IsCommit ? context.SaveChanges() : 0;
            return i_flag;
        }

        /// <summary>
        /// 删除复核条件的多个实体对象
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public int Delete(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, bool IsCommit = false)
        {
            var enties = dbSet.Where(whereLambda).ToList();
            foreach (var item in enties)
            {
                dbSet.Remove(item);
            }
            int i_flag = IsCommit ? context.SaveChanges() : 0;
            return i_flag;
        }
    }
}
