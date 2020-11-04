using System;
using System.Threading.Tasks;
using Example.Domain.Interfaces;
using Example.Infrastructure.Persistence;

namespace Example.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExampleDbContext _context;

        public UnitOfWork(ExampleDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
