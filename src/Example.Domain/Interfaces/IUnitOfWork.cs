using System;
using System.Threading.Tasks;

namespace Example.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
