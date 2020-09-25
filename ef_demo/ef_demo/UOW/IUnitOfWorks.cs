using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo.UOW
{
    public interface IUnitOfWorks<TContext>
    {
        bool IsCommit { get; set; }//是否自动提交
        /// <summary>
        /// 设置DbContext上下文
        /// </summary>
        /// <param name="context"></param>
        void SetDb(TContext context);
        /// <summary>
        /// 获取所有实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> All<T>() where T : class;

        /// <summary>
        /// 根据Lamda表达式来查询实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> Where<T>(Expression<Func<T, bool>> whereLambda) where T : class;

        /// <summary>
        /// 获取所有实体数量
        /// </summary>
        /// <returns></returns>
        int Count<T>() where T : class;

        /// <summary>
        /// 根据表达式获取实体数量
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        int Count<T>(Expression<Func<T, bool>> whereLambda) where T : class;

        /// <summary>
        /// 实体对象新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add<T>(T model) where T : class;

        /// <summary>
        /// 实体对象修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update<T>(T model) where T : class;

        /// <summary>
        /// 实体对象根据字段修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="proName"></param>
        /// <returns></returns>
        int Update<T>(T model, params string[] proName) where T : class;

        /// <summary>
        /// 实体对象删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Delete<T>(T model) where T : class;

        /// <summary>
        /// 删除复核条件的多个实体对象
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        int Delete<T>(Expression<Func<T, bool>> whereLambda) where T : class;

        /// <summary>
        /// 修改信息提交
        /// </summary>
        /// <returns></returns>
        int SaveChanges(bool validatonSave = true);

        void Dispose();
    }
}
