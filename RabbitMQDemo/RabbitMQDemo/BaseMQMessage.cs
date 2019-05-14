using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    /// <summary>
    /// 基本消息封装
    /// </summary>
    public class BaseMQMessage
    {
        /// <summary>
        /// 消息头
        /// </summary>
        public MessageHeader Header;
        /// <summary>
        /// 消息体(对象JSON串）
        /// </summary>
        public string Body;
        public BaseMQMessage()
        {
            Header = new MessageHeader();
        }
    }
}
