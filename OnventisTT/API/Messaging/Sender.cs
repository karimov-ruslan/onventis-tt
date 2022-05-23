using Newtonsoft.Json;
using OnventisTT.Messaging.Abstraction;
using RabbitMQ.Client;
using System.Text;

namespace OnventisTT.Messaging
{
    public class RabbitMqMessageSender : IMessageSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqMessageSender> _logger;

        public RabbitMqMessageSender(IConfiguration configuration, ILogger<RabbitMqMessageSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void SendMessage<T>(T message)
        {
            try
            {
                var url = _configuration["RabbitMQ:Url"];
                var queueName = _configuration["RabbitMQ:QueueName"];

                var factory = new ConnectionFactory { Uri = new Uri(url) };
                var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                //channel.QueueDeclare(queueName);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Could not send a message");
            }
        }
    }
}
