using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMessages.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("people", ExchangeType.Topic, durable: true);

                channel.QueueDeclare("people_normal", durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind("people_normal", "people", "people.normal");

                channel.QueueDeclare("people_general", durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind("people_general", "people", "people.*");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (object model, BasicDeliverEventArgs ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Received: {message} - {DateTime.Now}");

                    Thread.Sleep(1000);
                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume("people_normal", false, consumer);

                Console.WriteLine("CONSUMER RUNNING!");
                Console.ReadLine();
            }
        }
    }
}
