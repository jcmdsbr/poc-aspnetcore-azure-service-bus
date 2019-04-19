using System;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Contracts
{
    public interface ICreateNewProductService : IDisposable
    {
         Task Add(Product product);
    }
}