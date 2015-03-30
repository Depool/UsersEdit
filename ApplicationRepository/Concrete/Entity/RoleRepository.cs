using ApplicationRepository.Interface;
using ApplicationRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRepository.Concrete.Entity
{
    public class RoleRepository : GenericRepository<UsersEntities, Role>, IRoleRepository
    {

    }
}
