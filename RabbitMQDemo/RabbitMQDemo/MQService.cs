using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXZX.BL.Core.Encrypt;

namespace RabbitMQDemo
{
    public class MQService
    {
        /// <summary>
        /// 主机名/IP
        /// </summary>
        public string HostName { get; private set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// 默认交换机名
        /// </summary>
        public string Exchange { get; private set; }

        /// <summary>
        /// 消息队列构造参数
        /// </summary>
        public ConnectionFactory rabbitMqFactory { get; private set; }


        /// <summary>
        /// 初始化队列参数
        /// </summary>
        /// <param name="hostName">主机名/IP</param>
        /// <param name="port">端口</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="exchange">默认交换机名</param>
        public void InitMQService(string hostName, int port, string userName, string password, string exchange)
        {
            this.HostName = hostName;
            this.Port = port;
            this.UserName = userName;
            this.Password = password;
            this.Exchange = exchange;

            rabbitMqFactory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = port
            };
        }

        /// <summary>
        /// 解析消息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public T DecodeMessage<T>(BaseMQMessage message) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(message.Body);
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="messageType">消息类型</param>
        /// <param name="data">Body数据</param>
        /// <param name="category">消息类别</param>
        /// <param name="command">命令号</param>
        /// <returns></returns>
        public int SendMessage<T>(T data,string QueueName,int category)
        {
            //fanout广播模式，不需要定义队列
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            using (IModel channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(Exchange, "direct", durable: true, autoDelete: false, arguments: null);
                channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(QueueName, Exchange, routingKey: QueueName);
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                BaseMQMessage message = new BaseMQMessage
                {
                    Header = new MessageHeader
                    {
                        Version = MQConst.MQ_VERSION,
                        MQMessageType =  MQMessageTypeEnums.其他_9,
                        Category = category,
                        Command = 0,
                        TimeStamp = DateTime.Now.Ticks
                    },
                    Body = JsonConvert.SerializeObject(data)
                };
                message.Header.Token = Encrypt.MD5Encrypt(message.Header.TimeStamp.ToString() + ";" + message.Body);
                var msgBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish(exchange: Exchange, routingKey: QueueName, basicProperties: props, body: msgBody);
            }
            return 0;
        }

        /// <summary>
        /// 验证消息是否可靠
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <returns></returns>
        //public bool ValidMessage(BaseMQMessage message)
        //{
        //    string newToken = Encrypt.MD5Encrypt(message.Header.TimeStamp.ToString() + ";" + message.Body);
        //    return message.Header.Token.CompareTo(newToken) == 0;
        //}
    }
}
