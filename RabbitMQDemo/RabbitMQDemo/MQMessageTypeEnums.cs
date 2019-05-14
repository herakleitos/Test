using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MQMessageTypeEnums
    {
        日志记录类_1 = 1,
        消息通知类_2 = 2,
        流程事件类_3 = 3,
        数据分发_4 = 4,
        其他_9 = 9,
        数据转发_10 = 10
    }
}
