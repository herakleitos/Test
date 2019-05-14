using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ_Client
{
    class Program
    {
        static MQService mqService;
        static void Main(string[] args)
        {
            //mqService = new MQService();
            //string MQHostName = "localhost";
            //int MQPort = 5672;
            //string MQUserName = "guest";
            //string MQPassword = "guest";
            //string MQExchange = "Demo";
            //mqService.InitMQService(MQHostName, MQPort, MQUserName, MQPassword, MQExchange);
            //new Thread(new ThreadStart(MessageWorker)).Start();
            Console.Read();

        }
        private static void MessageWorker()
        {
            IConnection connEvnet = null;
            IModel channelEvent = null;
            try
            {
                connEvnet = mqService.rabbitMqFactory.CreateConnection();
                channelEvent = connEvnet.CreateModel();

                string queueName = channelEvent.QueueDeclare().QueueName;
                channelEvent.QueueBind(queueName, "Demo", routingKey: queueName);

                var consumer = new EventingBasicConsumer(channelEvent);
                consumer.Received += Consumer_Received;
                channelEvent.BasicConsume(queueName, true, queueName, consumer: consumer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            while (true)
            {
                Thread.Sleep(50);
            }
            //channelEvent.Dispose();
            //connEvnet.Dispose();
        }
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine(string.Format("收到消息：{0}",msg));
        }
    }
}
