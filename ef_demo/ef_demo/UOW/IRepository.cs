using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo.UOW
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 获取所有实体对象
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        /// 根据Lamda表达式来查询实体对象
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> Where(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 获取所有实体数量
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 根据表达式获取实体数量
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 实体对象新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(T model, bool IsCommit = false);

        /// <summary>
        /// 实体对象修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(T model, bool IsCommit = false);

        /// <summary>
        /// 实体对象根据字段修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="proName"></param>
        /// <returns></returns>
        int Update(T model, bool IsCommit = false, params string[] proName);

        /// <summary>
        /// 实体对象删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Delete(T model, bool IsCommit = false);

        /// <summary>
        /// 删除复核条件的多个实体对象
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> whereLambda, bool IsCommit = false);
    }
}
