using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Models;
using ApplicationRepository.Interface;
using ApplicationRepository.EntitiesConverter;
using System.Data.SqlClient;
using System.IO;

namespace ApplicationRepository.Concrete.ADOSql
{
    public sealed class ADOSqlUserRepository : ADOSqlGenericRepository<User>, IUserRepository
    {
        private IImageRepository imageRep;
        private int? maxId = null;

        public ADOSqlUserRepository() : base()
        {
            imageRep = new ADOSqlImageRepository(connectionString);
        }
        public ADOSqlUserRepository(string connString) : base(connString) 
        {
            imageRep = new ADOSqlImageRepository(connectionString);
        }
        public override IEnumerable<User> GetAll()
        {
            IEnumerable<User> users = base.GetAll();

            IEnumerable<Image> images = imageRep.GetAll();

            foreach (User user in users)
            {
                Image correspImage = images.Where(img => img.Id == user.ImageId).FirstOrDefault();
                user.Image = correspImage;
            }

            if (maxId == null)
                maxId = users.Max(usr => usr.Id);

            return users;
        }

        public override void Add(User instance)
        {
            if (maxId == null)
                GetAll();

            instance.Id = (int)maxId + 1;
            maxId++;
            base.Add(instance);
        }

        public User GetById(int id)
        {
            User res = null;
            try
            {
                conn.Open();
                SqlCommand query = new SqlCommand(String.Format("SELECT * FROM [User] WHERE Id = {0}", id), conn);

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        res = StaticEntitiesConverter.ReaderRowToEntity<User>(reader);
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            if (res.ImageId != null)
                res.Image = imageRep.GetById((int)res.ImageId);

            return res;
        }

        public System.IO.MemoryStream GetPhoto(int id)
        {
            User user = GetById(id);
            if (user == null || user.Image == null)
                return new MemoryStream();
            return new MemoryStream(user.Image.ImageContent.ToArray());
        }
    }
}
