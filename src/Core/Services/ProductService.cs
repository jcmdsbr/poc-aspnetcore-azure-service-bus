using System;
using System.Text;
using System.Threading.Tasks;
using Core.Configurations;
using Core.Contracts;
using Core.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Core.Services
{
    public class ProductService : ICreateNewProductService,  IProductHandlerService
    {
        private readonly ServiceBusConfiguration _config;
        private readonly IQueueClient _client;
        public ProductService(ServiceBusConfiguration config)
        {
            _config = config;
            _client = new QueueClient(_config.ConnectionString, _config.QueueName, ReceiveMode.ReceiveAndDelete);
        }
        public async Task Add(Product product)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(product));
            var message = new Message(body) 
            {
                Label = "Add"
            };
            await _client.SendAsync(message);
        }

        public async Task Handle() 
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

        public void Dispose()
        {
            _client.CloseAsync().Wait();
            GC.SuppressFinalize(this);
        }
    }
}