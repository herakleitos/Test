using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo
{
    public interface IBaseDAL<T> where T:class
    {
        T Get(object id);

        IList<T> GetAll(Expression<Func<T, bool>> whereCondition);

        IList<T> GetAll();
    }
}
