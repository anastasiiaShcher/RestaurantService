using Dvor.Common.Interfaces;
using Dvor.DAL.EF;

namespace Dvor.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DvorContext _db;
        private readonly IRepositoryFactory _repositoryFactory;

        public UnitOfWork(DvorContext context, IRepositoryFactory repositoryFactory)
        {
            _db = context;
            _repositoryFactory = repositoryFactory;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return _repositoryFactory.GetRepositoryInstance<T>();
        }

        public IRepository<T> GetRepository<T>(object key) where T : class
        {
            return _repositoryFactory.GetRepositoryInstance<T>(key);
        }
    }
}