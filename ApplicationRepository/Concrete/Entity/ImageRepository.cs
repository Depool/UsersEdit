using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Models;
using ApplicationRepository.Interface;
using Infrastructure.Repository.Generic.Concrete.Entity;

namespace ApplicationRepository.Concrete.Entity
{
    public sealed class ImageRepository : GenericRepository<UsersEntities, Image>, IImageRepository
    {
        public ImageRepository() : base() {}
        public Image ImageContentToImage(byte[] content, string name)
        {
            if (content.Length > 0)
            {
                Image newImage = new Image();

                newImage.ImageName = name;
                newImage.ImageContent = content;
                return newImage;
            }
            else
                return null;
        }

        public Image GetById(int id)
        {
            return context.Set<Image>().Find(id);
        }
    }
}
