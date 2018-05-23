using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI
{
    public interface ICalculate
    {
        int Add(int a,int b);
    }
    public class Calculate : ICalculate
    {
        IDeviation _deviation;
        //这种写法会导致Api错误
        //InvalidOperationException: Unable to resolve service for type 'System.Int32' while attempting to activate 'TestAPI.Calculate'.
        //public Calculate(int deviation)
        //{
        //    _deviation = deviation;
        //}
        public Calculate(IDeviation deviation)
        {
            _deviation = deviation;
        }
        public int Add(int a, int b)
        {
            return a + b + _deviation.GetDeviation();
        }
    }
    public interface IDeviation
    {
        int GetDeviation();
    }
    public class Deviation:IDeviation
    {
        public int GetDeviation()
        {
            return 6;
        }
    }
}
