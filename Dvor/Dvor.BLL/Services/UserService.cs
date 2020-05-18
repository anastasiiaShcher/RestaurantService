using System.Collections.Generic;
using System.Linq;
using CryptoHelper;
using Dvor.Common.Entities;
using Dvor.Common.Enums;
using Dvor.Common.Interfaces;
using Dvor.Common.Interfaces.Services;

namespace Dvor.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<User> GetAll()
        {
            return _unitOfWork.GetRepository<User>()
                .GetMany(u => !u.IsDeleted, null, TrackingState.Disabled).ToList();
        }

        public User Get(string id)
        {
            return _unitOfWork.GetRepository<User>().Get(g => g.UserId == id, TrackingState.Disabled, "Allergies");
        }

        public bool IsExist(string id)
        {
            return _unitOfWork.GetRepository<User>().IsExist(source => source.UserId == id);
        }

        public void Create(User item)
        {
            var passwordHash = Crypto.HashPassword(item.PasswordHash);
                item.PasswordHash = passwordHash;
                _unitOfWork.GetRepository<User>().Create(item);
                _unitOfWork.Save();
        }

        public void Update(User item)
        {
            var repository = _unitOfWork.GetRepository<User>();

            if (repository.IsExist(us => us.UserId.Equals(item.UserId)))
            {
                var user = repository.Get(source => source.UserId == item.UserId);
                MapEntities(item, user);

                _unitOfWork.Save();
            }
        }

        public void Delete(string id)
        {
            var user = _unitOfWork.GetRepository<User>().Get(source => source.UserId == id);
            user.IsDeleted = true;
            _unitOfWork.Save();
        }

        public User GetByEmail(string email)
        {
            return _unitOfWork.GetRepository<User>()
                .Get(u => u.Email.Equals(email), TrackingState.Disabled);
        }

        private void MapEntities(User item, User itemToUpdate)
        {
            itemToUpdate.Name = item.Name;

            itemToUpdate.Allergies?.Clear();
            itemToUpdate.Allergies = item.Allergies;
        }
    }
}