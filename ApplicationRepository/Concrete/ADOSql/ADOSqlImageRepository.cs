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

namespace ApplicationRepository.Concrete.ADOSql
{
    public sealed class ADOSqlImageRepository : ADOSqlGenericRepository<Image>, IImageRepository
    {
        public ADOSqlImageRepository() : base() { }
        public ADOSqlImageRepository(string connString) : base(connString) {}

        public override IEnumerable<Image> GetAll()
        {
           IEnumerable<Image> res = base.GetAll();

           return res;
        }

        public Image GetById(int id)
        {
            Image res = null;
            try
            {
                conn.Open();
                SqlCommand query = new SqlCommand(String.Format("SELECT * FROM [Image] WHERE Id = {0}", id), conn);

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        res = StaticEntitiesConverter.ReaderRowToEntity<Image>(reader);
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            //get image

            return res;
        }

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
    }
}
