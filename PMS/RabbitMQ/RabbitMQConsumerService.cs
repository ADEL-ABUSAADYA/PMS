// namespace PMS.RabbitMQ;
//
// public class RabbitMQConsumerService: IHostedService
// {
//     IConnection _connection;
//     IChannel _channel;
//     public RabbitMQConsumerService()
//     {
//         var factory = new ConnectionFactory{ HostName = "localhost" };
//         _connection = factory.CreateConnectionAsync().Result;
//         _channel = _connection.CreateChannelAsync().Result;
//     }
//     
//     public async Task StartAsync(CancellationToken cancellationToken)
//     {
//         var consumer = new AsyncEventingBasicConsumer(_channel);
//         consumer.ReceivedAsync += Consumer_ReceivedAsync;
//         
//         await _channel.BasicConsumeAsync("Ninja_Q", autoAck:false, consumer: consumer);
//     }
//
//     private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
//     {
//         
//         try
//         {
//             var body = Encoding.UTF8.GetString(@event.Body.ToArray());
//             await _channel.BasicAckAsync(@event.DeliveryTag, multiple: false);
//         }
//         catch (Exception ex) when (ex is NullReferenceException)
//         {
//             await _channel.BasicRejectAsync(@event.DeliveryTag, requeue: false);
//         }
//         catch (Exception ex)
//         {
//             await _channel.BasicRejectAsync(@event.DeliveryTag, requeue: true);
//         }
//         
//     }
//
//     public async Task StopAsync(CancellationToken cancellationToken)
//     {
//         await _channel.CloseAsync();
//         await _connection.CloseAsync();
//   }
// }