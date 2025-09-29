using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Interfaces;
using Infraestructure.Persistence;

namespace Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        public IProductRepository Products { get; }

        public UnitOfWork(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
            Products = new ProductRepository(_dbContext);
        }        
        public IRepository<T> Repository<T>() where T : class
        {
            if(_repositories.ContainsKey(typeof(T)))
                return (IRepository<T>)_repositories[typeof(T)];

            var repo = new Repository<T>(_dbContext);
            _repositories.Add(typeof(T), repo);
            return repo;
        }
        public Task<int> CommitAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
