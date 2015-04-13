using ApplicationRepository.Interface;
using ApplicationRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Interface;
using System.Data.SqlClient;
using Infrastructure.Repository.Generic.Concrete.ADOSql;
using Infrastructure.Repository.EntitiesConverter;
using System.Data;

namespace ApplicationRepository.Concrete.ADOSql
{
    public class ADOSqlRoleRepository : ADOSqlGenericRepository<Role>, IRoleRepository
    {


         public ADOSqlRoleRepository() : base() { }
         public ADOSqlRoleRepository(string connStringName) : base(connStringName) {}

         public Role GetById(int id)
         {
            Role res = null;
            using (IDataReader reader = dbEnterpriseInstance.ExecuteReader(CommandType.Text, 
                                                                          String.Format("SELECT * FROM [Role] WHERE Id = {0}", id)))
            {
                if (reader.Read())
                 res = StaticEntitiesConverter.ReaderRowToEntity<Role>(reader);
            }
            return res;
        }
    }
}
