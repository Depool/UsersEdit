using ApplicationRepository.Interface;
using ApplicationRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Interface;

namespace ApplicationRepository.Concrete.ADOSql
{
    public class ADOSqlRoleRepository : ADOSqlGenericRepository<Role>, IRoleRepository
    {

    }
}
