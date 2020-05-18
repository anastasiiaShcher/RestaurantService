using Dvor.Common.Entities;

namespace Dvor.Common.Interfaces
{
    public interface IMailService
    {
        void Send(string email, Notification notification);
    }
}