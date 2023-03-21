using System.Text;
using Identity.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Identity.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(
            IConfiguration configuration, 
            IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"])};

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: _configuration["RabbitExchange"], type: ExchangeType.Fanout);
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName,
                    exchange: _configuration["RabbitExchange"],
                    routingKey: "");

                Console.WriteLine("--> Listenting on the Message Bus...");

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShitdown;                
            }
            catch (System.Exception)
            {
                Console.WriteLine($"--> RabbitMQ is not up - Host: {_configuration["RabbitMQHost"]}, Port:{_configuration["RabbitMQPort"]}");
            }


        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            if(_queueName != null)
            {
                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (ModuleHandle, ea) =>
                {
                    Console.WriteLine("--> Event Received!");

                    var body = ea.Body;
                    var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                    _eventProcessor.ProcessEvent(notificationMessage);
                };

                _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

                //return Task.CompletedTask;
            }

            return Task.CompletedTask;

        }

        private void RabbitMQ_ConnectionShitdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public override void Dispose()
        {
            if(_channel != null)
            {
                if(_channel.IsOpen)
                {
                    _channel.Close();
                    _connection.Close();
                }                
            }
            base.Dispose(); 
        }
    }
}