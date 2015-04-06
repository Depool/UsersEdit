using ApplicationRepository.Models;
using Infrastructure.Repository.Generic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRepository.Interface
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Role GetById(int id);
    }
}
