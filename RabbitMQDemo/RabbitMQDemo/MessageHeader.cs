using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    /// <summary>
    /// 消息头
    /// </summary>
    public class MessageHeader
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MQMessageTypeEnums MQMessageType;
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version = MQConst.MQ_VERSION;
        /// <summary>
        /// 所属类别
        /// </summary>
        public int Category;
        /// <summary>
        /// 自定义命令
        /// </summary>
        public int Command;
        /// <summary>
        /// 随机数
        /// </summary>
        public long TimeStamp;
        /// <summary>
        /// MD5消息校验
        /// </summary>
        public string Token;

    }
}
