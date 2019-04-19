using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Subscriber
{
    public class QueueListener : IHostedService
    {
        private readonly IQueueClient _client;
        public QueueListener(ServiceBusConfiguration config)
        {
            _client = new QueueClient(config.ConnectionString, config.QueueName, ReceiveMode.ReceiveAndDelete);
        } 
        public async Task StartAsync(CancellationToken cancellationToken)
        {
           await Task.Run(() => { _client.RegisterMessageHandler(
                       async (message, token) =>
                       {
                           await LoadMessage(message);
                       },
                       new MessageHandlerOptions(
                           async (e) =>
                           {
                               await LoadError(e);
                           }
                       )
                );});
        }

private async Task LoadError(ExceptionReceivedEventArgs e)
        {
            await Task.Run(()=> 
            {
                Console.WriteLine(e.Exception.GetType().FullName + " " +
                                   e.Exception.Message);
           });
        }

        private async Task LoadMessage(Message message)
        {
            await Task.Run(() => {
                Product product = JsonConvert.DeserializeObject<Product>(Encoding.UTF8.GetString(message.Body));
                Console.WriteLine(product);
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await  _client.CloseAsync();
        }
    }
}