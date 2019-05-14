using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public class FlowMessageBody
    {
        /// <summary>
        /// 关联实例
        /// </summary>
        public string ParallelKeyDigNumGather { get; set; }
        /// <summary>
        /// 事项实例
        /// </summary>
        public string KeyDigNumGather { get; set; }
        /// <summary>
        /// 项目代码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 事件产生时间
        /// </summary>
        public DateTime ActionTime { get; set; }
        /// <summary>
        /// 扩展数据主键
        /// </summary>
        public string ExtKey { get; set; }
    }
}
