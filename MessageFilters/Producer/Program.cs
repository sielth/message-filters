using System.Text;
using RabbitMQ.Client;

var QUEUE_NAME = "eipqueue";
var EXCHANGE_NAME = "quote";

var factory = new ConnectionFactory() { HostName = "localhost", VirtualHost = "ucl"};
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
  channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Direct);

  SendMessage(channel, EXCHANGE_NAME, "Widget", "Hello EIP Widget!");
  SendMessage(channel, EXCHANGE_NAME, "Gadget", "Hello EIP Gadget!");
}

static void SendMessage(IModel channel, string exchange, string key, string message)
{
  var body = Encoding.UTF8.GetBytes(message);
  channel.BasicPublish(exchange, key, null, body);
  Console.WriteLine($" [x] Sent '{message}'");
}