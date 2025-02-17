// namespace PMS.RabbitMQ;
//
// public class RabbitMQPuublisherService
// {
//     IConnection _connection;
//     IChannel _channel;
//     public RabbitMQPuublisherService()
//     {
//         var factory = new ConnectionFactory{ HostName = "localhost" };
//         _connection = factory.CreateConnectionAsync().Result;
//         _channel = _connection.CreateChannelAsync().Result;
//         
//         _channel.ExchangeDeclareAsync("NewExchange", ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
//         
//         _channel.QueueDeclareAsync("Ninja_Q", durable: true, exclusive: false, autoDelete: false, arguments: null);
//         
//         _channel.QueueBindAsync("Ninja_Q", "NewExchange", "Ninja");
//         
//     }
//
//     public async Task Publish(string message)
//     {
//         var body = Encoding.UTF8.GetBytes(message);
//         await _channel.BasicPublishAsync("NewExchange", "Ninja", body);
//     }
// }