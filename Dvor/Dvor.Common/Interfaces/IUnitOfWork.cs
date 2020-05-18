namespace Dvor.Common.Interfaces
{
    public interface IUnitOfWork
    {
        void Save();

        IRepository<T> GetRepository<T>() where T : class;

        IRepository<T> GetRepository<T>(object key) where T : class;
    }
}