using System.Threading;
using System.Threading.Tasks;
using Core.Contracts;
using Microsoft.Extensions.Hosting;

namespace Subscriber
{
    public class QueueListener : IHostedService
    {
        private readonly IProductHandlerService _service;

        public QueueListener(IProductHandlerService service)
        {
            _service = service;
        } 
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _service.Handle();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _service.Dispose());
        }
    }
}