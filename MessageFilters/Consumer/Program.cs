using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var EXCHANGE_NAME = "quote";

var factory = new ConnectionFactory() { HostName = "localhost", VirtualHost = "ucl"};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Direct);

FilteredReceive(channel, "Widget", EXCHANGE_NAME);
FilteredReceive(channel, "Gadget", EXCHANGE_NAME);

Console.WriteLine("Press [Enter] to exit.");
Console.ReadLine();

static void FilteredReceive(IModel channel, string filter, string exchangeName)
{
  Console.WriteLine($"{filter} waiting for messages. To exit press CTRL+C");

  var queueName = channel.QueueDeclare().QueueName;
  channel.QueueBind(queueName, exchangeName, filter);

  var consumer = new EventingBasicConsumer(channel);
  consumer.Received += (model, ea) =>
  {
    var body = ea.Body.ToArray();
    var message = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine($"{filter} received message: '{message}'");
  };

  channel.BasicConsume(queueName, autoAck: true, consumer: consumer);
}