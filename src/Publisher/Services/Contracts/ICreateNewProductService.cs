using System;
using System.Threading.Tasks;
using Core;

namespace Publisher.Services.Contracts
{
    public interface ICreateNewProductService : IDisposable
    {
         Task Add(Product product);
    }
}