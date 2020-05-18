using System.Collections.Generic;
using System.Linq;
using Dvor.Common.Entities;
using Dvor.Common.Enums;
using Dvor.Common.Interfaces;

namespace Dvor.BLL.Services
{
    public class ImageService : IService<Image>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Image> GetAll()
        {
            return _unitOfWork.GetRepository<Image>().GetAll(TrackingState.Disabled).ToList();
        }

        public Image Get(string id)
        {
            return _unitOfWork.GetRepository<Image>().Get(source => source.ImageId == id, TrackingState.Disabled);
        }

        public bool IsExist(string id)
        {
            return _unitOfWork.GetRepository<Image>().IsExist(source => source.ImageId == id);
        }

        public void Create(Image item)
        {
            _unitOfWork.GetRepository<Image>().Create(item);
            _unitOfWork.Save();
        }

        public void Update(Image item)
        {
            _unitOfWork.GetRepository<Image>().Update(item);
            _unitOfWork.Save();
        }

        public void Delete(string id)
        {
            _unitOfWork.GetRepository<Image>().Delete(id);
            _unitOfWork.Save();
        }
    }
}