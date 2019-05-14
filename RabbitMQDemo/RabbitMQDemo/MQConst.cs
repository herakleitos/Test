using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public class MQConst
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public const int MQ_VERSION = 1;
        /// <summary>
        /// 日志记录类_1消息队列名
        /// </summary>
        public const string MQ_LOG_QUEUE_NAME = "GZJS_Q_Log";
        /// <summary>
        /// 消息通知类_2消息队列名
        /// </summary>
        public const string MQ_NOTIFY_QUEUE_NAME = "GZJS_Q_Notify";
        /// <summary>
        /// 流程事件类_3消息队列名
        /// </summary>
        public const string MQ_FLOW_QUEUE_NAME = "GZJS_Q_Flow";
        /// <summary>
        /// 数据分发_4消息队列名
        /// </summary>
        public const string MQ_DATA_QUEUE_NAME = "GZJS_Q_Data";
        /// <summary>
        /// 其他_9消息队列名
        /// </summary>
        public const string MQ_OTHER_QUEUE_NAME = "GZJS_Q_Other";
        /// <summary>
        /// 未知类型消息队列名
        /// </summary>
        public const string MQ_UNKOWN_QUEUE_NAME = "GZJS_Q_Unkown";

        /// <summary>
        /// 数据转发_10消息队列名
        /// </summary>
        public const string MQ_TRANSFER_QUEUE_NAME = "GZJS_Q_Transfer";

    }
}
