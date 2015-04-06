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

namespace ApplicationRepository.Concrete.ADOSql
{
    public class ADOSqlRoleRepository : ADOSqlGenericRepository<Role>, IRoleRepository
    {
         public ADOSqlRoleRepository() : base() { }
         public ADOSqlRoleRepository(string connString) : base(connString) {}

         public Role GetById(int id)
         {
            Role res = null;
            try
            {
                conn.Open();
                SqlCommand query = new SqlCommand(String.Format("SELECT * FROM [Role] WHERE Id = {0}", id), conn);

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        res = StaticEntitiesConverter.ReaderRowToEntity<Role>(reader);
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            return res;
        }
    }
}
