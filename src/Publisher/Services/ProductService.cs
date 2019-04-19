using System;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Publisher.Services.Contracts;

namespace Publisher.Services
{
    public class ProductService : ICreateNewProductService
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
        public void Dispose()
        {
            _client.CloseAsync().Wait();
            GC.SuppressFinalize(this);
        }
    }
}