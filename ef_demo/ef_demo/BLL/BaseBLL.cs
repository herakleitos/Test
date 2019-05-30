using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ef_demo
{
    /// <summary>
    /// 业务逻辑基类
    /// </summary>
    /// <typeparam name="T">实体对象类型</typeparam>
    public abstract class BaseBLL<T> : IBaseBLL<T> where T : class
    {
        private static readonly object syncRoot = new Object();
        protected IBaseDAL<T> baseDAL { get; set; }
        protected IUnityContainer container { get; set; }

        /// <summary>
        /// 默认构造函数。
        /// 默认获取缓存的容器，如果没有则创建容器，并注册所需的接口实现。
        /// </summary>
        public BaseBLL()
        {
            lock (syncRoot)
            {
                container = DALFactory.Instance.Container;
                if (container == null)
                {
                    throw new ArgumentNullException("container", "container没有初始化");
                }
            }
        }

        public BaseBLL(IBaseDAL<T> dal)
        {
            this.baseDAL = dal;
        }

        public T Get(object id)
        {
            return baseDAL.Get(id);
        }

        public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return baseDAL.GetAll(whereCondition);
        }

        public IList<T> GetAll()
        {
            return baseDAL.GetAll();
        }
        public void BulkInsert(IEnumerable<T> datas)
        {
            baseDAL.BulkInsert(datas);
        }
    }
}
