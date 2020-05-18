using Dvor.Common.Entities;

namespace Dvor.Common.Interfaces.Services
{
    public interface IUserService : IService<User>
    {
        User GetByEmail(string email);
    }
}