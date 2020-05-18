using Autofac;
using Dvor.Common.Interfaces;

namespace Dvor.DAL.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public RepositoryFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public IRepository<T> GetRepositoryInstance<T>() where T : class
        {
            return _lifetimeScope.Resolve<IRepository<T>>();
        }

        public IRepository<T> GetRepositoryInstance<T>(object key) where T : class
        {
            return _lifetimeScope.ResolveKeyed<IRepository<T>>(key);
        }
    }
}