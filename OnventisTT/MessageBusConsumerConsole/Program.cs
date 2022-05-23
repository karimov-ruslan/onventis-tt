
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

const string rabbitMQUrl = "amqps://udkduofy:YLAw5tHUDPQ4nUy-d5D88tqUide1TxkN@kangaroo.rmq.cloudamqp.com/udkduofy";
const string rabbitMQQueueName = "Invoices";

var factory = new ConnectionFactory { Uri = new Uri(rabbitMQUrl) };
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(rabbitMQQueueName);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
};

channel.BasicConsume(queue: rabbitMQQueueName, autoAck: true, consumer: consumer);
Console.Read();