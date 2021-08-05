using RabbitMQ.Client;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace RabbitMessages.Producer
{
    class Program
    {

        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory { HostName = "localhost" };
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using(var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    Object objPessoa = new
                    {
                        Nome = "Diego Doná",
                        DataDeNascimento = new DateTime(1989, 6, 19),
                        Id = Guid.NewGuid(),
                        Momento = DateTime.Now
                    };

                    var basicProperties = channel.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;

                    channel.BasicPublish(exchange: "",
                        routingKey: "hello",
                        basicProperties: basicProperties,
                        body: ObjetoParaArray(objPessoa));
                }
            }

            Console.WriteLine("Hello World!");
        }

        static byte[] ObjetoParaArray(object objeto)
        {
            string serializado = JsonSerializer.Serialize(objeto);
            return Encoding.UTF8.GetBytes(serializado);
        }
    }
}
