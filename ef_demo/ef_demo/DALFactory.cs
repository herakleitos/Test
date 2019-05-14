using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using System.Data.Entity;
namespace ef_demo
{
    /// <summary>
    /// 实体框架的数据访问层接口的构造工厂。
    /// </summary>
    public class DALFactory
    {
        //普通局部变量
        private static Hashtable objCache = new Hashtable();
        private static object syncRoot = new Object();
        private static DALFactory m_Instance = null;

        /// <summary>
        /// IOC的容器，可调用来获取对应接口实例。
        /// </summary>
        public IUnityContainer Container { get; set; }

        /// <summary>
        /// 创建或者从缓存中获取对应业务类的实例
        /// </summary>
        public static DALFactory Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (syncRoot)
                    {
                        if (m_Instance == null)
                        {
                            m_Instance = new DALFactory();
                            //初始化相关的注册接口
                            m_Instance.Container = new UnityContainer();

                            //手工加载
                           // m_Instance.Container.RegisterType<DbContext, DemoDBContext>();
                            m_Instance.Container.RegisterType<IDemoDAL, DemoDAL>();
                        }
                    }
                }
                return m_Instance;
            }
        }
    }
}
