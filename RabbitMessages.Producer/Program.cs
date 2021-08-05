using RabbitMessages.Producer.Data;
using RabbitMessages.Producer.Models;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;

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

            do
            {
                string name;
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("people", ExchangeType.Topic, durable: true);
                    var person = GetRandomPerson();
                    name = person.Name;
                    var basicProperties = channel.CreateBasicProperties();
                    var routingKey = person.VIP ? "people.vip" : "people.normal";
                    basicProperties.Persistent = true;

                    channel.BasicPublish(exchange: "people",
                        routingKey: routingKey,
                        basicProperties: basicProperties,
                        body: ObjectToArray(person));
                }

                Console.WriteLine($"PRODUCER RAN - {name} - {DateTime.Now}");
                Thread.Sleep(1000);
            } while (true);
        }

        private static Person GetRandomPerson()
        {
            return new PeopleFactory().GetRandom();
        }

        static byte[] ObjectToArray(object objeto)
        {
            string serializado = JsonSerializer.Serialize(objeto);
            return Encoding.UTF8.GetBytes(serializado);
        }
    }
}
