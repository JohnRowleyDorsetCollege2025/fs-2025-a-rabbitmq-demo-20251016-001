// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//var uri = Environment.GetEnvironmentVariable("CLOUDAMQP_URL")
//    ?? throw new InvalidOperationException("Missing CLOUDAMQP_URL environment variable.");
var uri = "amqps://ugvmwgak:Jk99nW7GcnUP46OdrD4TqW0kYKalVQyB@ostrich.lmq.cloudamqp.com/ugvmwgak";

var factory = new ConnectionFactory
{
    Uri = new Uri(uri),
    DispatchConsumersAsync = true
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

const string queue = "demo-queue";
channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

Console.WriteLine($"[Consumer] Connected to CloudAMQP. Waiting for messages in '{queue}'...");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.Received += async (_, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($"[Consumer] Received: {message}");
    await Task.Delay(500); // simulate processing
    channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume(queue, autoAck: false, consumer);
await Task.Delay(Timeout.Infinite);

