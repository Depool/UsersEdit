using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Models;
using System.IO;
using Infrastructure.Repository.Generic.Interface;

namespace ApplicationRepository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetById(int id);

        MemoryStream GetPhoto(int id);
    }
}
