using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace RabbitMQApp.Sender
{
    internal class SenderApp
    {
        static void Main(string[] args)
        { 
            //open a connection to RabbitMQ Server on the destination machine
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())

            //create communication channel and declare the queue
            //create if not exists
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "newBooks",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );




                //place messages at interval of 500ms to the queue
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(3000);
                    string releaseDate = DateTime.Now.AddMonths(-i).ToString();
                    string strBookJson = "{_Id': '6260e73bc363928f414c7765','id': "+ (100 + i) + ",'title': 'DotNet Design Pattersn', 'releaseDate': "+ releaseDate + ",'price': 66.66,'author': 'Tim Tuckey'}";

                    var body = Encoding.UTF8.GetBytes(strBookJson);
                    channel.BasicPublish(exchange: "",
                        routingKey: "newBooks",
                        basicProperties: null,
                        body: body
                        );
                    Console.WriteLine($"New Book published with id: {(100 + i)} :\t Published Date: {releaseDate}");
                }
            }
        }
    }
}
