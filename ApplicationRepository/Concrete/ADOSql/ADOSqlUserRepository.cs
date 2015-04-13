using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Models;
using ApplicationRepository.Interface;
using System.Data.SqlClient;
using System.IO;
using Infrastructure.Repository.Generic.Concrete.ADOSql;
using Infrastructure.Repository.EntitiesConverter;
using System.Data;

namespace ApplicationRepository.Concrete.ADOSql
{
    public sealed class ADOSqlUserRepository : ADOSqlGenericRepository<User>, IUserRepository
    {
        private IImageRepository imageRep;
        private IRoleRepository rolesRep;

        public ADOSqlUserRepository() : base()
        {
            imageRep = new ADOSqlImageRepository();
            rolesRep = new ADOSqlRoleRepository();
        }
        public ADOSqlUserRepository(string connStringName) : base(connStringName) 
        {
            imageRep = new ADOSqlImageRepository(connStringName);
            rolesRep = new ADOSqlRoleRepository(connStringName);
        }
        public override IEnumerable<User> GetAll()
        {
            IEnumerable<User> users = base.GetAll();

            IEnumerable<Image> images = imageRep.GetAll();
            IEnumerable<Role> roles = rolesRep.GetAll();

            foreach (User user in users)
            {
                user.Image = images.Where(img => img.Id == user.ImageId).FirstOrDefault();
                user.Role = roles.Where(role => role.Id == user.RoleId).FirstOrDefault();
            }

            return users;
        }

        public User GetById(int id)
        {
            User res = null;
            using (IDataReader reader = dbEnterpriseInstance.ExecuteReader(CommandType.Text, 
                                                                           String.Format("SELECT * FROM [User] WHERE Id = {0}", id)))
            {
                if (reader.Read())
                 res = StaticEntitiesConverter.ReaderRowToEntity<User>(reader);
            }
            if (res.ImageId != null)
                res.Image = imageRep.GetById((int)res.ImageId);

            res.Role = rolesRep.GetById(res.RoleId);

            return res;
        }

        public System.IO.MemoryStream GetPhoto(int id)
        {
            User user = GetById(id);
            if (user == null || user.Image == null)
                return new MemoryStream();
            return new MemoryStream(user.Image.ImageContent.ToArray());
        }

        public override User FindFirst(Func<User, bool> filter)
        {
            User res = base.FindFirst(filter);

            if (res.ImageId != null)
                res.Image = imageRep.GetById((int)res.ImageId);

            res.Role = rolesRep.GetById(res.RoleId);

            return res;
        }
    }
}
