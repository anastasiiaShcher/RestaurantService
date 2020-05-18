namespace Dvor.Common.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepositoryInstance<T>() where T : class;

        IRepository<T> GetRepositoryInstance<T>(object key) where T : class;
    }
}