using System;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IProductHandlerService: IDisposable
    {
         Task Handle();
    }
}