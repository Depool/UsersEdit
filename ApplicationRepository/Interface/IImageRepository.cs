using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Models;
using Infrastructure.Repository.Generic.Interface;

namespace ApplicationRepository.Interface
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        Image GetById(int id);

        Image ImageContentToImage(byte[] content, string name);
    }
}
