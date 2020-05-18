using System.Collections.Generic;

namespace Dvor.Common.Interfaces
{
    public interface IService<T> where T : class
    {
        IList<T> GetAll();

        T Get(string id);

        bool IsExist(string id);

        void Create(T item);

        void Update(T item);

        void Delete(string id);
    }
}