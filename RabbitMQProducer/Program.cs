using System.Text;
using RabbitMQ.Client;

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

Console.WriteLine("[Producer] Connected to CloudAMQP.");
Console.WriteLine("[Producer] Sending messages...");

for (int i = 1; i <= 5; i++)
{
    var body = Encoding.UTF8.GetBytes($"Hello CloudAMQP message #{i}");
    channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
    Console.WriteLine($"[Producer] Sent: message #{i}");
    await Task.Delay(1000);
}

Console.WriteLine("[Producer] Done.");
